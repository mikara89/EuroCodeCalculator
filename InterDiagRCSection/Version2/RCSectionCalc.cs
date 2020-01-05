using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterDiagRCSection
{
    public interface IMNInteraction
    {
        double N_Rd { get;  }
        double M_Rd { get; }
    }
    public class RCSectionCalc: IMNInteraction
    {
        public readonly ISectionStrainsFactory sectionStrains;
        private readonly ICalcForces calcForces;
        public Dictionary<string, RCSectionForces> Forces; 
        public double N_Rd { get;internal set; }
        public double M_Rd { get; internal set; }  

        public RCSectionCalc(ISectionStrainsFactory sectionStrains, ICalcForces  calcForces)
        {
            Forces = new Dictionary<string, RCSectionForces>();
            this.sectionStrains = sectionStrains;
            this.calcForces = calcForces;

            //Calc();
        }

        public void Calc()
        {
            Forces.Add(key: "Fc", calcForces.Calc(CalcForcesType.ConcreteForces));
            Forces.Add(key: "Fs1", calcForces.Calc(CalcForcesType.ReinforcementForces1));
            Forces.Add(key: "Fs2", calcForces.Calc(CalcForcesType.ReinforcementForces2));

            CalcNM();
        }


        private void CalcNM() 
        {
            var list = Forces.Values.ToList();
            list.ForEach(x =>
            {
                N_Rd += x.F;
            });
            if (!sectionStrains.geometry.IsInverted)
                M_Rd = -(list[0].F * list[0].Z - list[1].F * list[1].Z + list[2].F * list[2].Z) / 100;
            else
                M_Rd = +(list[0].F * list[0].Z - list[1].F * list[1].Z + list[2].F * list[2].Z) / 100;
        }
    }
}
