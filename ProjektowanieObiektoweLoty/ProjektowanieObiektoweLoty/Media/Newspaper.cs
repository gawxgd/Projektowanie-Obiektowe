using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektowanieObiektoweLoty.Media
{
    public class Newspaper : IMedia
    {
        public string MediaName;
        public Newspaper(string  mediaName) {  MediaName = mediaName; } 
        public string DoForAirport(Airport a)
        {
            return $"{MediaName} - A report from the {a.Name} airport, {a.Country}";
        }
        public string DoForCargoPlane(CargoPlane cp)
        {
            return $"{MediaName} - An interview with the crew of {cp.Serial}";
        }

        public string DoForPassengerPlane(PassengerPlane pp)
        {
            return $"{MediaName} - Breaking news! {pp.model} aircraft loses EASA fails certification after inspection of {pp.Serial}";
        }
    }
}
