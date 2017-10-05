using System;
using System.Collections.Generic;
using System.Diagnostics;
using ZylSerialPort;
using Jalapeno.Utils;
using Jalapeno.Messaging;
using Jalapeno.Messaging.Messages;
using Jalapeno.Config;

namespace Jalapeno
{
    public class SerialLink
    {
        private UtilThread EventThread;
        private ZylSerialPort.SerialPort CurrentSerialPort;
        private Queue<byte[]> ReceivedData;
        public Queue<Message> MessagesToSend;
        bool KeepAliveRequest;
        public int NumUnconfirmedKeepAlives;
        private UtilTimer KeepAliveTimer;
        public bool Connected;
        public bool IncompleteFlag;
        public Queue<byte[]> IncompleteReceivedData;
        public FullConfiguration CurrentConfiguration;

        public Utils.MessageListener MsgListener;

        //---------------------------------------------------------------------------------------------
        // EVENTS
        //---------------------------------------------------------------------------------------------

        public event EventHandler ConnectionStateChange;

        public event EventHandler MessageReceived;

        public event EventHandler MessageSent;
       

        //constructor
        public SerialLink()
        {
            MessagesToSend = new Queue<Message>();
            ReceivedData = new Queue<byte[]>();
            IncompleteReceivedData = new Queue<byte[]>();
            CurrentConfiguration = new FullConfiguration();
            MsgListener = new MessageListener();
            EventThread = null;
            Connected = false;
            KeepAliveRequest = false;

            NumUnconfirmedKeepAlives = 0;          
        }

        //Initializes the connection to a specified port
        public void ConnectSerialPort(String ComPortName)
        {           
            try
            {
                Console.WriteLine("Attempting to connect to " + ComPortName);
                if (CurrentSerialPort == null)
                {
                    InitSerialConnection(ComPortName);
                }

                KeepAliveRequest = true;
                InitEventThread();
                
                //Ensure that the COM PORT connection is open before publishing connection state
                if (CurrentSerialPort.Open()) 
                {
                    Console.WriteLine(ComPortName +" Connection Established");
                    Queue_ConnectionStateChange_Connected();
                    InitKeepAliveTimer(2000);
                    Console.WriteLine("...Connected...\n");
                }
                else
                {
                    throw new Exception("Error 0: COM Port is closed");
                }
            }
            catch(Exception e)
            {              
                Debug.WriteLine(e.Message);
                DisconnectSerialPort();
            } 
        }

        //method that queues the OnTimerElapsed event, which in turn will raise the KeepAwake event
        protected void Queue_OnTimerElapsed(object sender, EventArgs e)
        {
            QueueEventThread(OnTimerElapsed, sender, e);
        }

        // method that queues event threads
        protected void QueueEventThread(EventHandler target, object sender, EventArgs args)
        {
            lock (this)

            {   // add Event "target" to EventThread queue 
                if (EventThread != null)
                {
                    EventThread.Post(target, sender, args); 
                }
            }
        }

        // Subscribed to OnTimer event, method that raises the KeepAwake event
        void OnTimerElapsed(object sender, EventArgs e)
        {
            if (KeepAliveTimer != null)
            {
                KeepAliveTimer.Start(); 
                KeepAliveRequest = true;
                Queue_KeepAwake(); 
            }
        }

        //Methods adds KeepAwake event to event queue 
        protected void Queue_KeepAwake()
        {
            QueueEventThread(KeepAwake, this, EventArgs.Empty);
        }

        //subscribed to the OnTimer Event through wakeup() method, handles wakeups
        private void KeepAwake(object sender, EventArgs e)
        {      
            if (Connected)
            {
                try
                {
                    DoConnectionTasks();
                }       
                catch
                {
                    Debug.WriteLine("Error 4: Disconnecting");
                    DisconnectSerialPort();
                }
            }
        }

        //Changes the connection state to "connected" and queues the OnConnectionStateChange event
        protected void Queue_ConnectionStateChange_Connected()
        {
            QueueEventThread(ConnectionStateChange, this, EventArgs.Empty);  //there might not be any subscribers
            Connected = true;       
        }

         protected void DoConnectionTasks()
        {
          bool Awake = true;

          while(Awake)
          {
            Awake = false;

            try
            {
              if(CurrentSerialPort.ConnectedTo != SerialPort.SerialCommPort.None)
              {
                if(DoReceiving())
                {
                  Awake = true;
                }

                if(DoSending())
                {
                  Awake = true;
                }

                if(KeepAliveRequest)
                {
                  if(NumUnconfirmedKeepAlives < 3)
                  {
                    KeepAliveMessage MessageToSend = new KeepAliveMessage();
                    byte[] bytes = MessageToSend.MessagePacket;
                    CurrentSerialPort.SendByteArray(bytes);            

                    KeepAliveRequest = false;
                    NumUnconfirmedKeepAlives++;
                  }
                  else
                  {
    #if !IGNORE_USB_NO_RESPONSE
                      
                      throw new Exception("No USB Response");
    #endif
                  }
                }
              }
              else
              {               
                throw new Exception("Error 2: Disconnecting");               
              }
            }
            catch (Exception e)
            {
              Connected = false;
              DisconnectSerialPort();

              Debug.WriteLine(e.Message);

            }
          }
        }

        private void OnReceived(object sender, ZylSerialPort.DataEventArgs e)
        {
            lock (ReceivedData)
            {
                ReceivedData.Enqueue(e.Buffer); 
            }
        }

