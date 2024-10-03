using Avalonia.Markup.Xaml.Templates;
using Avalonia.Rendering;
using Mapsui.Projections;
using Splat;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace ProjektowanieObiektoweLoty
{
    public class FlightToGuiAdapter
    {
        public List<FlightGUI> ConvertedFlightsList = new List<FlightGUI>();
        public int FlightCount = 0;
        private DateTime actDate;
        
        public void Conversion()
        {
            if (CreateFtrObject.IDtoFlight.Count == 0)
            {
                throw new Exception("FligthsList empty");
            }
            ConvertedFlightsList.Clear();
            foreach (var flight in CreateFtrObject.IDtoFlight.Values)
            {
                actDate = DateTime.UtcNow;
                if (DateTime.Compare(flight.UpdatedTakeoff,actDate) > 0 || DateTime.Compare(flight.LandingTime,actDate) < 0)
                {
                    continue;
                }
                ulong flightTargetAirport = flight.TargetAsID;

                double progress = FlightCalculations.CalculateProgress(actDate,flight.UpdatedTakeoff,flight.FlightDuration);
                if (progress < 0 || progress > 1)
                    continue;

                WorldPosition FlightPosition = FlightCalculations.CalculatePosition(flight.Longtitude,flight.Latitude,flight.TargetAsID,progress);
                double Rotation = FlightCalculations.CalculateRotation(flight.Longtitude,flight.Latitude,flightTargetAirport);
                ConvertedFlightsList.Add(CreateFlightGUIObject(flight.ID,FlightPosition,Rotation));
            }
            UpdateGUI();
           
        }
        private void UpdateGUI()
        {
            FlightsGUIData GUIdata = new FlightsGUIData(ConvertedFlightsList);
            var GUI = GUIcontrol.GetInstance();
            GUI.UpdateFlightsGUI(GUIdata);
        }
        private FlightGUI CreateFlightGUIObject(ulong ID, WorldPosition position,double Rotation)
        {
            FlightGUI flightGUI = new FlightGUI
            {
                ID = ID,
                WorldPosition = position,
                MapCoordRotation = Rotation,
            };
            return flightGUI;
        }
        public void FlightMovment()
        {
            bool cond = true;
            while (cond)
            {
                Conversion();
                Thread.Sleep(1000);
            }
        }
}



}
