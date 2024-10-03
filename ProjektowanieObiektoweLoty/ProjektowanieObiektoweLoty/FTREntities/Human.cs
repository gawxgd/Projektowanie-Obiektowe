
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
    public abstract class Human : FtrObject
    {
        public string Name;
        public UInt64 Age;
        public string Phone;
        public string Email;
        public Human() { }
        public Human(string className, UInt64 iD, string name, UInt64 age, string phone, string email) : base(className, iD)
        {
            Name = name;
            Age = age;
            Phone = phone;
            Email = email;
            Program.eventManager.ContactEventSubscribe(this);
        }
    }
    public class Crew : Human
    {
        public UInt16 Practice;
        public string Role;
        public Crew() { }
        public Crew(string className, UInt64 iD, string name, UInt64 age, string phone, string email, UInt16 practice, string role) : base(className, iD, name, age, phone, email)
        {
            Practice = practice;
            Role = role;

        }
        public override void AddToList()
        {
            CreateFtrObject.CrewList.Add(this);
        }
        public override void AddToListFTR()
        {
            CreateFtrObject.CrewListFTR.Add(this);
        }
        public override void updateOnIDEvent(IDUpdateArgs args)
        {
            foreach(Flight flight in CreateFtrObject.IDtoFlight.Values)
            {
                for(int i=0;i<flight.CrewAsIDs.Length;i++)
                {
                    if (flight.CrewAsIDs[i] == args.ObjectID)
                    {
                        flight.CrewAsIDs[i] = args.NewObjectID;
                    }
                }
            }
            Program.eventManager.IDEventUnsubscribe(this);
            Crew temp;
            CreateFtrObject.IDtoCrew.Remove(args.ObjectID, out temp);
            temp.ID = args.NewObjectID;
            if (CreateFtrObject.IDtoCrew.TryAdd(temp.ID, temp) == false)
            {
                Console.WriteLine($"failed to update {temp.ID}");
            }
            Program.eventManager.IDEventSubscribe(this);
            Tools.UpdateLog.AddUpdate(temp.ClassName, args.ObjectID, temp.ID);
        }
        public override void updateOnContactEvent(ContactInfoUpdateArgs args)
        {
            string oldPhone = Phone;
            string oldMail = Email;
            this.Phone = args.PhoneNumber;
            this.Email = args.EmailAddress;
            Tools.UpdateLog.AddUpdate(ID,ClassName, oldPhone, Phone, oldMail, Email);
        }
        public override bool EvaluateFQL(string cond)
        {
            return FilterTables.EvaluateCondition<Crew>(cond, this, CreateQuery.CrewGetField);
        }
        public override string GetField(string field)
        {
            return CreateQuery.CrewGetField[field].Invoke(this);
        }
        public override IFtr Cast()
        {
            return this;
        }
        public override string[] GetAllFields()
        {
            List<string> fields = new List<string>();
            foreach (var field in CreateQuery.CrewGetField.Values)
            {
                fields.Add(field.Invoke(this));
            }
            return fields.ToArray();
        }
        public override void SetField(string value, string field)
        {
            CreateQuery.CrewSetField[field].Invoke(this, value);
        }
        public override string[] GetFieldNames()
        {
            List<string> fields = new List<string>();
            foreach (var item in CreateQuery.CrewGetField.Keys)
            {
                fields.Add(item);
            }
            return fields.ToArray();
        }
    }
    public class Passenger : Human
    {
        public string Class;
        public UInt64 Miles;
        public Passenger() { }
        public Passenger(string className, UInt64 iD, string name, UInt64 age, string phone, string email, string @class, UInt64 miles) : base(className, iD, name, age, phone, email)
        {
            Class = @class;
            Miles = miles;
        }
        public override void AddToList()
        {
            CreateFtrObject.PassengersList.Add(this);
        }
        public override void AddToListFTR()
        {
            CreateFtrObject.PassengersListFTR.Add(this);
        }
        public override void updateOnIDEvent(IDUpdateArgs args)
        {
            Program.eventManager.IDEventUnsubscribe(this);
            Passenger temp;
            CreateFtrObject.IDtoPassenger.Remove(args.ObjectID, out temp);
            temp.ID = args.NewObjectID;
            if (CreateFtrObject.IDtoPassenger.TryAdd(temp.ID, temp) == false)
            {
                Console.WriteLine($"failed to update {temp.ID}");
            }
            Program.eventManager.IDEventSubscribe(this);
            Tools.UpdateLog.AddUpdate(temp.ClassName, args.ObjectID, temp.ID);
        }
        public override void updateOnContactEvent(ContactInfoUpdateArgs args)
        {
            string oldPhone = Phone;
            string oldMail = Email;
            this.Phone = args.PhoneNumber;
            this.Email = args.EmailAddress;
            Tools.UpdateLog.AddUpdate(ID,ClassName, oldPhone, Phone, oldMail, Email);
        }
        public override bool EvaluateFQL(string cond)
        {
            return FilterTables.EvaluateCondition<Passenger>(cond, this, CreateQuery.PassengerGetField);
        }
        public override string GetField(string field)
        {
            return CreateQuery.PassengerGetField[field].Invoke(this);
        }
        public override IFtr Cast()
        {
            return this;
        }
        public override string[] GetAllFields()
        {
            List<string> fields = new List<string>();
            foreach (var field in CreateQuery.PassengerGetField.Values)
            {
                fields.Add(field.Invoke(this));
            }
            return fields.ToArray();
        }
        public override void SetField(string value, string field)
        {
            CreateQuery.PassengerSetField[field].Invoke(this, value);
        }
        public override string[] GetFieldNames()
        {
            List<string> fields = new List<string>();
            foreach (var item in CreateQuery.PassengerGetField.Keys)
            {
                fields.Add(item);
            }
            return fields.ToArray();
        }
    }
    public class CrewCreator : CreatorFTR
    {
        static private readonly int FieldCount = 8;
        public override Crew Create(string[] ObjectParameters)
        {
            TestIfAllArguments(FieldCount, ObjectParameters.Length);
            string ClassName = ObjectParameters[0];
            UInt64 ID = UInt64.Parse(ObjectParameters[1]);
            string Name = ObjectParameters[2];
            UInt64 Age = UInt64.Parse(ObjectParameters[3]);
            string Phone = ObjectParameters[4];
            string Email = ObjectParameters[5];
            UInt16 Practice = UInt16.Parse(ObjectParameters[6]);
            string Role = ObjectParameters[7];
            return new Crew(ClassName, ID, Name, Age, Phone, Email, Practice, Role);
        }
        public override Crew Create(byte[] ObjectParameters)
        {
            char[] classID = Encoding.ASCII.GetChars(ObjectParameters[0..3]);
            string classShortName = new string(classID);
            UInt32 FollowingMessageLength = BitConverter.ToUInt32(ObjectParameters[3..7]);
            UInt64 ID = BitConverter.ToUInt64(ObjectParameters[7..15]);
            UInt16 NameLength = BitConverter.ToUInt16(ObjectParameters[15..17]);
            char[] Name = Encoding.ASCII.GetChars(ObjectParameters[17..(17 + NameLength)]);
            UInt16 Age = BitConverter.ToUInt16(ObjectParameters[(17 + NameLength)..(17 + NameLength + 2)]);
            char[] PhoneNumber = Encoding.ASCII.GetChars(ObjectParameters[(19 + NameLength)..(19 + NameLength + 12)]);
            UInt16 EmailLength = BitConverter.ToUInt16(ObjectParameters[(31 + NameLength)..(31 + NameLength + 2)]);
            char[] EmailAddress = Encoding.ASCII.GetChars(ObjectParameters[(33 + NameLength)..(33 + NameLength + EmailLength)]);
            UInt16 Practice = BitConverter.ToUInt16(ObjectParameters[(33 + NameLength + EmailLength)..(33 + NameLength + EmailLength + 2)]);
            char[] Role = Encoding.ASCII.GetChars(ObjectParameters[(35 + NameLength + EmailLength)..(35 + NameLength + EmailLength + 1)]);
            return new Crew(classShortName, ID, new string(Name), Age, new string(PhoneNumber), new string(EmailAddress), Practice, new string(Role));
        }

        public override Crew Create()
        {
            return new Crew();
        }
    }
    public class PassengerCreator : CreatorFTR
    {
        static private readonly int FieldCount = 8;
        public override Passenger Create(string[] ObjectParameters)
        {
            TestIfAllArguments(FieldCount, ObjectParameters.Length);
            string ClassName = ObjectParameters[0];
            UInt64 ID = UInt64.Parse(ObjectParameters[1]);
            string Name = ObjectParameters[2];
            UInt64 Age = UInt64.Parse(ObjectParameters[3]);
            string Phone = ObjectParameters[4];
            string Email = ObjectParameters[5];
            string Class = ObjectParameters[6];
            UInt64 Miles = UInt64.Parse(ObjectParameters[7]);
            return new Passenger(ClassName, ID, Name, Age, Phone, Email, Class, Miles);
        }
        public override Passenger Create(byte[] ObjectParameters)
        {
            char[] classID = Encoding.ASCII.GetChars(ObjectParameters[0..3]);
            string classShortName = new string(classID);
            UInt32 FollowingMessageLength = BitConverter.ToUInt32(ObjectParameters[3..7]);
            UInt64 ID = BitConverter.ToUInt64(ObjectParameters[7..15]);
            UInt16 NameLength = BitConverter.ToUInt16(ObjectParameters[15..17]);
            char[] Name = Encoding.ASCII.GetChars(ObjectParameters[17..(17 + NameLength)]);
            UInt16 Age = BitConverter.ToUInt16(ObjectParameters[(17 + NameLength)..(17 + NameLength + 2)]);
            char[] PhoneNumber = Encoding.ASCII.GetChars(ObjectParameters[(19 + NameLength)..(19 + NameLength + 12)]);
            UInt16 EmailLength = BitConverter.ToUInt16(ObjectParameters[(31 + NameLength)..(31 + NameLength + 2)]);
            char[] EmailAddress = Encoding.ASCII.GetChars(ObjectParameters[(33 + NameLength)..(33 + NameLength + EmailLength)]);
            char[] Class = Encoding.ASCII.GetChars(ObjectParameters[(33 + NameLength + EmailLength)..(33 + NameLength + EmailLength + 1)]);
            UInt64 Miles = BitConverter.ToUInt64(ObjectParameters[(34 + NameLength + EmailLength)..(34 + NameLength + EmailLength + 8)]);
            return new Passenger(classShortName, ID, new string(Name), Age, new string(PhoneNumber), new string(EmailAddress), new string(Class), Miles);
        }

        public override Passenger Create()
        {
            return new Passenger();
        }
    }
}
