
using DynamicData;
using NetworkSourceSimulator;
using ProjektowanieObiektoweLoty.FQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektowanieObiektoweLoty
{
    public class Cargo : FtrObject
    {
        public Single Weight;
        public string Code;
        public string Description;
        public Cargo() { }
        public Cargo(string className, UInt64 iD, Single weight, string code, string description) : base(className, iD)
        {
            Weight = weight;
            Code = code;
            Description = description;
        }
        public override void AddToList()
        {
            CreateFtrObject.CargoList.Add(this);
        }
        public override void AddToListFTR()
        {
            CreateFtrObject.CargoListFTR.Add(this);
        }
        public override void updateOnIDEvent(IDUpdateArgs args)
        {
            foreach (Flight flight in CreateFtrObject.IDtoFlight.Values)
            {
                for(int i = 0; i<flight.LoadAsIDs.Length;i++)
                {
                    if (flight.LoadAsIDs[i] == args.ObjectID)
                    {
                        flight.LoadAsIDs[i] = args.NewObjectID;
                    }
                }
            }
            Program.eventManager.IDEventUnsubscribe(this);
            Cargo tempCargo;
            CreateFtrObject.IDtoCargo.Remove(args.ObjectID, out tempCargo);
            tempCargo.ID = args.NewObjectID;
            if (CreateFtrObject.IDtoCargo.TryAdd(tempCargo.ID,tempCargo) == false)
            {
                Console.WriteLine($"failed to update {tempCargo.ID}");
            }
            Program.eventManager.IDEventSubscribe(this);
            Tools.UpdateLog.AddUpdate(tempCargo.ClassName, args.ObjectID, tempCargo.ID);

        }
        public override bool EvaluateFQL(string cond)
        {
            return FilterTables.EvaluateCondition<Cargo>(cond, this, CreateQuery.CargoGetField);
        }
        public override string GetField(string field)
        {
            return CreateQuery.CargoGetField[field].Invoke(this);
        }
        public override IFtr Cast()
        {
            return this;
        }
        public override string[] GetAllFields()
        {
            List<string> fields = new List<string>();
            foreach (var field in CreateQuery.CargoGetField.Values)
            {
                fields.Add(field.Invoke(this));
            }
            return fields.ToArray();
        }
        public override void SetField(string value, string field)
        {
            CreateQuery.CargoSetField[field].Invoke(this, value);
        }
        public override string[] GetFieldNames()
        {
            List<string> fields = new List<string>();
            foreach (var item in CreateQuery.CargoGetField.Keys)
            {
                fields.Add(item);
            }
            return fields.ToArray();
        }
    }
    public class CargoCreator : CreatorFTR
    {
        static private readonly int FieldCount = 5;
        public override Cargo Create(string[] ObjectParameters)
        {
            TestIfAllArguments(FieldCount, ObjectParameters.Length);
            string ClassName = ObjectParameters[0];
            UInt64 ID = UInt64.Parse(ObjectParameters[1]);
            Single Weight = Single.Parse(ObjectParameters[2]);
            string Code = ObjectParameters[3];
            string Description = ObjectParameters[4];
            return new Cargo(ClassName, ID, Weight, Code, Description);
        }
        public override Cargo Create(byte[] ObjectParameters)
        {
            char[] classID = Encoding.ASCII.GetChars(ObjectParameters[0..3]);
            string classShorName = new string(classID);
            UInt32 FollowingMessageLength = BitConverter.ToUInt32(ObjectParameters[3..7]);
            UInt64 ID = BitConverter.ToUInt64(ObjectParameters[7..15]);
            Single Weight = BitConverter.ToSingle(ObjectParameters[15..19]);
            char[] Code = Encoding.ASCII.GetChars(ObjectParameters[19..25]);
            UInt16 DL = BitConverter.ToUInt16(ObjectParameters[25..27]);
            char[] Description = Encoding.ASCII.GetChars(ObjectParameters[27..(27 + DL)]);
            return new Cargo(classShorName, ID,Weight, new string(Code),new string(Description));
        }

        public override Cargo Create()
        {
            return new Cargo();
        }
    }
}
