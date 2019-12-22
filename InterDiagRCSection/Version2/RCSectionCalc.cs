using System.Collections.Generic;
using System.Linq;

namespace InterDiagRCSection
{
    public class RCSectionCalc
    {
        public readonly ISectionStrainsModel sectionStrains;
        private readonly ICalcForces calcForces;

        public Dictionary<string, RCSectionForces> Forces; 
        public double N_Ed { get;internal set; }
        public double M_Ed { get; internal set; } 
        public RCSectionForces Forces_s1 { get; set; }
        public RCSectionForces Forces_s2 { get; set; } 

        public RCSectionCalc(ISectionStrainsModel sectionStrains, ICalcForces  calcForces)
        {
            this.sectionStrains = sectionStrains;
            this.calcForces = calcForces;
            this.calcForces.SetStrains(sectionStrains);
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
                N_Ed += x.F;
                M_Ed += x.F * x.Z;
            });
        }
    }
}
