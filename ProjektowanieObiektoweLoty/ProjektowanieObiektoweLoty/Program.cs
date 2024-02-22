using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjektowanieObiektoweLoty
{
    public class Program
    {
        private static string FilePath = "C:\\Users\\marci\\Downloads\\example_data.ftr";
        static CrewCreator CrewFactory;
        static PassengerCreator PassengerFactory;
        static CargoCreator CargoFactory;
        static CargoPlaneCreator CarogPlaneFactory;
        static PassengerPlaneCreator PassengerPlaneFactory;
        static AirportCreator AirportFactory;
        static FlightCreator FlightFactory;
        public static List<Crew> CrewObjectList;
        public static List<Passenger> PassengerObjectList;
        public static List<Cargo> CargoObjectList;
        public static List<CargoPlane> CargoPlaneObjectList;
        public static List<PassengerPlane> PassengerPlaneObjectList;
        public static List<Airport> AirportObjectList;
        public static List<Flight> FlightObjectList;
        private static void ReadFromFtrFile(string FilePathArg)
        {
            String line;
            try
            {
                StreamReader sr = new StreamReader(FilePath);

                line = sr.ReadLine();

                while (line != null)
                {

                   
                    string[] ObjectParameters = line.Split(',');
                    string ClassShortName = ObjectParameters[0];
                    try
                    {
                        switch (ClassShortName)
                        {
                            case "C":
                                CrewObjectList.Add(CrewFactory.Create(ObjectParameters));
                 
                                break;
                            case "P":
                                PassengerObjectList.Add(PassengerFactory.Create(ObjectParameters));
                                
                                break;
                            case "CA":
                                CargoObjectList.Add(CargoFactory.Create(ObjectParameters));
                                break;
                            case "CP":
                                CargoPlaneObjectList.Add(CarogPlaneFactory.Create(ObjectParameters));
                                break;
                            case "PP":
                                PassengerPlaneObjectList.Add(PassengerPlaneFactory.Create(ObjectParameters));
                                break;
                            case "AI":
                                AirportObjectList.Add(AirportFactory.Create(ObjectParameters));
                                break;
                            case "FL":
                                FlightObjectList.Add(FlightFactory.Create(ObjectParameters));
                                break;

                        }
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("Exception: " + e.Message);

                    }

                    line = sr.ReadLine();
                }
                

                sr.Close();
                
            }
            
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            
            
        }
        private static void CreateFactoryClasses()
        {
            CrewFactory = new CrewCreator();
            PassengerFactory = new PassengerCreator();
            CargoFactory = new CargoCreator();
            CarogPlaneFactory = new CargoPlaneCreator();
            PassengerPlaneFactory = new PassengerPlaneCreator();
            AirportFactory = new AirportCreator();
            FlightFactory = new FlightCreator();         
        }
        public static void Serialize()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(SerializeList(AirportObjectList));
            sb.Append("\n");
            sb.Append(SerializeList(CargoPlaneObjectList));
            sb.Append("\n");
            sb.Append(SerializeList(PassengerPlaneObjectList));
            sb.Append("\n");
            sb.Append(SerializeList(CargoObjectList));
            sb.Append("\n");
            sb.Append(SerializeList(CrewObjectList));
            sb.Append("\n");
            sb.Append(SerializeList(PassengerObjectList));
            sb.Append("\n");
            sb.Append(SerializeList(FlightObjectList));
            sb.Append("\n");
            string jsonString = sb.ToString();
            using (StreamWriter outputFile = new StreamWriter("jsonSerialize.json"))
            {
               outputFile.Write(jsonString);
            }

        }
        public static string SerializeList<T>(List<T> ObjectList)
        {
            StringBuilder sb = new StringBuilder();
            foreach(var Object in ObjectList)
            {
                sb.Append(Newtonsoft.Json.JsonConvert.SerializeObject(Object));
                sb.Append("\n");
            }
            return sb.ToString();
        }
        private static void InitializeLists()
        {
            CrewObjectList = new List<Crew>();
            PassengerObjectList = new List<Passenger>();
            CargoObjectList = new List<Cargo>();
            CargoPlaneObjectList = new List<CargoPlane>();
            PassengerPlaneObjectList = new List<PassengerPlane>();
            AirportObjectList = new List<Airport>();
            FlightObjectList = new List<Flight>();
        }
        private static void SetCulture()
        {
            CultureInfo newCulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = newCulture;
            CultureInfo.DefaultThreadCurrentUICulture = newCulture;
        }
        static void Main()
        {
            SetCulture();
            InitializeLists();
            CreateFactoryClasses();
            ReadFromFtrFile(FilePath);
            Serialize();
            
        }
    }
}
