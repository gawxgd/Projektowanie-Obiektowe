using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektowanieObiektoweLoty
{
    public static class CreateFactories
    {
        public static CrewCreator CrewFactory;
        public static PassengerCreator PassengerFactory;
        public static CargoCreator CargoFactory;
        public static CargoPlaneCreator CargoPlaneFactory;
        public static PassengerPlaneCreator PassengerPlaneFactory;
        public static AirportCreator AirportFactory;
        public static FlightCreator FlightFactory;
        public static Dictionary<string, IFTRCreator> MatchClassNameWithFactoryDict;
        public static Dictionary<string, IFTRCreator> MatchClassNameWithFactoryDictNetwork;
        public static readonly Dictionary<string, string> MatchClassNameWithShortClassName = new Dictionary<string, string>()
        {
            {"Crew","C"},
            {"Passenger","P" },
            {"Cargo","CA"},
            {"CargoPlane","CP" },
            {"PassengerPlane","PP" },
            {"Airport","AI" },
            {"Flight","FL" }
        };
        public static void CreateFactoryClasses()
        {
            CrewFactory = new CrewCreator();
            PassengerFactory = new PassengerCreator();
            CargoFactory = new CargoCreator();
            CargoPlaneFactory = new CargoPlaneCreator();
            PassengerPlaneFactory = new PassengerPlaneCreator();
            AirportFactory = new AirportCreator();
            FlightFactory = new FlightCreator();
        }
        public static void FillFactoryDictionary()
        {
            MatchClassNameWithFactoryDict = new Dictionary<string, IFTRCreator>();
            MatchClassNameWithFactoryDict.Add("C", CrewFactory);
            MatchClassNameWithFactoryDict.Add("P", PassengerFactory);
            MatchClassNameWithFactoryDict.Add("CA", CargoFactory);
            MatchClassNameWithFactoryDict.Add("CP", CargoPlaneFactory);
            MatchClassNameWithFactoryDict.Add("PP", PassengerPlaneFactory);
            MatchClassNameWithFactoryDict.Add("AI", AirportFactory);
            MatchClassNameWithFactoryDict.Add("FL", FlightFactory);
        }
        public static void FillNetworkDictionary()
        {
            MatchClassNameWithFactoryDictNetwork = new Dictionary<string, IFTRCreator>();
            MatchClassNameWithFactoryDictNetwork.Add("NCR", CrewFactory);
            MatchClassNameWithFactoryDictNetwork.Add("NPA", PassengerFactory);
            MatchClassNameWithFactoryDictNetwork.Add("NCA", CargoFactory);
            MatchClassNameWithFactoryDictNetwork.Add("NCP", CargoPlaneFactory);
            MatchClassNameWithFactoryDictNetwork.Add("NPP", PassengerPlaneFactory);
            MatchClassNameWithFactoryDictNetwork.Add("NAI", AirportFactory);
            MatchClassNameWithFactoryDictNetwork.Add("NFL", FlightFactory);
        }
    }
}
