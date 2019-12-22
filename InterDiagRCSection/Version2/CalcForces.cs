using CalcModels;
using System;

namespace InterDiagRCSection
{
    public class CalcForces : ICalcForces
    {
        private readonly IMaterial material;
        private readonly IElementGeometryWithReinf geometry;
        private ISectionStrainsModel sectionStrains;

        public CalcForces(IMaterial material, IElementGeometryWithReinf geometry) 
        {
            this.material = material 
                ?? throw new ArgumentNullException(nameof(material));
            this.geometry = geometry 
                ?? throw new ArgumentNullException(nameof(geometry));
        }
        public void SetStrains(ISectionStrainsModel sectionStrains)
        {
            this.sectionStrains = sectionStrains 
                ?? throw new ArgumentNullException(nameof(sectionStrains));
        }
        public RCSectionForces Calc(CalcForcesType calcType)
        {
            if (sectionStrains != null)
                throw new ArgumentNullException($"{nameof(sectionStrains)} not set");
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
            throw new NotImplementedException();
        }
        private RCSectionForces ReinforcementForcesCalc(int i) 
        {
            if (i == 1) 
            {
                var z = geometry.d1;
                var s = getSigma(sectionStrains.Get_eps(z));
                return new RCSectionForces
                {
                    Sigma = s,
                    Z = geometry.y1 - geometry.d1,
                    F = geometry.As_1 * s / 10
                };
            }else
            {
                var z = geometry.h- geometry.d2;
                var s = getSigma(sectionStrains.Get_eps(z));
                return new RCSectionForces
                {
                    Sigma = s,
                    Z = geometry.y2 - geometry.d2,
                    F = geometry.As_2 * s / 10
                };
            }
               
        }
        private double getSigma(double eps)
        {
            if (Math.Abs(eps) * material.armatura.Es > material.armatura.fyd * 10)
                return Math.Abs(eps) / eps * material.armatura.fyd * 10;
            else return eps * material.armatura.Es;
        }
    }



}
