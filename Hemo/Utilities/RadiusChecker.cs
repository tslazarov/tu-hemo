using System;

namespace Hemo.Utilities
{
    internal class RadiusChecker
    {
        internal static double GetDistance(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            double R = 6371; // Radius of the earth in km
            double dLat = ToRadians(latitude2 - latitude1);  // deg2rad below
            double dLon = ToRadians(longitude2 - longitude1);
            double a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(latitude1)) * Math.Cos(ToRadians(latitude2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double d = R * c; // Distance in km
            return d;
        }

        private static double ToRadians(double deg)
        {
            return deg * (Math.PI / 180);
        }
    }
}