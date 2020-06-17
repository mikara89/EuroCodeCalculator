using System;
using System.Collections.Generic;
using ClipperLib;
//using ClipperLib;

namespace SectionDesign
{
    public class LinkBar
    {
        /// <summary>
        /// In [mm]
        /// </summary>
        public int diametar { get; set; }
        public List<LongBar> linkedBars { get; set; }
        public List<Point> LinkPoints { get; internal set; } 
        public List<Point> GetPath() 
        {
            List<Point> result = new List<Point>();
            List<IntPoint> intPoints = new List<IntPoint>();

            List < List <IntPoint>> solution = new List<List<IntPoint>>();
            for (int i = 0; i < linkedBars.Count; i++)
            {
                intPoints.Add(new IntPoint 
                { 
                    X = Convert.ToInt64(linkedBars[i].X * 100 ),
                    Y = Convert.ToInt64(linkedBars[i].Y * 100)
                });
            }
            ClipperOffset c = new ClipperOffset();
            c.AddPath(intPoints,  JoinType.jtRound, EndType.etClosedPolygon);
            c.Execute(ref solution, (linkedBars[0].diametar + diametar) * 10.0);

            for (int i = 0; i < solution[0].Count; i++)
            {
                result.Add(new Point
                {
                    X = solution[0][i].X / 100.0,
                    Y = solution[0][i].Y / 100.0,
                });
            }

            return result;
        }
        /// <summary>
        /// in [cm]
        /// </summary>
        public double Vh
        {
            get
            {
                double d = 0;
                for (int i = 0; i < linkedBars.Count - 1; i++)
                {
                    var nextP = linkedBars[i + 1];
                    var thisP = linkedBars[i];
                    d += GetDistance(thisP.X, thisP.Y, nextP.X, nextP.Y) + linkedBars[i + 1].diametar/10.0 + diametar/10.0;
                }
                return d * Math.Pow((double)diametar / 10.0, 2) * Math.PI / 4.0;
            }
        }

        private static double GetDistance(double x1, double y1, double x2, double y2) 
        {
            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }    
    }
}
