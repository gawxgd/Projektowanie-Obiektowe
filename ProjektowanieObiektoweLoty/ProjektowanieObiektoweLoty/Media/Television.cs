using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektowanieObiektoweLoty.Media
{
    public class Television : IMedia
    {
        public string MediaName;
        public Television(string mediaName) { MediaName = mediaName; }
        public string DoForAirport(Airport a)
        {
            return $"An image of {a.Name} airport";
        }

        public string DoForCargoPlane(CargoPlane cp)
        {
            return $"An image of {cp.model} cargo plane";
        }

        public string DoForPassengerPlane(PassengerPlane pp)
        {
            return $"An image of {pp.model} passenger plane";
        }
    }
}
