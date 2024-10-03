
using DynamicData;
using NetworkSourceSimulator;
using ProjektowanieObiektoweLoty.FQL;
using ProjektowanieObiektoweLoty.Media;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektowanieObiektoweLoty
{
    public abstract class Plane : FtrObject
    {
        public string Serial;
        public string Country;
        public string model;
        public Plane() { }
        public Plane(string className, UInt64 iD, string serial, string country, string model) : base(className, iD)
        {
            Serial = serial;
            Country = country;
            this.model = model;
        }
    }
    public class CargoPlane : Plane, IReportable
    {
        public Single MaxLoad;
        public CargoPlane() { }
        public CargoPlane(string className, UInt64 iD, string serial, string country, string model, Single maxLoad) : base(className, iD, serial, country, model)
        {
            MaxLoad = maxLoad;
        }
        public override void AddToList()
        {
            CreateFtrObject.CargoPlaneList.Add(this);
        }
        public override void AddToListFTR()
        {
            CreateFtrObject.CargoPlaneListFTR.Add(this);
        }
        public string ReportFor(IMedia media)
        {
           return media.DoForCargoPlane(this);    
        }
        public override void updateOnIDEvent(IDUpdateArgs args)
        {
            foreach (Flight flight in CreateFtrObject.IDtoFlight.Values)
            {
               if(flight.PlaneID == args.ObjectID)
               {
                    flight.PlaneID = args.NewObjectID;
               }
            }
            Program.eventManager.IDEventUnsubscribe(this);
            CargoPlane temp;
            CreateFtrObject.IDtoCargoPlane.Remove(args.ObjectID, out temp);
            temp.ID = args.NewObjectID;
            if (CreateFtrObject.IDtoCargoPlane.TryAdd(temp.ID, temp) == false)
            {
                Console.WriteLine($"failed to update {temp.ID}");
            }
            Program.eventManager.IDEventSubscribe(this);
            Tools.UpdateLog.AddUpdate(temp.ClassName, args.ObjectID, temp.ID);
        }
        public override bool EvaluateFQL(string cond)
        {
            return FilterTables.EvaluateCondition<CargoPlane>(cond, this, CreateQuery.CargoPlaneGetField);
        }
        public override string GetField(string field)
        {
            return CreateQuery.CargoPlaneGetField[field].Invoke(this);
        }
        public override IFtr Cast()
        {
            return this;
        }
        public override string[] GetAllFields()
        {
            List<string> fields = new List<string>();
            foreach (var field in CreateQuery.CargoPlaneGetField.Values)
            {
                fields.Add(field.Invoke(this));
            }
            return fields.ToArray();
        }
        public override void SetField(string value, string field)
        {
            CreateQuery.CargoPlaneSetField[field].Invoke(this, value);
        }
        public override string[] GetFieldNames()
        {
            List<string> fields = new List<string>();
            foreach (var item in CreateQuery.CargoPlaneGetField.Keys)
            {
                fields.Add(item);
            }
            return fields.ToArray();
        }
    }
    public class PassengerPlane : Plane, IReportable
    {
        public UInt16 FirstClassSize;
        public UInt16 BusinessSize;
        public UInt16 EconomyClassSize;
        public PassengerPlane() { }
        public PassengerPlane(string className, UInt64 iD, string serial, string country, string model, UInt16 firstClassSize, UInt16 businessSize, UInt16 economyClassSize) : base(className, iD, serial, country, model)
        {
            FirstClassSize = firstClassSize;
            BusinessSize = businessSize;
            EconomyClassSize = economyClassSize;
        }
        public override void AddToListFTR()
        {
            CreateFtrObject.PassengerPlanesListFTR.Add(this);
        }
        public override void AddToList()
        {
            CreateFtrObject.PassengerPlanesList.Add(this);
        }
        public string ReportFor(IMedia media)
        {
           return media.DoForPassengerPlane(this);
        }
        public override void updateOnIDEvent(IDUpdateArgs args)
        {
            foreach (Flight flight in CreateFtrObject.IDtoFlight.Values)
            {
                if(flight.PlaneID == args.ObjectID)
                {
                    flight.PlaneID = args.NewObjectID;
                }
            }
            Program.eventManager.IDEventUnsubscribe(this);
            PassengerPlane temp;
            CreateFtrObject.IDtoPassengerPlane.Remove(args.ObjectID, out temp);
            temp.ID = args.NewObjectID;
            if (CreateFtrObject.IDtoPassengerPlane.TryAdd(temp.ID, temp) == false)
            {
                Console.WriteLine($"failed to update {temp.ID}");
            }
            Program.eventManager.IDEventSubscribe(this);
            Tools.UpdateLog.AddUpdate(temp.ClassName, args.ObjectID, temp.ID);
        }
        public override bool EvaluateFQL(string cond)
        {
            return FilterTables.EvaluateCondition<PassengerPlane>(cond, this, CreateQuery.PassengerPlaneGetField);
        }
        public override string GetField(string field)
        {
            return CreateQuery.PassengerPlaneGetField[field].Invoke(this);
        }
        public override IFtr Cast()
        {
            return this;
        }
        public override string[] GetAllFields()
        {
            List<string> fields = new List<string>();
            foreach (var field in CreateQuery.PassengerPlaneGetField.Values)
            {
                fields.Add(field.Invoke(this));
            }
            return fields.ToArray();
        }
        public override void SetField(string value, string field)
        {
            CreateQuery.PassengerPlaneSetField[field].Invoke(this, value);
        }
        public override string[] GetFieldNames()
        {
            List<string> fields = new List<string>();
            foreach (var item in CreateQuery.PassengerPlaneGetField.Keys)
            {
                fields.Add(item);
            }
            return fields.ToArray();
        }
    }
    public class CargoPlaneCreator : CreatorFTR
    {
        static private readonly int FieldCount = 6;
        public override CargoPlane Create(string[] ObjectParameters)
        {
            TestIfAllArguments(FieldCount, ObjectParameters.Length);
            string ClassName = ObjectParameters[0];
            UInt64 ID = UInt64.Parse(ObjectParameters[1]);
            string Serial = ObjectParameters[2];
            string Country = ObjectParameters[3];
            string Model = ObjectParameters[4];
            Single MaxLoad = Single.Parse(ObjectParameters[5]);
            return new CargoPlane(ClassName, ID, Serial, Country, Model, MaxLoad);
        }
        public override CargoPlane Create(byte[] ObjectParameters)
        {
            char[] classID = Encoding.ASCII.GetChars(ObjectParameters[0..3]);
            string classShortName = new string(classID);
            UInt32 FollowingMessageLength = BitConverter.ToUInt32(ObjectParameters[3..7]);
            UInt64 ID = BitConverter.ToUInt64(ObjectParameters[7..15]);
            char[] Serial = Encoding.ASCII.GetChars(ObjectParameters[15..25]);
            char[] ISOC = Encoding.ASCII.GetChars(ObjectParameters[25..28]);
            UInt16 ML = BitConverter.ToUInt16(ObjectParameters[28..30]);
            char[] Model = Encoding.ASCII.GetChars(ObjectParameters[30..(30 + ML)]);
            Single MaxLoad = BitConverter.ToSingle(ObjectParameters[(30+ML)..(30 + ML + 4)]);
            return new CargoPlane(classShortName, ID, new string(Serial), new string(ISOC), new string(Model), MaxLoad);
        }

        public override CargoPlane Create()
        {
            return new CargoPlane();
        }
    }
    public class PassengerPlaneCreator : CreatorFTR
    {
        static private readonly int FieldCount = 8;
        public override PassengerPlane Create(string[] ObjectParameters)
        {
            TestIfAllArguments(FieldCount, ObjectParameters.Length);
            string ClassName = ObjectParameters[0];
            UInt64 ID = UInt64.Parse(ObjectParameters[1]);
            string Serial = ObjectParameters[2];
            string Country = ObjectParameters[3];
            string Model = ObjectParameters[4];
            UInt16 FirstClassSize = UInt16.Parse(ObjectParameters[5]);
            UInt16 BusinessClassSize = UInt16.Parse(ObjectParameters[6]);
            UInt16 EconomyClassSize = UInt16.Parse(ObjectParameters[7]);
            return new PassengerPlane(ClassName, ID, Serial, Country, Model, FirstClassSize, BusinessClassSize, EconomyClassSize);
        }
        public override PassengerPlane Create(byte[] ObjectParameters)
        {
            char[] classID = Encoding.ASCII.GetChars(ObjectParameters[0..3]);
            string classShortName = new string(classID);
            UInt32 FollowingMessageLength = BitConverter.ToUInt32(ObjectParameters[3..7]);
            UInt64 ID = BitConverter.ToUInt64(ObjectParameters[7..15]);
            char[] Serial = Encoding.ASCII.GetChars(ObjectParameters[15..25]);
            char[] ISOC = Encoding.ASCII.GetChars(ObjectParameters[25..28]);
            UInt16 ML = BitConverter.ToUInt16(ObjectParameters[28..30]);
            char[] Model = Encoding.ASCII.GetChars(ObjectParameters[30..(30 + ML)]);
            UInt16 First = BitConverter.ToUInt16(ObjectParameters[(30 + ML)..(30 + ML + 2)]);
            UInt16 Bussines = BitConverter.ToUInt16(ObjectParameters[(32 + ML)..(32 + ML + 2)]);
            UInt16 Economy = BitConverter.ToUInt16(ObjectParameters[(34 + ML)..(34 + ML + 2)]);
            return new PassengerPlane(classShortName, ID, new string(Serial), new string(ISOC),new string(Model), First,Bussines,Economy);
        }

        public override PassengerPlane Create()
        {
            return new PassengerPlane();
        }
    }
}