        protected void DisconnectSerialPort()
        {
            if (KeepAliveTimer != null)
            {
                KeepAliveTimer.Stop();
                KeepAliveTimer = null;
            }
            try
            {
                Debug.WriteLine("...Closing Port...\n");             
                CurrentSerialPort.Close();
                Connected = false; // testing
                Debug.WriteLine("...Port Closed...\n");  
            }
            catch
            {
                // do nothing
            }
            try
            {
                CurrentSerialPort.Dispose();
                CurrentSerialPort = null;
            }
            catch
            {
                // do nothing
            }

            Debug.WriteLine("Disconnected");
        }

        //Send byteArray using ZylSerialPort Method
        public void SendData(byte[] bytes)
        {
            try
            {
                if (Connected)
                {
                    Debug.WriteLine("Sening Byte Array");
                    CurrentSerialPort.SendByteArray(bytes);
                    
                }
                else
                {
                    throw new Exception("Error 1: COM port is closed, Data cannot be sent.");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        // Attempts to receive a message. Returns true if a message was received.
        private bool DoReceiving()
        {        
            bool Awake = false;

            byte[] ReceivedDataBuffer = null;

            lock (ReceivedData)
            {
                //if the Received data queue contains a message, dequeue into the buffer in order to read message
                if (ReceivedData.Count > 0)
                {
                    ReceivedDataBuffer = ReceivedData.Dequeue();
                    
                }
            }

            //Checks 
            if (ReceivedDataBuffer != null && ReceivedDataBuffer.Length > 0)
            {               
                MessageHandler ReceivedMessage = new MessageHandler();

                //If incomplete has not been flagged, do normal operation
                if (!IncompleteFlag)
                {
                    ReceivedMessage.DePacketizeMessage(this, ReceivedDataBuffer);
                }
                else
                {
                    ReceivedMessage.CheckIfCompleteMessage(ReceivedDataBuffer);
                }

                //If incomplete message is detected and SerialLink has not yet flagged incomplete, queue incomplete data and flag Incomplete
                if (!ReceivedMessage.CompleteMessage && !IncompleteFlag)
                {
                    IncompleteReceivedData.Enqueue(ReceivedDataBuffer);
                    IncompleteFlag = true; //telling SerialLink that next message received could be incomplete and will need to be buffered                
                    NumUnconfirmedKeepAlives = 0;

                    Awake = true;
                }
                //Operations when IncompleteFlag has been raised
                else if (!ReceivedMessage.CompleteMessage && IncompleteFlag)
                {
                    IncompleteReceivedData.Enqueue(ReceivedDataBuffer);
                    ReceivedMessage.DePacketizeIncompleteMessage(this, ReceivedDataBuffer);
                    NumUnconfirmedKeepAlives = 0;
                   
                    if (ReceivedMessage.CompleteMessage)
                    {
                        IncompleteFlag = false;
                        IncompleteReceivedData.Clear();
                    } 
                    
                    Awake = true;
                }
                // if a complete message is received in between incomplete fragments, it can still be consumed and parsed
                else if (ReceivedMessage.CompleteMessage && IncompleteFlag)
                {               
                    ReceivedMessage.DePacketizeMessage(this, ReceivedDataBuffer);
                    NumUnconfirmedKeepAlives = 0;

                    Awake = true;
                }
 
                //If a valid message was received, the device is determined to be awake and count is reset
                if (ReceivedMessage.PacketType != MessageHandler.PacketTypeEnum.None)
                {
                    NumUnconfirmedKeepAlives = 0;
                    QueueEventThread(MessageReceived, this, EventArgs.Empty);

                    Awake = true;
                }

            }

            return Awake;
        }

        // Attempts to send a message. Returns true if a message was sent
        private bool DoSending()
        {
            bool Awake = false;

            //Checks if there is a message in the queue 
            if (MessagesToSend != null && MessagesToSend.Count > 0)
            {
                //packetize and send message
                Message message = MessagesToSend.Dequeue();

                byte[] bytes = message.MessagePacket;
                CurrentSerialPort.SendByteArray(bytes);
                Awake = true;

                Debug.WriteLine("...Message Sent... \n" + BitConverter.ToString(bytes)+"\n");
            }

            QueueEventThread(MessageSent, this, EventArgs.Empty);
            return Awake;
        }

        private bool InitEventThread()
        {
            bool Success = false;

            if (EventThread == null)
            {
                EventThread = new UtilThread("tempName");
                EventThread.Start();
                Success = true;
            }

            return Success;
        }

        private void MyThread_Connect(object sender, EventArgs e)
        {
            //PublishConnectionState(Connected, null);
            Connected = true;
            //MyThread_HandleConnect();
        }

        private void InitKeepAliveTimer(int TimerSetting)
        {
            KeepAliveTimer = new UtilTimer(null);
            KeepAliveTimer.TimerElapsed += Queue_OnTimerElapsed;
            KeepAliveTimer.Start(TimerSetting);
        }

        private void InitSerialConnection(String COM)
        {
            CurrentSerialPort = new SerialPort(SerialPort.StringToSerialCommPort(COM), SerialPort.SerialDataWidth.dw8Bits, SerialPort.SerialStopBits.sb1Bit, SerialPort.SerialParityBits.pbNone);
            CurrentSerialPort.BaudRate = SerialPort.SerialBaudRate.br019200;
            CurrentSerialPort.Received += OnReceived;
        }

    }


}
