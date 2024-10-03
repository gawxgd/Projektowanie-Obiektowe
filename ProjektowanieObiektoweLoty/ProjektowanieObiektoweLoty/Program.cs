using ProjektowanieObiektoweLoty.Media;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace ProjektowanieObiektoweLoty
{
    public class Program
    {
        private static readonly string FilePath = "example_data.ftr";
        private static readonly string NetworkFilePath = "example.ftre";
        private static readonly string FtrSerializationFileName = "ftrJson.json";
        private static readonly string NetworkSerializationFileName = "networkJson.json";

        public static List<FtrObject> FtrObjectList;
        public static List<FtrObject> NetworkObjectList;

        static int minOffsetInMins = 100;
        static int maxOffsetInMins = 500;
        private static Thread ConsoleThread;

        public static EventManger eventManager;

        private static void InitializeLists()
        {
            FtrObjectList = new List<FtrObject>();
            NetworkObjectList = new List<FtrObject>();
        }
        private static void SetCulture()
        {
            CultureInfo newCulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = newCulture;
            CultureInfo.DefaultThreadCurrentUICulture = newCulture;
        }
        static void Main()
        {
            // Projekt 1
            SetCulture();
            InitializeLists();
            eventManager = new EventManger();
            CreateFactories.CreateFactoryClasses();
            CreateFactories.FillFactoryDictionary();
            ReadFromFile.ReadFromFtrFile(FilePath);
            SerializeJson.Serialize(NetworkObjectList,FtrSerializationFileName);
            CreateFtrObject.InitalizeIdDictionaries();
            Console.WriteLine("data read");

            ConsoleThread = new Thread(() => ConsoleUser.Run());
            ConsoleThread.Start();
            //Projekt 2
            //CreateFactories.FillNetworkDictionary();
            //ConsoleThread = new Thread(() => ConsoleUser.Run());
            //ConsoleThread.Start();
            //ConnectWithNetwork ConnectWithNetworkObject = ConnectWithNetwork.GetInstance();
            //ConnectWithNetworkObject.EstabilishConnection(NetworkFilePath,minOffsetInMins,maxOffsetInMins);
            //try
            //{
            //    ReciveMessagesViaNetwork MessageReciver = new ReciveMessagesViaNetwork(ConnectWithNetworkObject);
            //    MessageReciver.SubscribeNewDataReadyEvent();
            //    ConnectWithNetworkObject.StartListeningToServer();
            //}
            //catch (Exception ex) 
            //{
            //    Console.WriteLine(ex.ToString());
            //}
            //ConsoleThread.Join();
            //SerializeJson.Serialize(NetworkObjectList,NetworkSerializationFileName);
            //Etap 3
            //Etap 5
            
            ConnectWithNetwork ConnectWithNetworkObject = ConnectWithNetwork.GetInstance();
            ConnectWithNetworkObject.EstabilishConnection(NetworkFilePath, minOffsetInMins, maxOffsetInMins);
            try
            {
                ReciveMessagesViaNetwork MessageReciver = new ReciveMessagesViaNetwork(ConnectWithNetworkObject,eventManager);
                ConnectWithNetworkObject.StartListeningToServer();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            ConnectWithNetworkObject.JoinConnection();
            ConsoleThread.Join();
            Tools.UpdateLog.PrintToFile();
        }
    }
}
