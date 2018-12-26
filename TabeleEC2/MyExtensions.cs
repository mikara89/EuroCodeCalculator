using System;
namespace Extensions
{
    public static class MyExtensions
    {
        public static double Round(this Double d, int i = 2)
        {
            return Math.Round(d, i);
        }

        public static double Radians(this double d)
        {
            return d * Math.PI / 180; ;
        }
        public static double Angle(this double d)
        {
            return d * 180 / Math.PI;
        }
    }
}