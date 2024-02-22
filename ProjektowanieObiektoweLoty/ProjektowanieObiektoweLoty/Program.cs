using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektowanieObiektoweLoty
{
    public class Program
    {
        private static string FilePath = "C:\\Users\\marci\\Downloads\\example_data.ftr";
        static CrewCreator CrewFactory;
        /*
        static PassengerCreator PassengerFactory;
        static CargoCreator CargoFactory;
        static CargoPlaneCreator CarogPlaneFactory;
        static PassengerPlaneCreator PassengerPlaneCFactory;
        static AirportCreator AirportFactory;
        static FlightCreator FlightFactory;
        */
        private static void ReadFromFtrFile(string FilePathArg)
        {
            String line;
            try
            {
                StreamReader sr = new StreamReader(FilePath);

                line = sr.ReadLine();

                while (line != null)
                {

                   // Console.WriteLine(line);
                    string[] ObjectParameters = line.Split(',');
                    string ClassShortName = ObjectParameters[0];
                    switch (ClassShortName)
                    {
                        case "C":
                            Crew CrewObject = CrewFactory.Create(ObjectParameters);
                            Console.WriteLine(CrewObject.Name);
                            break;
                        /*
                        case "P":
                            Passenger PassengerObject = (Passenger)
                        */

                    }

                    line = sr.ReadLine();
                }

                sr.Close();
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
            
        }
        private static void CreateFactoryClasses()
        {
            CrewFactory = new CrewCreator();
            /*
            PassengerFactory = new PassengerCreator();
            CargoFactory = new CargoCreator();
            CarogPlaneFactory = new CargoPlaneCreator();
            PassengerPlaneCFactory = new PassengerPlaneCreator();
            AirportFactory = new AirportCreator();
            FlightFactory = new FlightCreator();
            */
        }
        static void Main()
        {
            CreateFactoryClasses();
            ReadFromFtrFile(FilePath);
        }
    }
}
