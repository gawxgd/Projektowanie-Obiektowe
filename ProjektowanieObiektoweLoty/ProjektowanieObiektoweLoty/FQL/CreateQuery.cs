using Avalonia.Controls.Templates;
using DynamicData;
using NetTopologySuite.Triangulate;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tmds.DBus.Protocol;
using static ProjektowanieObiektoweLoty.FQL.CreateQuery;


namespace ProjektowanieObiektoweLoty.FQL
{
    public class CreateQuery
    {
        public delegate string Get<T>(T item) where T : IFtr;
        public static readonly Dictionary<string, Get<Airport>> AirportGetField = new Dictionary<string, Get<Airport>>()
        {
            {"ID", airport => airport.ID.ToString() },
            {"Name", airport => airport.Name},
            {"Code", airport => airport.Code},
            {"WorldPosition.Long", airport => airport.Longtitude.ToString()},
            {"WorldPosition.Lat", airport => airport.Latitude.ToString() },
            {"WorldPosition", airport => airport.Latitude.ToString() + " " + airport.Longtitude },
            {"AMSL",airport => airport.AMSL.ToString() },
            {"CountryCode",airport => airport.Country }
        };
        public static readonly Dictionary<string, Get<Crew>> CrewGetField = new Dictionary<string, Get<Crew>>()
        {
            {"ID", crew => crew.ID.ToString()},
            {"Name", crew => crew.Name},
            {"Age", crew => crew.Age.ToString()},
            {"Phone", crew => crew.Phone},
            {"Email", crew => crew.Email},
            {"Practice", crew => crew.Practice.ToString()},
            {"Role", crew => crew.Role}
        };
        public static readonly Dictionary<string, Get<Passenger>> PassengerGetField = new Dictionary<string, Get<Passenger>>()
        {
            {"ID", passenger => passenger.ID.ToString()},
            {"Name", passenger => passenger.Name},
            {"Age", passenger => passenger.Age.ToString()},
            {"Phone", passenger => passenger.Phone},
            {"Email", passenger => passenger.Email},
            {"Class", passenger => passenger.Class},
            {"Miles", passenger => passenger.Miles.ToString()}
        };
        public static readonly Dictionary<string, Get<Cargo>> CargoGetField = new Dictionary<string, Get<Cargo>>()
        {
            {"ID", cargo => cargo.ID.ToString()},
            {"Weight", cargo => cargo.Weight.ToString()},
            {"Code", cargo => cargo.Code},
            {"Description", cargo => cargo.Description}
        };
        public static readonly Dictionary<string, Get<CargoPlane>> CargoPlaneGetField = new Dictionary<string, Get<CargoPlane>>()
        {
            {"ID", plane => plane.ID.ToString()},
            {"Serial",plane => plane.Serial },
            {"CountryCode",plane => plane.Country },
            {"Model", plane => plane.model},
            {"MaxLoad",plane => plane.MaxLoad.ToString() },
        };
        public static readonly Dictionary<string, Get<PassengerPlane>> PassengerPlaneGetField = new Dictionary<string, Get<PassengerPlane>>()
        {
            {"ID", passengerPlane => passengerPlane.ID.ToString()},
            {"Serial",plane => plane.Serial },
            {"CountryCode",plane => plane.Country },
            {"Model", passengerPlane => passengerPlane.model},
            {"FirstClassSize",plane => plane.FirstClassSize.ToString()},
            {"BusinessClassSize",plane => plane.BusinessSize.ToString()},
            {"EconomyClassSize",plane => plane.EconomyClassSize.ToString()},
        };
        public static readonly Dictionary<string, Get<Flight>> FlightGetField = new Dictionary<string, Get<Flight>>()
        {
            {"ID", flight => flight.ID.ToString()},
            {"Origin", flight => flight.OriginAsID.ToString()},
            {"Target", flight => flight.OriginAsID.ToString()},
            {"TakeoffTime", flight => flight.TakeOffTime.ToString()},
            {"LandingTime", flight => flight.LandingTime.ToString()},
            {"WorldPosition.Long", flight => flight.Longtitude.ToString()},
            {"WorldPosition.Lat", flight => flight.Latitude.ToString() },
            {"WorldPosition", flight => flight.Latitude.ToString() + " " + flight.Longtitude },
            {"AMSL",flight => flight.AMSL.ToString() },
            {"Plane",flight => flight.PlaneID.ToString() },
        };
        public static readonly Dictionary<string, Func<string, IComparable>> FtrObjectGetFieldValue = new Dictionary<string, Func<string, IComparable>>()
        {
            {"ID", (fieldValue) => ulong.Parse(fieldValue) },
            {"Name",(fieldValue) => fieldValue },
            {"Code",(fieldValue) => fieldValue },
            {"WorldPosition",(fieldValue) => {var temp = fieldValue.Split(' ');
                return new WorldPositionC(Single.Parse(temp[0]),Single.Parse(temp[1]));}},
            {"WorldPosition.Long",(fieldValue) => fieldValue},
            {"WorldPosition.Lat",(fieldValue) => fieldValue},
            {"AMSL",(fieldValue) => float.Parse(fieldValue)},
            {"CountryCode",(fieldValue) => fieldValue },
            {"Age",(f) => uint.Parse(f)},
            {"Phone",(f) => f},
            {"Email",(f) => f},
            {"Practice",(f) => uint.Parse(f)},
            {"Role",(f) =>f },
            {"Class",(f) => f },
            {"Miles",(f) =>uint.Parse(f)},
            {"Weight",(f) => float.Parse(f)},
            //{"Code",(f)=>f },
            {"Description",(f)=>f },
            {"Serial",(f)=>f },
            {"Model",(f)=>f },
            {"MaxLoad",(f) => float.Parse(f) },
            {"FirstClassSize",(f)=>uint.Parse(f) },
            {"BusinessClassSize",(f)=>uint.Parse(f) },
            {"EconomyClassSize",(f)=>uint.Parse(f) },
            {"[Origin as ID]",(f) => uint.Parse(f) },
            {"[Target as ID]",(f) => uint.Parse(f) },
            {"LandingTime",(f) => f},
            {"TakeoffTime",(f) => f},
            {"PlaneID",(f)=>uint.Parse(f) },
        };
        public delegate void Set<T>(T item, string value) where T : IFtr;
        public static readonly Dictionary<string, Set<Airport>> AirportSetField = new Dictionary<string, Set<Airport>>()
        {
            {"ID", (airport, value) => airport.ID = ulong.Parse(value)},
            {"Name", (airport, value) => airport.Name = value},
            {"Code", (airport, value) => airport.Code = value},
            {"WorldPosition.Long", (airport, value) => airport.Longtitude = float.Parse(value)},
            {"WorldPostition.Lat", (airport, value) => airport.Latitude = float.Parse(value)},
            {"AMSL", (airport, value) => airport.AMSL = float.Parse(value)},
            {"CountryCode", (airport, value) => airport.Country = value}
        };
        public static readonly Dictionary<string, Set<Crew>> CrewSetField = new Dictionary<string, Set<Crew>>()
        {
            {"ID", (crew, value) => crew.ID = ulong.Parse(value)},
            {"Name", (crew, value) => crew.Name = value},
            {"Age", (crew, value) => crew.Age = ulong.Parse(value)},
            {"Phone", (crew, value) => crew.Phone = value},
            {"Email", (crew, value) => crew.Email = value},
            {"Practice", (crew, value) => crew.Practice = ushort.Parse(value)},
            {"Role", (crew, value) => crew.Role = value}
        };
        public static readonly Dictionary<string, Set<Passenger>> PassengerSetField = new Dictionary<string, Set<Passenger>>()
        {
            {"ID", (passenger, value) => passenger.ID = ulong.Parse(value)},
            {"Name", (passenger, value) => passenger.Name = value},
            {"Age", (passenger, value) => passenger.Age = ulong.Parse(value)},
            {"Phone", (passenger, value) => passenger.Phone = value},
            {"Email", (passenger, value) => passenger.Email = value},
            {"Class", (passenger, value) => passenger.Class = value},
            {"Miles", (passenger, value) => passenger.Miles = ulong.Parse(value)}
        };

