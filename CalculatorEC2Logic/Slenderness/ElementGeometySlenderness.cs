using CalcModels;
using System;

namespace CalculatorEC2Logic
{
    public class ElementGeometySlenderness : ElementGeometry, IElementGeometrySlenderness
    {
        public double L { get; set; }
        public double Ix { get { return b * Math.Pow(h, 3) / 12; } }
        public double Ac { get { return b * h; } }
        public double ic { get { return Math.Sqrt(Ix / Ac); } }
        public double k { get; set; } 
        public double li => k*L;
        public double λ { get { return li / ic; } }
    }
}
