using Mapsui.Projections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektowanieObiektoweLoty
{
    public static class FlightCalculations
    {
        public static double CalculateProgress(DateTime actDate, DateTime takeOfftime, long duration)
        {
            TimeSpan time = (actDate - takeOfftime);
            return time.TotalMilliseconds / duration;
        }
        public static WorldPosition CalculatePosition(Single longP,Single latP, ulong flightTargetAirport, double progress)
        {
            (Single AirportLongtitudeTarget, Single AirportLatitudeTarget) = GetAirportCords(flightTargetAirport);

            Single interpolatedLongitude = (float)(longP + (AirportLongtitudeTarget - longP) * progress);
            Single interpolatedLatitude = (float)(latP + (AirportLatitudeTarget - latP) * progress);

            return new WorldPosition(interpolatedLatitude, interpolatedLongitude);
        }
        public static double CalculateRotation(Single LongP,Single LatP, ulong flightTargetAirport)
        {
            (Single LongT, Single LatT) = GetAirportCords(flightTargetAirport);

            (double X, double Y) pos1 = SphericalMercator.FromLonLat(LongP, LatP);
            (double X, double Y) pos2 = SphericalMercator.FromLonLat(LongT, LatT);
            double num = Math.Atan2(pos2.Y - pos1.Y, pos1.X - pos2.X) - Math.PI / 2;

            if (!(num < 0.0))
            {
                return num;
            }

            return num;
        }
        public static (Single LongTarget, Single LatTarget) GetAirportCords(ulong flightTargetAirport)
        {
            Airport TargetAirport = CreateFtrObject.IDtoAirport[flightTargetAirport];
            Single AirportLongtitudeTarget = TargetAirport.Longtitude;
            Single AirportLatitudeTarget = TargetAirport.Latitude;
            return (AirportLongtitudeTarget, AirportLatitudeTarget);
        }
    }
}
