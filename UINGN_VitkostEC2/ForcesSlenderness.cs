using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculatorEC2Logic
{
    public class ForcesSlenderness : IForcesSlenderness
    {
        private readonly double li;
        private readonly double h;

        public ForcesSlenderness(double li, double h)
        {
            this.li = li;
            this.h = h;
        }
        public double M_top { get; set; }
        public double M_bottom { get; set; }

        public double NEd { get; set; }

        public double M02
        {
            get
            {
                var m = Math.Abs(M_top) >= Math.Abs(M_bottom) ? M_top  : M_bottom;
                var sign =m!=0? m / Math.Abs(m):0;
                return sign*( Math.Max(Math.Abs(M_top), Math.Abs(M_bottom)) + NEd * li / 100 / 400);
            }
        }
        public double M01
        {
            get
            {
                var m = Math.Abs(M_top) >= Math.Abs(M_bottom) ? M_bottom : M_top;
                var sign = m != 0 ? m / Math.Abs(m) : 0;
                return sign*(Math.Min(Math.Abs(M_top), Math.Abs(M_bottom)) - NEd * li / 100 / 400);
            }
        }

        public double M0Ed
        {
            get=>  0.6 * M02 + 0.4 * M01 >= 0.4 * M02 ?  0.6 * M02 + 0.4 * M01 : 0.4 * M02;
        }
        public double MEd(double e2=0)
        {
            var list = new List<double>() { M02, Math.Max(h / 30, 2) / 100 * NEd + e2 / 100 * NEd, M01 + 0.5 * M02, Math.Max(h / 30, 2) / 100 * NEd };
            return list.Max();
        }
    }
}
