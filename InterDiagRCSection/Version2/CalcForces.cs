using CalcModels;
using System;

namespace InterDiagRCSection
{
    public class CalcForces : ICalcForces
    {
        private readonly IConcreteForceCalc concreteForce;

        public ISectionStrainsModel sectionStrains { get; internal set; }

        public CalcForces(IConcreteForceCalc concreteForce) 
        {
            this.concreteForce = concreteForce
               ?? throw new ArgumentNullException(nameof(concreteForce));
            this.sectionStrains = concreteForce.sectionStrains;
           
        }
        public RCSectionForces Calc(CalcForcesType calcType)
        {
            switch (calcType)
            {

                case CalcForcesType.ConcreteForces:
                    return ConcreteForcesCalc();
                case CalcForcesType.ReinforcementForces1:
                    return ReinforcementForcesCalc((int)CalcForcesType.ReinforcementForces1);
                case CalcForcesType.ReinforcementForces2:
                    return ReinforcementForcesCalc((int)CalcForcesType.ReinforcementForces2);
                default:
                    throw new NotImplementedException();
            }
        }
        private RCSectionForces ConcreteForcesCalc()
        {
            return new RCSectionForces {
                F = concreteForce.GetForce(),
                Z = concreteForce.GetDistance(),
                Sigma = sectionStrains.Get_sig(sectionStrains.geometry.h)
           };
        }
        private RCSectionForces ReinforcementForcesCalc(int i) 
        {
            if (i == 1) 
            {
                var z = sectionStrains.geometry.d1;
                var s = sectionStrains.GetSigmaForReinf(sectionStrains.Get_eps(z));
                return new RCSectionForces
                {
                    Sigma = s,
                    Z = sectionStrains.geometry.y1 - sectionStrains.geometry.d1,
                    F = sectionStrains.geometry.As_1 * s / 10
                };
            }else
            {
                var z = sectionStrains.geometry.h- sectionStrains.geometry.d2;
                var s = sectionStrains.GetSigmaForReinf(sectionStrains.Get_eps(z));
                return new RCSectionForces
                {
                    Sigma = s,
                    Z = sectionStrains.geometry.y2 - sectionStrains.geometry.d2,
                    F = sectionStrains.geometry.As_2 * s / 10
                };
            }
               
        }
    }
}
