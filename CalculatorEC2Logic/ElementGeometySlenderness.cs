using System;

namespace CalculatorEC2Logic
{
    public class ElementGeometySlenderness : IElementGeometrySlenderness
    {
        public double b { get; set; }
        public double h { get; set; }
        public double d1 { get; set; }
        public double L { get; set; }
        public double Ix { get { return b * Math.Pow(h, 3) / 12; } }
        public double Ac { get { return b * h; } }
        public double ic { get { return Math.Sqrt(Ix / Ac); } }
        public double k { get; set; } 
        public double li => k*L;
        public double λ { get { return li / ic; } }
        public UnitDimesionType unit { get; set; } = UnitDimesionType.cm;
        public double d2 { get; set; }
        public double d =>h-d1;
    }
}
