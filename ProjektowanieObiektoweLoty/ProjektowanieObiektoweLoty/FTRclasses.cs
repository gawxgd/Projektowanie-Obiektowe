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
    public string Practice;
    public string Role;
    public Crew(string className, UInt64 iD, string name, UInt64 age, string phone, string email,string practice, string role) : base(className,iD,name,age,phone,email)
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
}
public class CrewCreator : CreatorFTR
{   
    public override Crew Create(string[] ObjectParameters)
    {
        Type type = typeof(Crew);
        int NumberOfRecords = type.GetFields().Length;
        if (NumberOfRecords != ObjectParameters.Length)
        {
            Console.WriteLine(ObjectParameters[2]);
            Console.WriteLine(NumberOfRecords.ToString());
            Console.WriteLine(ObjectParameters.Length.ToString());
            throw new ArgumentException("invalid number of arguments");
        }
        
        string ClassName = ObjectParameters[0];
        UInt64 ID = UInt64.Parse(ObjectParameters[1]);
        string Name = ObjectParameters[2];
        UInt64 Age = UInt64.Parse(ObjectParameters[3]);
        string Phone = ObjectParameters[4];
        string Email = ObjectParameters[5];
        string Practice = ObjectParameters[6];
        string Role = ObjectParameters[7];
        
        return new Crew(ClassName,ID,Name,Age,Phone,Email,Practice,Role);
    }
}
/*
public class PassengerCreator : CreatorFTR
{
    public override Passenger Create(string[] ObjectParameters)
    {
        return new Passenger();
    }
}
public class CargoCreator : CreatorFTR
{
    public override Cargo Create(string[] ObjectParameters)
    {
        
        return new Cargo();
    }
}
public class CargoPlaneCreator : CreatorFTR
{
    public override CargoPlane Create(string[] ObjectParameters)
    {
        return new CargoPlane();
    }
}
public class PassengerPlaneCreator : CreatorFTR
{
    public override PassengerPlane Create(string[] ObjectParameters)
    {
        return new PassengerPlane();
    }
}
public class AirportCreator : CreatorFTR
{
    public override Airport Create(string[] ObjectParameters)
    {
        return new Airport();
    }
}
public class FlightCreator : CreatorFTR
{
    public override Flight Create(string[] ObjectParameters)
    {
        return new Flight();
    }
}
*/
