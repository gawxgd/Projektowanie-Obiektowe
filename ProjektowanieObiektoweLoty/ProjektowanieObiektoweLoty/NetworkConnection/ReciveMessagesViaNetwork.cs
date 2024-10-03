using NetworkSourceSimulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ProjektowanieObiektoweLoty
{
    public class ReciveMessagesViaNetwork
    {
        ConnectWithNetwork NetworkConnection;
        NetworkSourceSimulator.NetworkSourceSimulator connectionObject;
        EventManger eventManger;
        public ReciveMessagesViaNetwork(ConnectWithNetwork networkConnection,EventManger eventManger)
        {
            NetworkConnection = networkConnection;
            this.eventManger = eventManger;
            SubscribeContactUpdate();
            SubscribeIDUpdate();
            SubscribePositionUpdate();
        }
        public void SubscribeNewDataReadyEvent()
        {
            try
            {
                connectionObject = NetworkConnection.GetConnectionObject();
                connectionObject.OnNewDataReady += OnNewDataReadyEventHandler;
            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.ToString());
            }
        }
        private void SubscribeIDUpdate()
        {
            try
            {
                connectionObject = NetworkConnection.GetConnectionObject();
                connectionObject.OnIDUpdate += OnIDUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        private void SubscribePositionUpdate()
        {
            try
            {
                connectionObject = NetworkConnection.GetConnectionObject();
                connectionObject.OnPositionUpdate += OnPositionUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        private void SubscribeContactUpdate()
        {
            try
            {
                connectionObject = NetworkConnection.GetConnectionObject();
                connectionObject.OnContactInfoUpdate += OnContactUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public void OnNewDataReadyEventHandler(object sender,NewDataReadyArgs args)
        {
            int MessageIndex = args.MessageIndex;
            Message NewMessage = connectionObject.GetMessageAt(MessageIndex);
            ReadMessage.ReadFromMessage(NewMessage);
        }
        private void OnIDUpdate(object sender,IDUpdateArgs args)
        {
            eventManger.NotifyOnIdUpdate(args);
        }
        private void OnPositionUpdate(object sender, PositionUpdateArgs args)
        {
            //Console.WriteLine($"{args.ObjectID} should be updated to {args.AMSL}");
            eventManger.NotifyOnPositionUpdate(args);
        }
        private void OnContactUpdate(object sender, ContactInfoUpdateArgs args)
        {
            //Console.WriteLine($"{args.ObjectID} should be updated to {args.PhoneNumber}");
            eventManger.NotifyOnContactUpdate(args);
        }
        
    }
}
