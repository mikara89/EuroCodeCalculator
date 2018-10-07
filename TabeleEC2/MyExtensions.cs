using System;
namespace Extensions
{
    public static class MyExtensions
    {
        public static double Round(this Double d, int i = 2)
        {
            return Math.Round(d, i);
        }
    }
}