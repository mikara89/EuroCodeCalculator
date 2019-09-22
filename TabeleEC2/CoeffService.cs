using CalcModels;
using Extensions;
using System;
using TabeleEC2.Model;

namespace TabeleEC2
{
    public class CoeffService : ICoeffService
    {
        public CoeffService(IMaterial material)
        {
            Material = material;
        }

        public IMaterial Material { get; }

        public CoeffForCalcRectCrossSectionModelEC GetNew()
        {
            return new CoeffForCalcRectCrossSectionModelEC(Material.beton.ni);
        }
        public CoeffForCalcRectCrossSectionModelEC GetNew(double eps_c, double eps_s1)
        {
            return new CoeffForCalcRectCrossSectionModelEC(eps_c, eps_s1, Material.beton.ni);
        }

        public CoeffForCalcRectCrossSectionModelEC GetByμ(double μSd, int percision = 4)
        {
            var μ_lim = GetNew();
            μ_lim.SetByEcEs1(-3.5, 20);
            var kofResult = GetNew();

            if (μSd > μ_lim.μRd)
            {
                var max_εs1 = Material.armatura.eps_ud;
                var min_εs1 = -0.7;

                var test_εs1 = (max_εs1 + min_εs1) / 2;
                var adder_εs1 = test_εs1;


                while (μSd.Round(percision) != kofResult.μRd.Round(percision))
                {

                    adder_εs1 = adder_εs1 / 2;

                    kofResult = GetNew(Material.beton.εcu2, test_εs1);
                    if (kofResult.μRd > μSd) test_εs1 += adder_εs1;
                    else test_εs1 -= adder_εs1;
                }
            }
            else if (μSd < μ_lim.μRd)
            {
                var max_εc = 0.0;
                var min_εc = -3.5;

                var test_εc = (max_εc + min_εc) / 2;
                var adder_εc = test_εc;

                while (μSd.Round(percision) != kofResult.μRd.Round(percision))
                {
                    adder_εc = adder_εc / 2;
                    kofResult = GetNew(test_εc, Material.armatura.eps_ud);
                    if (kofResult.μRd > μSd) test_εc -= adder_εc;
                    else test_εc += adder_εc;
                }
            }
            else return μ_lim;

            return kofResult;
        }
        public CoeffForCalcRectCrossSectionModelEC GetByμ(double Msd, double b, double d)
        {
            var result = Msd * 100 / (b * Math.Pow(d, 2) * Material.beton.fcd/10);
            var max = GetNew(Material.beton.εcu2, -0.7).μRd;
            var min = new CoeffForCalcRectCrossSectionModelEC(0, Material.armatura.eps_ud, Material.beton.ni).μRd;
            if (result > max)
                return GetByμ(max);
            //throw new Exception("Diletacija u armaturi i betonu prekoracema; \nPovecajte presek!");
            if (result < min)
                throw new Exception("Diletacija u armaturi i betonu prekoracema; \nPovecajte presek!");
            return GetByμ(result);
        }

        public CoeffForCalcRectCrossSectionModelEC GetByξ(double ξ)
        {
            var t = new CoeffForCalcRectCrossSectionModelEC(Material.beton.ni);
            t.SetByξ(ξ);
            return t;
        }

        public CoeffForCalcRectCrossSectionModelEC GetByξ_lim()
        {
            return GetByξ(Material.armatura.lim_ξ);
        }
    }
}
