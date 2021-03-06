﻿using System;

namespace CalcModels
{
    public class ElementGeometryTShape : IElementGeometryT
    {
        private double _b_eff;
        private double _h_f;

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
        public double b { get; set; }
        public double h { get; set; }
        public double d1 { get; set; }
        public double d { get => h - d1; }
        public UnitDimesionType unit { get; set; } = UnitDimesionType.cm;
        public double d2 { get; set; }
        //public double b_eff
        //{
        //    get
        //    {
        //        if (_b_eff == 0 && _h_f == 0) return b;
        //        return b_eff;
        //    }
        //    set => _b_eff = value;
        //}
        //public double h_f { get => _h_f; set => _h_f = value; }
        public double b_eff { get; set; }
        public double h_f { get; set; }
        public double y1
        {
            get
            {
                var Ac1 = (h - h_f) * b;
                var Ac2 = h_f * b_eff;
                var yc1 = (h - h_f) / 2;
                var yc2 = h - (h_f / 2);
                return (Ac1 * yc1 + Ac2 * yc2) / (Ac1 + Ac2);
            }
        }

        public double y2 => h - y1;
    }
}
