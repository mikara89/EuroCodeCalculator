using System;

namespace CalcModels
{
    public abstract class ElementGeometryWithReinfBase : IElementGeometryWithReinf
    {
        public string GetUnits()
        {
            switch (unit)
            {
                case UnitDimesionType.cm:
                    return nameof(UnitDimesionType.cm);
                case UnitDimesionType.m:
                    return nameof(UnitDimesionType.m);
                case UnitDimesionType.mm:
                    return nameof(UnitDimesionType.mm);
                default:
                    throw new ArgumentOutOfRangeException("Error in setting units");
            }
        }

        public abstract double calc_y1();

        public abstract double calc_y2();
        public abstract double calc_I(); 

        public abstract double Get_b(double z);

        public abstract void Invert(bool isInverted=true);

        public double b { get; set; }
        public double h { get; set; }
        public double d1 { get; set; }
        public double d { get => h - d1; }
        public UnitDimesionType unit { get; set; } = UnitDimesionType.cm;
        public double d2 { get; set; }
        public double As_1 { get; set; }
        public double As_2 { get; set; }

        public double y1 => calc_y1();

        public double y2 => calc_y2();

        public abstract bool IsInverted { get;internal set; }

        public double I => calc_I();
    }
    public abstract class ElementGeometryStiffnessBase : IElementGeometryStiffness 
    {
        public string GetUnits()
        {
            switch (unit)
            {
                case UnitDimesionType.cm:
                    return nameof(UnitDimesionType.cm);
                case UnitDimesionType.m:
                    return nameof(UnitDimesionType.m);
                case UnitDimesionType.mm:
                    return nameof(UnitDimesionType.mm);
                default:
                    throw new ArgumentOutOfRangeException("Error in setting units");
            }
        }

        public abstract double calc_y1();

        public abstract double calc_y2();
        public abstract double calc_I();

        public double b { get; set; }
        public double h { get; set; }
        public double d1 { get; set; }
        public double d { get => h - d1; }
        public UnitDimesionType unit { get; set; } = UnitDimesionType.cm;
        public double d2 { get; set; }

        public double y1 => calc_y1();

        public double y2 => calc_y2();

        public double I => calc_I();

        public double L { get; set; }

        public NodeStiffness Lvl { get; set; }
    }
    public class ElementGeometryWithReinfold : IElementGeometryWithReinfold
    {
        public double b_eff { get; set; }
        public double h_f { get; set; }

        public double y1 { get {
                var Ac1 = (h - h_f) * b;
                var Ac2 = h_f * b_eff;
                var yc1 = (h - h_f) / 2;
                var yc2 = h - (h_f / 2);
                return (Ac1 * yc1 + Ac2 * yc2) / (Ac1 + Ac2);
            } }

        public double y2 => h-y1;

        public double b { get; set; }
        public double h { get; set; }
        public double d1 { get; set; }
        public double d { get => h - d1; }
        public UnitDimesionType unit { get; set; } = UnitDimesionType.cm;
        public double d2 { get; set; }
        public double As_1 { get; set; }
        public double As_2 { get; set; }
    }
}
