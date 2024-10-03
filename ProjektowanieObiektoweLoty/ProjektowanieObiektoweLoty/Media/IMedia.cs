using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektowanieObiektoweLoty.Media
{
    public interface IMedia
    {
        public string DoForAirport(Airport a);
        public string DoForCargoPlane(CargoPlane cp);
        public string DoForPassengerPlane(PassengerPlane pp);
    }
}
