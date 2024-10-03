using SkiaSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektowanieObiektoweLoty
{
    public class CreateFtrObject
    {
        public static ThreadSafeList<Flight> FlightsList = new ThreadSafeList<Flight>();
        public static ThreadSafeList<Airport> AirportsList = new ThreadSafeList<Airport>();
        public static ThreadSafeList<Cargo> CargoList = new ThreadSafeList<Cargo>();
        public static ThreadSafeList<Passenger> PassengersList = new ThreadSafeList<Passenger>();
        public static ThreadSafeList<Crew> CrewList = new ThreadSafeList<Crew>();
        public static ThreadSafeList<CargoPlane> CargoPlaneList = new ThreadSafeList<CargoPlane>();
        public static ThreadSafeList<PassengerPlane> PassengerPlanesList = new ThreadSafeList<PassengerPlane>();

        public static List<Flight> FlightsListFTR = new List<Flight>();
        public static List<Airport> AirportsListFTR = new List<Airport>();
        public static List<Cargo> CargoListFTR = new List<Cargo>();
        public static List<Passenger> PassengersListFTR = new List<Passenger>();
        public static List<Crew> CrewListFTR = new List<Crew>();
        public static List<CargoPlane> CargoPlaneListFTR = new List<CargoPlane>();
        public static List<PassengerPlane> PassengerPlanesListFTR = new List<PassengerPlane>();
        
        public static ConcurrentDictionary<ulong,Flight> IDtoFlight = new ConcurrentDictionary<ulong, Flight>();
        public static ConcurrentDictionary<ulong, Airport> IDtoAirport = new ConcurrentDictionary<ulong, Airport>();
        public static ConcurrentDictionary<ulong, Cargo> IDtoCargo = new ConcurrentDictionary<ulong, Cargo>();
        public static ConcurrentDictionary<ulong, Passenger> IDtoPassenger = new ConcurrentDictionary<ulong, Passenger>();
        public static ConcurrentDictionary<ulong, Crew> IDtoCrew = new ConcurrentDictionary<ulong, Crew>();
        public static ConcurrentDictionary<ulong, CargoPlane> IDtoCargoPlane = new ConcurrentDictionary<ulong, CargoPlane>();
        public static ConcurrentDictionary<ulong,PassengerPlane> IDtoPassengerPlane = new ConcurrentDictionary<ulong, PassengerPlane>();

        public static ConcurrentDictionary<string, ConcurrentDictionary<ulong, IFtr>> TypeNameToDict = new ConcurrentDictionary<string, ConcurrentDictionary<ulong, IFtr>>();

        public static void CreateObjectFromDictionaryNetwork(string ClassShortName, byte[] byteObjectParameters, Dictionary<string,IFTRCreator> CreatorDictionary)
        {
            if (CreatorDictionary.TryGetValue(ClassShortName, out var FactoryObject))
            {
                if (FactoryObject is null)
                    throw new Exception("there is no class with this short name");
                if (byteObjectParameters != null)
                {
                    var CreatedObject = FactoryObject.Create(byteObjectParameters);
                    Program.NetworkObjectList.Add(CreatedObject);
                    CreatedObject.AddToList();
                }
                else
                    throw new ArgumentNullException("object parameters are null");
            }
            else
            {
                Console.WriteLine(ClassShortName);
                throw new Exception("there is no class with this short name");
            }
        }
        public static void CreateObjectFromDictionaryFtr(string ClassShortName, string[] stringObjectParameters, Dictionary<string, IFTRCreator> CreatorDictionary)
        {
            if (CreatorDictionary.TryGetValue(ClassShortName, out var FactoryObject))
            {
                if (FactoryObject is null)
                    throw new Exception("there is no class with this short name");
                if (stringObjectParameters != null)
                {
                    var CreatedObject = FactoryObject.Create(stringObjectParameters);
                    Program.NetworkObjectList.Add(CreatedObject); // weird stage
                    //CreatedObject.AddToListFTR(); if basic network simulator
                    CreatedObject.AddToList();
                }
                else
                    throw new ArgumentNullException("object parameters are null");
            }
            else
            {
                Console.WriteLine(ClassShortName);
                throw new Exception("there is no class with this short name");
            }
        }
        public static bool IsIdTaken(ulong ID)
        {
            if (IDtoFlight.ContainsKey(ID))
                return true;
            if (IDtoAirport.ContainsKey(ID))
                return true;
            if (IDtoCargo.ContainsKey(ID))
                return true;
            if (IDtoPassenger.ContainsKey(ID))
                return true;
            if (IDtoCrew.ContainsKey(ID))
                return true;
            if (IDtoCargoPlane.ContainsKey(ID))
                return true;
            if (IDtoPassengerPlane.ContainsKey(ID))
                return true;
            return false;
        }
        public static void InitalizeIdDictionaries()
        {
            IDtoFlight = new ConcurrentDictionary<ulong, Flight>();
            IDtoAirport = new ConcurrentDictionary<ulong, Airport>();
            IDtoCargo = new ConcurrentDictionary<ulong, Cargo>();
            IDtoPassenger = new ConcurrentDictionary<ulong, Passenger>();
            IDtoCrew = new ConcurrentDictionary<ulong, Crew>();
            IDtoCargoPlane = new ConcurrentDictionary<ulong, CargoPlane>();
            IDtoPassengerPlane = new ConcurrentDictionary<ulong, PassengerPlane>();
            foreach (var airport in AirportsList)
            {
                if(IDtoAirport.TryAdd(airport.ID, airport) == false)
                {
                    Console.WriteLine($"key already exists airport {airport.ID} {IDtoAirport[airport.ID].Name}");
                }
            }
            foreach (var item in FlightsList)
            {
                if (IDtoFlight.TryAdd(item.ID, item) == false)
                {
                    Console.WriteLine($"key already exists {item.ID}");
                }
            }
            foreach (var item in CargoList)
            {
                if (IDtoCargo.TryAdd(item.ID, item) == false)
                {
                    Console.WriteLine($"key already exists {item.ID}");
                }
            }
            foreach (var item in PassengersList)
            {
                if (IDtoPassenger.TryAdd(item.ID, item) == false)
                {
                    Console.WriteLine($"key already exists {item.ID}");
                }
            }
            foreach (var item in CrewList)
            {
                if (IDtoCrew.TryAdd(item.ID, item) == false)
                {
                    Console.WriteLine($"key already exists {item.ID}");
                }
            }
            foreach (var item in CargoPlaneList)
            {
                if (IDtoCargoPlane.TryAdd(item.ID, item) == false)
                {
                    Console.WriteLine($"key already exists {item.ID}");
                }
            }
            foreach (var item in PassengerPlanesList)
            {
                if (IDtoPassengerPlane.TryAdd(item.ID, item)==false)
                {
                    Console.WriteLine($"key already exists {item.ID}");
                }
            }
            TypeNameToDict["Flight"] = new ConcurrentDictionary<ulong, IFtr>(IDtoFlight.ToDictionary(kvp => kvp.Key, kvp => (IFtr)kvp.Value));
            TypeNameToDict["Airport"] = new ConcurrentDictionary<ulong, IFtr>(IDtoAirport.ToDictionary(kvp => kvp.Key, kvp => (IFtr)kvp.Value));
            TypeNameToDict["Cargo"] = new ConcurrentDictionary<ulong, IFtr>(IDtoCargo.ToDictionary(kvp => kvp.Key, kvp => (IFtr)kvp.Value));
            TypeNameToDict["Passenger"] = new ConcurrentDictionary<ulong, IFtr>(IDtoPassenger.ToDictionary(kvp => kvp.Key, kvp => (IFtr)kvp.Value));
            TypeNameToDict["Crew"] = new ConcurrentDictionary<ulong, IFtr>(IDtoCrew.ToDictionary(kvp => kvp.Key, kvp => (IFtr)kvp.Value));
            TypeNameToDict["CargoPlane"] = new ConcurrentDictionary<ulong, IFtr>(IDtoCargoPlane.ToDictionary(kvp => kvp.Key, kvp => (IFtr)kvp.Value));
            TypeNameToDict["PassengerPlane"] = new ConcurrentDictionary<ulong, IFtr>(IDtoPassengerPlane.ToDictionary(kvp => kvp.Key, kvp => (IFtr)kvp.Value));
        }
    }
}
