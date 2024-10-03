using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace ProjektowanieObiektoweLoty
{
    public class ConnectWithNetwork
    {
        private ConnectWithNetwork() { }
        private static ConnectWithNetwork _instance;
        private static readonly object _lock = new object();
        private NetworkSourceSimulator.NetworkSourceSimulator NetworkConnectionObject;
        private Thread ConnectionThread;

        public static ConnectWithNetwork GetInstance()
        {
            if(_instance == null)
            {
                lock(_lock) 
                {
                    if (_instance == null)
                    {
                        _instance = new ConnectWithNetwork();
                    }
                }
            }
            return _instance;
        }
        public void EstabilishConnection(string FTRfilePath, int minOffsetInMins,int maxOffsetInMins)
        {
            if (NetworkConnectionObject == null)
            {
                NetworkSourceSimulator.NetworkSourceSimulator NetworkConnection = new NetworkSourceSimulator.NetworkSourceSimulator(FTRfilePath, minOffsetInMins, maxOffsetInMins);
                NetworkConnectionObject = NetworkConnection;
            }
        }
        public void StartListeningToServer()
        {
            try
            {
                if (NetworkConnectionObject == null)
                    throw new Exception("no network connection object");
                try
                {
                    ConnectionThread = new Thread(() => NetworkConnectionObject.Run());
                }
                catch(Exception ex)
                { }
                ConnectionThread.Start();
                Console.WriteLine("thread started");
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
        }
        public NetworkSourceSimulator.NetworkSourceSimulator GetConnectionObject()
        {
            if (NetworkConnectionObject == null)
                throw new Exception("connection not estabilished");
            else
                return NetworkConnectionObject;
        }
        public void CloseConnection()
        {
            if (NetworkConnectionObject == null)
                throw new Exception("connection already closed");
            else
            {
                try
                {
                    ConnectionThread.Interrupt();
                }
                catch(Exception ex) { }
                NetworkConnectionObject = null;
            }
        }
        public void JoinConnection()
        {
            if (NetworkConnectionObject == null)
                throw new Exception("connection already closed");
            else
            {
                Console.WriteLine("waiting to recive objects ");
                ConnectionThread.Join();
                NetworkConnectionObject = null;
            }
        }
    }
}
