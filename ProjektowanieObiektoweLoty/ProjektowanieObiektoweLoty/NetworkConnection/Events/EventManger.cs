using NetworkSourceSimulator;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ProjektowanieObiektoweLoty
{
    public class EventManger
    {
        private Dictionary<ulong,FtrObject> IDEventListeners;
        private Dictionary<ulong,FtrObject> PositionEventListeners;
        private Dictionary<ulong,FtrObject> ContactEventListeners;
        public EventManger()
        {
            IDEventListeners = new Dictionary<ulong, FtrObject>();
            PositionEventListeners = new Dictionary<ulong, FtrObject>();
            ContactEventListeners = new Dictionary<ulong, FtrObject>();
        }
        public void IDEventSubscribe(FtrObject listenerObject)
        {
            try
            {
                IDEventListeners.Add(listenerObject.ID, listenerObject);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(IDEventListeners[listenerObject.ID].ClassName);
            }
        }
        public void PositionEventSubscribe(FtrObject listenerObject)
        {
            PositionEventListeners.Add(listenerObject.ID, listenerObject);
        }
        public void ContactEventSubscribe(FtrObject listenerObject)
        {
            ContactEventListeners.Add(listenerObject.ID, listenerObject);
        }
        public void IDEventUnsubscribe(FtrObject listenerObject) 
        {
            try
            {
                IDEventListeners.Remove(listenerObject.ID);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void PositionEventUnsubscribe(FtrObject listenerObject)
        {
            PositionEventListeners.Remove(listenerObject.ID);
        }
        public void ContactEventUnsubscribe(FtrObject listenerObject)
        {
            ContactEventListeners.Remove(listenerObject.ID);
        }
        public void NotifyOnIdUpdate(IDUpdateArgs args) 
        {
            if(CreateFtrObject.IsIdTaken(args.NewObjectID) == true)
            {
                Tools.UpdateLog.AddTakenIDLog(args.NewObjectID);
                return;
            }
            try
            {
                IDEventListeners[args.ObjectID].updateOnIDEvent(args);
            }
            catch (Exception ex) 
            { 
            }
        }
        public void NotifyOnPositionUpdate(PositionUpdateArgs args)
        {
            try
            {
                PositionEventListeners[args.ObjectID].updateOnPositionEvent(args);
            }
            catch (Exception ex) {}
        }
        public void NotifyOnContactUpdate(ContactInfoUpdateArgs args)
        {
            try
            {
                ContactEventListeners[args.ObjectID].updateOnContactEvent(args);
            }
            catch(Exception ex) { }
        }

    }
}
