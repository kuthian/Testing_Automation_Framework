namespace Jalapeno
{
    public class Program
    {
        public static Session Session1 = new Session(Properties.Settings.Default.COMPORT);
        //public static Session Session2 = new Session(Properties.Settings.Default.COMPORT);

        public static void Main(string[] args)
        {
            while (Session1.TG300.Connected)
            {
            }
        }
    }
}

