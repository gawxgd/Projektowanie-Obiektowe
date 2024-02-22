using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektowanieObiektoweLoty
{
    public class Program
    {
        private static string FilePath = "C:\\Users\\marci\\Downloads\\example_data.ftr";
        static CrewCreator CrewFactory;
        static PassengerCreator PassengerFactory;
        static CargoCreator CargoFactory;
        static CargoPlaneCreator CarogPlaneFactory;
        static PassengerPlaneCreator PassengerPlaneFactory;
        static AirportCreator AirportFactory;
        static FlightCreator FlightFactory;
        private static void ReadFromFtrFile(string FilePathArg)
        {
            String line;
            try
            {
                StreamReader sr = new StreamReader(FilePath);

                line = sr.ReadLine();

                while (line != null)
                {

                   
                    string[] ObjectParameters = line.Split(',');
                    string ClassShortName = ObjectParameters[0];
                    switch (ClassShortName)
                    {
                        case "C":
                            Crew CrewObject = CrewFactory.Create(ObjectParameters);
                            Console.WriteLine(CrewObject.Name);
                            break;
                        case "P":
                            Passenger PassengerObject = PassengerFactory.Create(ObjectParameters);
                            Console.WriteLine(PassengerObject.Name);
                            break;
                        case "CA":
                            Cargo CargoObject = CargoFactory.Create(ObjectParameters);
                            Console.WriteLine(CargoObject.Description);
                            break;
                        case "CP":
                            CargoPlane CargoPlaneObject = CarogPlaneFactory.Create(ObjectParameters);
                            Console.WriteLine(CargoPlaneObject.model);
                            break;
                        case "PP":
                            PassengerPlane PassengerPlaneObject = PassengerPlaneFactory.Create(ObjectParameters);
                            Console.WriteLine(PassengerPlaneObject.model);
                            break;
                        case "AI":
                            Airport AirportObject = AirportFactory.Create(ObjectParameters);
                            Console.WriteLine(AirportObject.Name);
                            break;
                        case "FL":
                            Flight FlightObject = FlightFactory.Create(ObjectParameters);
                            Console.WriteLine(FlightObject.ID);
                            break;    

                    }

                    line = sr.ReadLine();
                }
                

                sr.Close();
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
            
        }
        private static void CreateFactoryClasses()
        {
            CrewFactory = new CrewCreator();
            PassengerFactory = new PassengerCreator();
            CargoFactory = new CargoCreator();
            CarogPlaneFactory = new CargoPlaneCreator();
            PassengerPlaneFactory = new PassengerPlaneCreator();
            AirportFactory = new AirportCreator();
            FlightFactory = new FlightCreator();
            
        }
        static void Main()
        {
            CultureInfo newCulture = new CultureInfo("en-US"); 
            CultureInfo.DefaultThreadCurrentCulture = newCulture;
            CultureInfo.DefaultThreadCurrentUICulture = newCulture;
            CreateFactoryClasses();
            ReadFromFtrFile(FilePath);
        }
    }
}
