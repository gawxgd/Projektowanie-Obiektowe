using Avalonia.Controls;
using RTools_NTS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace ProjektowanieObiektoweLoty
{
    public class GUIcontrol
    {
        private GUIcontrol() { }
        private static GUIcontrol _instance;
        private static bool running = false;
        public static GUIcontrol GetInstance()
        {
            if (_instance == null)
            {
                _instance = new GUIcontrol();
                running = true;
            }
            return _instance;
        }
        public void StartGUI()
        {
            FlightTrackerGUI.Runner.Run();
        }
        public void UpdateFlightsGUI(FlightsGUIData flightsGUIData)
        {
            if(running == false) 
            {
                throw new Exception("GUI not running");
            }
            FlightTrackerGUI.Runner.UpdateGUI(flightsGUIData);
        }
        public void InitalizeGUIflights()
        {
            FlightToGuiAdapter adapter = new FlightToGuiAdapter();
            adapter.Conversion();
            adapter.FlightMovment();
        }
        public void RunGUI()
        {
            try
            {
                //ConnectWithNetwork.GetInstance().JoinConnection(); only when needed to wait for objects to be sent
                Task.Run(()=>_instance.StartGUI());
                _instance.InitalizeGUIflights();
            }
            catch (Exception e) 
            {
                Console.WriteLine(e);
            }
        }
    }
}
