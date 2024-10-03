using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektowanieObiektoweLoty.Media
{
    public class Radio : IMedia
    {
        public string MediaName;
        public Radio(string mediaName) { MediaName = mediaName; }
        public string DoForAirport(Airport a)
        {
            return $"Reporting for {MediaName}, Ladies and gentelmen, we are at the {a.Name} airport";
        }

        public string DoForCargoPlane(CargoPlane cp)
        {
            return $"Reporting for {MediaName}, Ladies and gentelmen, we are seeing the {cp.Serial} aircraft fly above us";
        }

        public string DoForPassengerPlane(PassengerPlane pp)
        {
            return $"Reporting for {MediaName}, Ladies and gentelmen, we've just witnessed {pp.Serial} take off";
        }
    }
}
