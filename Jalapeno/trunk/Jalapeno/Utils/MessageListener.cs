using System;
using Jalapeno.Messaging;

namespace Jalapeno.Utils
{
    public class MessageListener
    {
        public MessageHandler.PacketTypeEnum PacketTypeFlagged;
        public MessagePacketData.CommandTypeEnum CommandTypeFlagged;

        public bool CommandTypeFlag;
        public bool PacketTypeFlag;

        public event EventHandler<MessageEventArgs> MsgTypeReceived; 

        public MessageListener()
        {
            CommandTypeFlag = false;
            PacketTypeFlag = false;
        }

        //MsgTypeReceived publisher (raises the event)
        public virtual void OnMsgTypeReceived(Messaging.MessageHandler.PacketTypeEnum PktType, Messaging.MessagePacketData.CommandTypeEnum CmdType)
        {
            if (MsgTypeReceived != null)
            {
                MsgTypeReceived(this, new MessageEventArgs() { CommandType = CmdType, PacketType = PktType });
            }
        }

        //Suscribed to the MsgTypeReceived event when EnableMsgListener has been called
        public void OnCommandTypeReceived(object source, MessageEventArgs Args)
        {
            if (Args.CommandType == CommandTypeFlagged)
            {
                CommandTypeFlag = true;
            }
        }

        //Suscribed to the MsgTypeReceived event when EnableMsgListener has been called
        public void OnPacketTypeReceived(object source, MessageEventArgs Args)
        {
            if (Args.PacketType == PacketTypeFlagged)
            {
                PacketTypeFlag = true;
            }
        }

        //Suscribes OnCommandTypeReceived and OnPacketTypeReceived to the MsgTypeReceived event
        public void EnableMsgListener()
        {
            MsgTypeReceived += OnCommandTypeReceived;
            MsgTypeReceived += OnPacketTypeReceived;
        }

    }
}
