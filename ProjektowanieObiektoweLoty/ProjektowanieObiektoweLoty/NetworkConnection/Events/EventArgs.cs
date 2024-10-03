using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektowanieObiektoweLoty.NetworkConnection.Events
{
    public class EventArgs
    {
        public UInt64 ObjectID { get; init; }
    }
    public class IDUpdateArgs : EventArgs
    {
        public ulong NewObjectID { get; init; }
    }
    public class PositionUpdateArgs : EventArgs
    {
        public float Longitude { get; init; }
        public float Latitude { get; init; }
        public float AMSL { get; init; }
    }
    public class ContactInfoUpdateArgs : EventArgs
    {
        public string PhoneNumber { get; init; }
        public string EmailAddress { get; init; }
    }
}