        public static readonly Dictionary<string, Set<Cargo>> CargoSetField = new Dictionary<string, Set<Cargo>>()
        {
            {"ID", (cargo, value) => cargo.ID = ulong.Parse(value)},
            {"Weight", (cargo, value) => cargo.Weight = float.Parse(value)},
            {"Code", (cargo, value) => cargo.Code = value},
            {"Description", (cargo, value) => cargo.Description = value}
        };

        public static readonly Dictionary<string, Set<CargoPlane>> CargoPlaneSetField = new Dictionary<string, Set<CargoPlane>>()
        {
            {"ID", (plane, value) => plane.ID = ulong.Parse(value)},
            {"Serial", (plane, value) => plane.Serial = value},
            {"CountryCode", (plane, value) => plane.Country = value},
            {"Model", (plane, value) => plane.model = value},
            {"MaxLoad", (plane, value) => plane.MaxLoad = float.Parse(value)}
        };

        public static readonly Dictionary<string, Set<PassengerPlane>> PassengerPlaneSetField = new Dictionary<string, Set<PassengerPlane>>()
        {
            {"ID", (passengerPlane, value) => passengerPlane.ID = ulong.Parse(value)},
            {"Serial", (plane, value) => plane.Serial = value},
            {"CountryCode", (plane, value) => plane.Country = value},
            {"Model", (passengerPlane, value) => passengerPlane.model = value},
            {"FirstClassSize", (plane, value) => plane.FirstClassSize = ushort.Parse(value)},
            {"BusinessClassSize", (plane, value) => plane.BusinessSize = ushort.Parse(value)},
            {"EconomyClassSize", (plane, value) => plane.EconomyClassSize = ushort.Parse(value)}
        };

        public static readonly Dictionary<string, Set<Flight>> FlightSetField = new Dictionary<string, Set<Flight>>()
        {
            {"ID", (flight, value) => flight.ID = ulong.Parse(value)},
            {"Origin", (flight, value) => flight.OriginAsID = ulong.Parse(value)},
            {"Target", (flight, value) => flight.OriginAsID = ulong.Parse(value)},
            {"TakeoffTime", (flight, value) => flight.TakeOffTime = DateTime.Parse(value)},
            {"LandingTime", (flight, value) => flight.LandingTime = DateTime.Parse(value)},
            {"WorldPosition.Long", (flight, value) => flight.Longtitude = float.Parse(value)},
            {"WorldPostition.Lat", (flight, value) => flight.Latitude = float.Parse(value)},
            {"AMSL", (flight, value) => flight.AMSL = float.Parse(value)},
            {"Plane", (flight, value) => flight.PlaneID = ulong.Parse(value)}
        };
    }
}
