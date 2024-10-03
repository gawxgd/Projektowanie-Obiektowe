using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektowanieObiektoweLoty
{
    public interface IEvent
    {
        public NetworkSourceSimulator.ContactInfoUpdate ContactEvent { get; set; }
        public NetworkSourceSimulator.IDUpdate IDEvent { get; set; }
        public NetworkSourceSimulator.PositionUpdate PositionEvent { get; set; }
    }
}
