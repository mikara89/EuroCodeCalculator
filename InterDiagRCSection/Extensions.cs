using CalcModels;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace InterDiagRCSection
{
    public static class MyExtensions
    {
        ///// <summary>
        ///// Determines if the given point is inside the polygon
        ///// </summary>
        ///// <param name="polygon">the vertices of polygon</param>
        ///// <param name="testPoint">the given point</param>
        ///// <returns>true if the point is inside the polygon; otherwise, false</returns>
        private static bool IsPointInPolygon4(PointF[] polygon, PointF testPoint)
        {
            bool result = false;
            int j = polygon.Count() - 1;
            for (int i = 0; i < polygon.Count(); i++)
            {
                if (polygon[i].Y < testPoint.Y && polygon[j].Y >= testPoint.Y || polygon[j].Y < testPoint.Y && polygon[i].Y >= testPoint.Y)
                {
                    if (polygon[i].X + (testPoint.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) * (polygon[j].X - polygon[i].X) < testPoint.X)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }
    
        public static bool IsMNValid<T>(this List<T> list, double M, double N) 
            where T : IMNInteraction
        {
            bool result = false;
            List<PointF> poly= new List<PointF>();
            PointF point;

            point.X = (float)M;
            point.Y = (float)N;

            list.ForEach(x =>
            {
                PointF p;
                p.X = (float)x.M_Rd;
                p.Y = (float)x.N_Rd;
                poly.Add(p);
            });
            result = IsPointInPolygon4(poly.ToArray(), point);
            return result;
        }
    }
}
