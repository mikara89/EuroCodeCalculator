using System;
using System.Collections.Generic;
using System.Linq;

namespace CalcModels
{
    public class CoeffService : ICoeffService
    {
        public CoeffService(IMaterial material, IElementGeometry geometry)
        {
            Material = material ?? throw new ArgumentNullException(nameof(material));
            Geometry = geometry ?? throw new ArgumentNullException(nameof(geometry));
        }

        public IMaterial Material { get; }
        public IElementGeometry Geometry { get; }

        public CoeffForCalcRectCrossSectionModelEC GetNew()
        {
            return new CoeffForCalcRectCrossSectionModelEC(Material, Geometry);
        }
        public CoeffForCalcRectCrossSectionModelEC GetNew(double eps_c, double eps_s1)
        {
            return new CoeffForCalcRectCrossSectionModelEC(eps_c, eps_s1, Material, Geometry);
        }

        public CoeffForCalcRectCrossSectionModelEC GetByμ(double μSd, int percision = 4)
        {
            var μ_lim = GetNew();
            μ_lim.SetByEcEs1(Material.beton.εcu2, Material.armatura.eps_ud);
            var kofResult = GetNew();

            if (μSd > μ_lim.μRd)
            {
                var max_εs1 = Material.armatura.eps_ud;
                var min_εs1 =Material.beton.εcu2* Geometry.d1/Geometry.h;

                var test_εs1 = (max_εs1 + Math.Abs(min_εs1)) / 2;
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
                var min_εc = Material.beton.εcu2;

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
            var max = GetNew(Material.beton.εcu2, Material.beton.εcu2 * Geometry.d1 / Geometry.h).μRd;
            var min = new CoeffForCalcRectCrossSectionModelEC(0, Material.armatura.eps_ud, Material, Geometry).μRd;
            if (result > max)
                return GetByμ(max);
            //throw new Exception("Diletacija u armaturi i betonu prekoracema; \nPovecajte presek!");
            if (result < min)
                throw new Exception("Diletacija u armaturi i betonu prekoracema; \nPovecajte presek!");
            return GetByμ(result);
        }

        public CoeffForCalcRectCrossSectionModelEC GetByξ(double ξ)
        {
            var t = new CoeffForCalcRectCrossSectionModelEC(Material, Geometry);
            t.SetByξ(ξ);
            return t;
        }

        public CoeffForCalcRectCrossSectionModelEC GetByξ_lim()
        {
            return GetByξ(Material.armatura.lim_ξ);
        }

        public List<CoeffForCalcRectCrossSectionModelEC> GetList()
        {
            List<CoeffForCalcRectCrossSectionModelEC> result = new List<CoeffForCalcRectCrossSectionModelEC>();
            var t = Material.beton.εcu2 * Geometry.d1 / Geometry.h;
            for (double i = t; i >= Material.beton.εcu2; i += -0.1)
            {
                result.Add(new CoeffForCalcRectCrossSectionModelEC(i, Material.armatura.eps_ud, Material, Geometry));
            }
            for (double i = 0; i < Material.armatura.eps_ud; i += 0.5)
            {
                result.Add(new CoeffForCalcRectCrossSectionModelEC(Material.beton.εcu2, i, Material, Geometry));
            }
            result.Add(new CoeffForCalcRectCrossSectionModelEC(Material.beton.εcu2, Material.armatura.eps_ud, Material, Geometry));
            return result.OrderBy(n => n.μRd).ToList();
        }
    }
}
