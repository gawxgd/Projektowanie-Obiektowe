using Newtonsoft.Json.Serialization;
using ProjektowanieObiektoweLoty.FQL;
using ProjektowanieObiektoweLoty.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektowanieObiektoweLoty
{
    public static class ConsoleUser
    {
        public static readonly object _serializeLock = new object();
        private static string CreateSnapshotFileName()
        {
            var date = DateTime.Now;
            return $"snapshot_{date.Hour}_{date.Minute}_{date.Second}.json";
        }
        private static NewsCollection NCreporter = new NewsCollection();
        private static IMediaIterator iterator = NCreporter.CreateIterator();
        public static void Run()
        {
            FQL.Parser parser = new FQL.Parser();
            bool cond = true;
            while(cond)
            {
                Console.WriteLine("type 'print' to make snapshot, 'exit' to exit, 'gui' to start GUI, 'report' to generate news");
                string userInput = Console.ReadLine();
                switch (userInput)
                {
                    case "print":
                        Console.WriteLine("made snapshot");
                        lock(_serializeLock)
                        {
                            SerializeJson.Serialize(Program.NetworkObjectList,CreateSnapshotFileName());
                        }
                        break;
                    case "exit":
                        Console.WriteLine("exititng");
                        try
                        {
                            Tools.UpdateLog.PrintToFile();
                            Environment.Exit(0);
                            //ConnectWithNetwork.GetInstance().CloseConnection();
                        }
                        catch(Exception e) 
                        {
                            Console.WriteLine(e);
                        }
                        cond = false;
                        break;
                    case "gui":
                        
                        var GUI = GUIcontrol.GetInstance();
                        try
                        {
                            Task.Run(() => GUI.RunGUI());
                        }
                        catch(Exception ex) 
                        {
                            Console.WriteLine(ex.Message);
                            Console.WriteLine(ex.StackTrace);
                        }
                        break;
                    case "report":
                        Console.WriteLine(iterator.GenerateNextNews());
                        break;
                    case "all":
                        iterator.DisplayALL();
                        break;
                    default:
                        parser.ParseFQL(userInput);
                        break;
                      
                   
                }
            
            }
        }
    }
}
