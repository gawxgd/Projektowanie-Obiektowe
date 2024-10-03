
using DynamicData;
using NetTopologySuite.Geometries;
using NetworkSourceSimulator;
using ProjektowanieObiektoweLoty.FQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektowanieObiektoweLoty
{
    public class Flight : FtrObject
    {
        public UInt64 OriginAsID;
        public UInt64 TargetAsID;
        public DateTime TakeOffTime;
        public DateTime LandingTime;
        public Single Longtitude;
        public Single Latitude;
        public Single AMSL;
        public UInt64 PlaneID;
        public UInt64[] CrewAsIDs;
        public UInt64[] LoadAsIDs;
        public long FlightDuration;
        public DateTime UpdatedTakeoff;
        public Flight() { }
        public Flight(string className, UInt64 iD, UInt64 originAsID, UInt64 targetAsID, Int64 takeOffTime, Int64 landingTime, Single longtitude, Single latitude, Single aMSL, UInt64 planeID, UInt64[] crewAsIDs, UInt64[] loadAsIDs) : base(className, iD)
        {
            OriginAsID = originAsID;
            TargetAsID = targetAsID;
            TakeOffTime = DateTimeOffset.FromUnixTimeMilliseconds(takeOffTime).UtcDateTime;
            LandingTime = DateTimeOffset.FromUnixTimeMilliseconds(landingTime).UtcDateTime;
            UpdatedTakeoff = TakeOffTime;
            try
            {
                var airport = CreateFtrObject.IDtoAirport[originAsID];
                Longtitude = airport.Longtitude;
                Latitude = airport.Latitude;
            }
            catch(Exception e) 
            {
                Longtitude = longtitude;
                Latitude = latitude;
            }
            AMSL = aMSL;
            PlaneID = planeID;
            CrewAsIDs = crewAsIDs;
            LoadAsIDs = loadAsIDs;
            FlightDuration = landingTime - takeOffTime;
            Program.eventManager.PositionEventSubscribe(this);
        }
        public Flight(string className, UInt64 iD, UInt64 originAsID, UInt64 targetAsID, string takeOffTime, string landingTime, Single longtitude, Single latitude, Single aMSL, UInt64 planeID, UInt64[] crewAsIDs, UInt64[] loadAsIDs) : base(className, iD)
        {
            OriginAsID = originAsID;
            TargetAsID = targetAsID;
            TakeOffTime = DateTime.Parse(takeOffTime);
            LandingTime = DateTime.Parse(landingTime);
            UpdatedTakeoff = TakeOffTime;
            try
            {
                var airport = CreateFtrObject.IDtoAirport[originAsID];
                Longtitude = airport.Longtitude;
                Latitude = airport.Latitude;
            }
            catch (Exception e)
            {
                Longtitude = longtitude;
                Latitude = latitude;
            }
            AMSL = aMSL;
            PlaneID = planeID;
            CrewAsIDs = crewAsIDs;
            LoadAsIDs = loadAsIDs;
            FlightDuration = (long)(LandingTime - TakeOffTime).TotalMilliseconds;
            Program.eventManager.PositionEventSubscribe(this);
        }
        public override void AddToList()
        {
            CreateFtrObject.FlightsList.Add(this);
        }
        public override void AddToListFTR()
        {
            CreateFtrObject.FlightsListFTR.Add(this);
        }
        public override void updateOnIDEvent(IDUpdateArgs args)
        {
          
            Flight temp;
            Program.eventManager.IDEventUnsubscribe(this);
            CreateFtrObject.IDtoFlight.Remove(args.ObjectID, out temp);
            temp.ID = args.NewObjectID;
            if (CreateFtrObject.IDtoFlight.TryAdd(temp.ID, temp) == false)
            {
                Console.WriteLine($"failed to update {temp.ID}");
            }
            Program.eventManager.IDEventSubscribe(this);
            Tools.UpdateLog.AddUpdate(temp.ClassName, args.ObjectID, temp.ID);
        }
        public override void updateOnPositionEvent(PositionUpdateArgs args)
        {
            Tools.UpdateLog.AddUpdate(ClassName, ID, Longtitude, Latitude, args.Longitude, args.Latitude, AMSL, args.AMSL);
            Latitude = args.Latitude;
            Longtitude = args.Longitude;
            AMSL = args.AMSL;
            UpdatedTakeoff = DateTime.UtcNow;
            FlightDuration = (long)(LandingTime - UpdatedTakeoff).TotalMilliseconds;
        }
        public override bool EvaluateFQL(string cond)
        {
            return FilterTables.EvaluateCondition<Flight>(cond, this, CreateQuery.FlightGetField);
        }
        public override string GetField(string field)
        {
            return CreateQuery.FlightGetField[field].Invoke(this);
        }
        public override IFtr Cast()
        {
            return this;
        }
        public override string[] GetAllFields()
        {
            List<string> fields = new List<string>();
            foreach (var field in CreateQuery.FlightGetField.Values)
            {
                fields.Add(field.Invoke(this));
            }
            return fields.ToArray();
        }
        public override void SetField(string value, string field)
        {
            CreateQuery.FlightSetField[field].Invoke(this, value);
        }
        public override string[] GetFieldNames()
        {
            List<string> fields = new List<string>();
            foreach (var item in CreateQuery.FlightGetField.Keys)
            {
                fields.Add(item);
            }
            return fields.ToArray();
        }
    }
    public class FlightCreator : CreatorFTR
    {
        static private readonly int FieldCount = 12;
        public override Flight Create(string[] ObjectParameters)
        {
            TestIfAllArguments(FieldCount, ObjectParameters.Length);
            string ClassName = ObjectParameters[0];
            UInt64 ID = UInt64.Parse(ObjectParameters[1]);
            UInt64 OriginAsId = UInt64.Parse(ObjectParameters[2]);
            UInt64 TargetAsId = UInt64.Parse(ObjectParameters[3]);
            string TakeOffTime = ObjectParameters[4];
            string LandingTime = ObjectParameters[5];
            Single Longtitude = Single.Parse(ObjectParameters[6]);
            Single Latitude = Single.Parse(ObjectParameters[7]);
            Single AMSL = Single.Parse(ObjectParameters[8]);
            UInt64 PlaneID = UInt64.Parse(ObjectParameters[9]);
            string[] CrewAsIDsString = ObjectParameters[10].Trim(new char[] { '[', ']' }).Split(';');
            UInt64[] CrewAsIDs = new UInt64[CrewAsIDsString.Length];
            int j = 0;
            foreach (var i in CrewAsIDsString)
            {
                CrewAsIDs[j++] = UInt64.Parse(i);
            }
            string[] LoadAsIDsString = ObjectParameters[11].Trim(new char[] { '[', ']' }).Split(';');
            UInt64[] LoadAsIDs = new UInt64[LoadAsIDsString.Length];
            j = 0;
            foreach (var i in LoadAsIDsString)
            {
                LoadAsIDs[j++] = UInt64.Parse(i);
            }
            return new Flight(ClassName, ID, OriginAsId, TargetAsId, TakeOffTime, LandingTime, Longtitude, Latitude, AMSL, PlaneID, CrewAsIDs, LoadAsIDs);
        }
        public override Flight Create(byte[] ObjectParameters)
        {
            char[] classID = Encoding.ASCII.GetChars(ObjectParameters[0..3]);
            string classShortName = new string(classID);
            UInt32 FollowingMessageLength = BitConverter.ToUInt32(ObjectParameters[3..7]);
            UInt64 ID = BitConverter.ToUInt64(ObjectParameters[7..15]);
            UInt64 Origin = BitConverter.ToUInt64(ObjectParameters[15..(23)]);
            UInt64 Target = BitConverter.ToUInt64(ObjectParameters[23..(31)]);
            Int64 takeoff = BitConverter.ToInt64(ObjectParameters[31..(39)]);
            Int64 landing = BitConverter.ToInt64(ObjectParameters[39..(47)]);
            UInt64 PlaneID = BitConverter.ToUInt64(ObjectParameters[47..(55)]);
            UInt16 CrewCount = BitConverter.ToUInt16(ObjectParameters[55..(57)]);
            UInt64[] Crew = new UInt64[CrewCount];
            int j = 0;
            for (int i=0;i<CrewCount; i++)
            {
                Crew[i] = BitConverter.ToUInt64(ObjectParameters[(57 + j)..(57 + j + 8)]);
                j += 8;
            }
            UInt16 PCCount = BitConverter.ToUInt16(ObjectParameters[(57+j)..(57 + j + 2)]);
            UInt64[] PC = new UInt64[PCCount];
            int k = 0;
            for (int i = 0; i < PCCount; i++)
            {
                PC[i] = BitConverter.ToUInt64(ObjectParameters[(57 + j + k)..(57 + j + k + 8)]);
                k += 8;
            }
            return new Flight(classShortName, ID,Origin,Target, takeoff, landing,0,0,0,PlaneID,Crew,PC); // uwaga 0
        }

        public override Flight Create()
        {
            return new Flight();
        }
    }

}
