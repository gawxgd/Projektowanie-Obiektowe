using System.Globalization;

public interface IFtr
{ }
public abstract class FtrObject : IFtr
{
    public string ClassName;
    public UInt64 ID;
    public FtrObject(string className, UInt64 iD)
    {
        ClassName = className;
        ID = iD;
    }
}
public abstract class Human : FtrObject
{
    public string Name;
    public UInt64 Age;
    public string Phone;
    public string Email;
    public Human(string className,UInt64 iD,string name, UInt64 age, string phone, string email) : base(className,iD)
    {
        Name = name;
        Age = age;
        Phone = phone;
        Email = email;
    }
}
public abstract class Plane : FtrObject
{
    public string Serial;
    public string Country;
    public string model;
    public Plane(string className, UInt64 iD,string serial, string country, string model) : base(className,iD)
    {
        Serial = serial;
        Country = country;
        this.model = model;
    }
}
public class Crew : Human
{
    public UInt16 Practice;
    public string Role;
    public Crew(string className, UInt64 iD, string name, UInt64 age, string phone, string email,UInt16 practice, string role) : base(className,iD,name,age,phone,email)
    {
        Practice = practice;
        Role = role;
    }
}
public class Passenger : Human
{
    public string Class;
    public UInt64 Miles;
    public Passenger(string className, UInt64 iD, string name, UInt64 age, string phone, string email,string @class, UInt64 miles) : base(className, iD, name, age, phone, email)
    {
        Class = @class;
        Miles = miles;
    }
}
public class Cargo : FtrObject
{
    public Single Weight;
    public string Code;
    public string Description;
    public Cargo(string className, UInt64 iD,Single weight, string code, string description) : base(className,iD)
    {
        Weight = weight;
        Code = code;
        Description = description;
    }
}
public class CargoPlane : Plane
{
    public Single MaxLoad;
    public CargoPlane(string className, UInt64 iD, string serial, string country, string model,Single maxLoad) : base(className,iD,serial,country,model)
    {
        MaxLoad = maxLoad;
    }
}
public class PassengerPlane : Plane
{
    public UInt16 FirstClassSize;
    public UInt16 BusinessSize;
    public UInt16 EconomyClassSize;
    public PassengerPlane(string className, UInt64 iD, string serial, string country, string model,UInt16 firstClassSize, UInt16 businessSize, UInt16 economyClassSize) : base(className, iD, serial, country, model)
    {
        FirstClassSize = firstClassSize;
        BusinessSize = businessSize;
        EconomyClassSize = economyClassSize;
    }
}
public class Airport : FtrObject
{
    public string Name;
    public string Code;
    public Single Longtitude;
    public Single Latitude;
    public Single AMSL;
    public string Country;
    public Airport(string className, UInt64 iD, string name, string code, Single longtitude, Single latitude, Single aMSL, string country) : base(className,iD)
    {
        Name = name;
        Code = code;
        Longtitude = longtitude;
        Latitude = latitude;
        AMSL = aMSL;
        Country = country;
    }
}
public class Flight : FtrObject
{
    public UInt64 OriginAsID;
    public UInt64 TargetAsID;
    public string TakeOffTime;
    public string LandingTime;
    public Single Longtitude;
    public Single Latitude;
    public Single AMSL;
    public UInt64 PlaneID;
    public UInt64[] CrewAsIDs;
    public UInt64[] LoadAsIDs; //Cargo or Passengers IDs
    public Flight(string className, UInt64 iD,UInt64 originAsID, UInt64 targetAsID, string takeOffTime, string landingTime, Single longtitude, Single latitude, Single aMSL, UInt64 planeID, UInt64[] crewAsIDs, UInt64[] loadAsIDs) : base(className,iD)
    {
        OriginAsID = originAsID;
        TargetAsID = targetAsID;
        TakeOffTime = takeOffTime;
        LandingTime = landingTime;
        Longtitude = longtitude;
        Latitude = latitude;
        AMSL = aMSL;
        PlaneID = planeID;
        CrewAsIDs = crewAsIDs;
        LoadAsIDs = loadAsIDs;
    }
}
public abstract class CreatorFTR
{
    public abstract IFtr Create(string[] ObjectParameters);
    protected void TestIfAllArguments(Type type,int ArgumentsFromFileCount)
    {
        int NumberOfRecords = type.GetFields().Length;
        if (NumberOfRecords != ArgumentsFromFileCount)
        {
            throw new ArgumentException("invalid number of arguments");
        }
    }
}
public class CrewCreator : CreatorFTR
{   
    public override Crew Create(string[] ObjectParameters)
    {
        TestIfAllArguments(typeof(Crew),ObjectParameters.Length);
        
        string ClassName = ObjectParameters[0];
        UInt64 ID = UInt64.Parse(ObjectParameters[1]);
        string Name = ObjectParameters[2];
        UInt64 Age = UInt64.Parse(ObjectParameters[3]);
        string Phone = ObjectParameters[4];
        string Email = ObjectParameters[5];
        UInt16 Practice = UInt16.Parse(ObjectParameters[6]);
        string Role = ObjectParameters[7];
        
        return new Crew(ClassName,ID,Name,Age,Phone,Email,Practice,Role);
    }
}
public class PassengerCreator : CreatorFTR
{
    public override Passenger Create(string[] ObjectParameters)
    {
        TestIfAllArguments(typeof(Passenger), ObjectParameters.Length);

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
}
public class CargoCreator : CreatorFTR
{
    public override Cargo Create(string[] ObjectParameters)
    {
        TestIfAllArguments(typeof(Cargo), ObjectParameters.Length);

        string ClassName = ObjectParameters[0];
        UInt64 ID = UInt64.Parse(ObjectParameters[1]);
        Single Weight = Single.Parse(ObjectParameters[2]);
        string Code = ObjectParameters[3];
        string Description = ObjectParameters[4];
        return new Cargo(ClassName, ID,Weight,Code,Description);
    }
}
public class CargoPlaneCreator : CreatorFTR
{
    public override CargoPlane Create(string[] ObjectParameters)
    {
        TestIfAllArguments(typeof(CargoPlane), ObjectParameters.Length);

        string ClassName = ObjectParameters[0];
        UInt64 ID = UInt64.Parse(ObjectParameters[1]);
        string Serial = ObjectParameters[2];
        string Country = ObjectParameters[3];
        string Model = ObjectParameters[4];
        Single MaxLoad = Single.Parse(ObjectParameters[5]);

        return new CargoPlane(ClassName, ID, Serial, Country, Model, MaxLoad);
    }
}
public class PassengerPlaneCreator : CreatorFTR
{
    public override PassengerPlane Create(string[] ObjectParameters)
    {
        TestIfAllArguments(typeof(PassengerPlane), ObjectParameters.Length);

        string ClassName = ObjectParameters[0];
        UInt64 ID = UInt64.Parse(ObjectParameters[1]);
        string Serial = ObjectParameters[2];
        string Country = ObjectParameters[3];
        string Model = ObjectParameters[4];
        UInt16 FirstClassSize = UInt16.Parse(ObjectParameters[5]);
        UInt16 BusinessClassSize = UInt16.Parse(ObjectParameters[6]);
        UInt16 EconomyClassSize = UInt16.Parse(ObjectParameters[7]);
        
        return new PassengerPlane(ClassName, ID, Serial, Country, Model, FirstClassSize,BusinessClassSize,EconomyClassSize);
    }
}
public class AirportCreator : CreatorFTR
{
    public override Airport Create(string[] ObjectParameters)
    {
        TestIfAllArguments(typeof(Airport), ObjectParameters.Length);

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
}
    public class FlightCreator : CreatorFTR
    {
        public override Flight Create(string[] ObjectParameters)
        {
            TestIfAllArguments(typeof(Flight), ObjectParameters.Length);

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
    }


