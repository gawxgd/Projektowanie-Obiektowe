
using NetworkSourceSimulator;
using ProjektowanieObiektoweLoty.FQL;
using ProjektowanieObiektoweLoty.Media;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektowanieObiektoweLoty
{
    public class Airport : FtrObject, IReportable
    {
        public string Name;
        public string Code;
        public Single Longtitude;
        public Single Latitude;
        public Single AMSL;
        public string Country;
        public Airport(): base() { }
        public Airport(string className, UInt64 iD, string name, string code, Single longtitude, Single latitude, Single aMSL, string country) : base(className, iD)
        {
            Name = name;
            Code = code;
            Longtitude = longtitude;
            Latitude = latitude;
            AMSL = aMSL;
            Country = country;
        }
        public override void AddToList()
        {
            CreateFtrObject.AirportsList.Add(this);
        }
        public override void AddToListFTR()
        {
            CreateFtrObject.AirportsListFTR.Add(this);
        }
        public string ReportFor(IMedia media)
        {
            return media.DoForAirport(this);
        }
        public override void updateOnIDEvent(IDUpdateArgs args)
        {
            foreach(Flight flight in CreateFtrObject.IDtoFlight.Values) 
            {
                if(flight.TargetAsID == args.ObjectID)
                {
                    flight.TargetAsID = args.NewObjectID;
                }else if(flight.OriginAsID == args.ObjectID)
                {
                    flight.OriginAsID = args.NewObjectID;
                }
            }
            Program.eventManager.IDEventUnsubscribe(this);
            Airport tempAirport;
            CreateFtrObject.IDtoAirport.Remove(args.ObjectID, out tempAirport);
            tempAirport.ID = args.NewObjectID;
            if(CreateFtrObject.IDtoAirport.TryAdd(tempAirport.ID,tempAirport) == false)
            {
                Console.WriteLine($"failed to update {tempAirport.ID}");
            }
            Program.eventManager.IDEventSubscribe(this);
            Tools.UpdateLog.AddUpdate(tempAirport.ClassName,args.ObjectID,tempAirport.ID);
        }
        public override bool EvaluateFQL(string cond)
        {
            return FilterTables.EvaluateCondition<Airport>(cond, this, CreateQuery.AirportGetField);
        }
        public override string GetField(string field)
        {
            return CreateQuery.AirportGetField[field].Invoke(this);
        }
        public override IFtr Cast()
        {
            return this;
        }
        public override string[] GetAllFields()
        {
            List<string> fields = new List<string>();
            foreach(var field in CreateQuery.AirportGetField.Values)
            {
                fields.Add(field.Invoke(this));
            }
            return fields.ToArray();
        }
        public override void SetField(string value,string field) 
        {
            CreateQuery.AirportSetField[field].Invoke(this, value);
        }
        public override string[] GetFieldNames()
        {
            List<string> fields = new List<string>();
            foreach(var item in CreateQuery.AirportGetField.Keys)
            {
                fields.Add(item);
            }
            return fields.ToArray();
        }
    }
    public class AirportCreator : CreatorFTR
    {
        static private readonly int FieldCount = 8;
        public override Airport Create(string[] ObjectParameters)
        {
            TestIfAllArguments(FieldCount, ObjectParameters.Length);
            string ClassName = ObjectParameters[0];
            UInt64 ID = UInt64.Parse(ObjectParameters[1]);
            string Name = ObjectParameters[2];
            string Code = ObjectParameters[3];
            Single Longtitude = Single.Parse(ObjectParameters[4]);
            Single Latitude = Single.Parse(ObjectParameters[5]);
            Single AMSL = Single.Parse(ObjectParameters[6]);
            string Country = ObjectParameters[7];
            return new Airport(ClassName, ID, Name, Code, Longtitude, Latitude, AMSL, Country);
        }

        public override Airport Create(byte[] ObjectParameters)
        {
            char[] classID = Encoding.ASCII.GetChars(ObjectParameters[0..3]);
            string classShortName = new string(classID);
            UInt32 FollowingMessageLength = BitConverter.ToUInt32(ObjectParameters[3..7]);
            UInt64 ID = BitConverter.ToUInt64(ObjectParameters[7..15]);
            UInt16 NL = BitConverter.ToUInt16(ObjectParameters[15..17]);
            char[] Name = Encoding.ASCII.GetChars(ObjectParameters[17..(17 + NL)]);
            char[] Code = Encoding.ASCII.GetChars(ObjectParameters[(17 + NL)..(17 + NL + 3)]);
            Single Longtitude = BitConverter.ToSingle(ObjectParameters[(20 + NL)..(20 + NL + 4)]);
            Single Latitude = BitConverter.ToSingle(ObjectParameters[(24 + NL)..(24 + NL + 4)]);
            Single AMSL = BitConverter.ToSingle(ObjectParameters[(28 + NL)..(28 + NL + 4)]);
            char[] ISOC = Encoding.ASCII.GetChars(ObjectParameters[(32 + NL)..(32 + NL + 3)]);
            return new Airport(classShortName, ID,new string(Name),new string(Code),Longtitude,Latitude, AMSL, new string(ISOC));
        }
        public override Airport Create()
        {
            return new Airport();
        }
    }
}
