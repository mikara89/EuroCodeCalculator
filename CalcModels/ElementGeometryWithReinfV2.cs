﻿using System;

namespace CalcModels
{
    public class ElementGeometryWithReinfV2 : IElementGeometryWithReinfV2
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
        public double b { get; set; }
        public double h { get; set; }
        public double d1 { get; set; }
        public double d { get => h - d1; }
        public UnitDimesionType unit { get; set; } = UnitDimesionType.cm;
        public double d2 { get; set; }
        public double As_1 { get; set; }
        public double As_2 { get; set; }
        public double b_eff { get; set; }
        public double h_f { get; set; }
    }
}
