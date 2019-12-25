using System;
using System.Collections.Generic;
using System.Linq;

namespace InterDiagRCSection
{
    public class RCSectionCalc
    {
        public readonly ISectionStrainsModel sectionStrains;
        private readonly ICalcForces calcForces;
        public Dictionary<string, RCSectionForces> Forces; 
        public double N_Rd { get;internal set; }
        public double M_Rd { get; internal set; } 
        public RCSectionForces Forces_s1 { get; set; }
        public RCSectionForces Forces_s2 { get; set; } 

        public RCSectionCalc(ISectionStrainsModel sectionStrains, ICalcForces  calcForces)
        {
            Forces = new Dictionary<string, RCSectionForces>();
            this.sectionStrains = sectionStrains;
            this.calcForces = calcForces;

            Calc();
            CalcNM();
        }

        private void Calc()
        {
            Forces.Add(key: "Fc", calcForces.Calc(CalcForcesType.ConcreteForces));
            Forces.Add(key: "Fs1", calcForces.Calc(CalcForcesType.ReinforcementForces1));
            Forces.Add(key: "Fs2", calcForces.Calc(CalcForcesType.ReinforcementForces2));
        }

        private void CalcNM() 
        {
            var list = Forces.Values.ToList();
            list.ForEach(x =>
            {
                N_Rd -= x.F;
            });
            if (!sectionStrains.geometry.IsInverted)
                M_Rd = (list[0].F * Math.Abs(list[0].Z) - list[1].F * list[1].Z + list[2].F * list[2].Z) / 100;
            else
                M_Rd = -(list[0].F * Math.Abs(list[0].Z) - list[1].F * list[1].Z + list[2].F * list[2].Z) / 100;
        }
    }
}
