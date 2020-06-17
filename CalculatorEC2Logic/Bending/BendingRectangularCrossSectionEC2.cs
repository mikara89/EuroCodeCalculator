using CalcModels;
using System;

namespace CalculatorEC2Logic
{
    public enum TypeDimensioning:int
    {
        Free=1,
        Bound=2,
    }
    public class BendingRectangularCrossSectionEC2 : IDisposable 
    {
        private readonly ICoeffService coeffService;
        public double Mrd_limit { get; private set; }

        public IForcesBendingAndCompressison Forces { get; set; } 
        public IMaterial Material { get; set; }
        public IElementGeometry Geometry { get; set; }
        public TypeDimensioning TypeDim { get; private set; }
        public double μSd { get; private set; }

        public CoeffForCalcRectCrossSectionModelEC KofZaProracunPravougaonogPreseka;

        public double As1_pot { get; private set; }
        public double As2_pot { get; private set; }
        /// <summary>
        /// ρ_max = 4%
        /// </summary>
        private double ρ_max => 0.04;
        private CoeffForCalcRectCrossSectionModelEC Kof_lim { get; set; }

        public double X { get; private set; }

        public BendingRectangularCrossSectionEC2(
            IForcesBendingAndCompressison forces,
            IElementGeometry geometry,
            IMaterial material,
            CoeffForCalcRectCrossSectionModelEC kof = null)
        {
            InitValidations(geometry, material);
            this.Forces = forces;
            this.Geometry = geometry;
            this.Material = material;

            coeffService = new CoeffService(Material,Geometry);

            Kof_lim = coeffService.GetByξ(material.beton.ξ_lim);


            if (kof != null)
            {
                KofZaProracunPravougaonogPreseka = kof;
                μSd = kof.μRd;
            }

            Start();
        }

        private void Start()
        {
            TypeDim = Geometry.h == 0 ? TypeDimensioning.Free : TypeDimensioning.Bound;
            if (KofZaProracunPravougaonogPreseka == null) SetKof();
            if (TypeDim == TypeDimensioning.Free) FreeDim(); else BoundDim();
        }

        /// <summary>
        /// If TypeDim == TypeDimensioning.Free
        /// </summary>
        private void FreeDim()
        {
            double d_r = 0;
            double h_r = 0;
            d_r = Math.Sqrt(Forces.Msds(Geometry.h,Geometry.d) * 100 / (Geometry.b * KofZaProracunPravougaonogPreseka.μRd * Material.beton.fcd / 10));
            h_r = d_r + this.Geometry.d1;
            if (h_r % 5 != 0) Geometry.h = h_r - (h_r % 5) + 5;
            else Geometry.h = h_r;
            ReinforcementCalc();
        }
        /// <summary>
        /// If TypeDim == TypeDimensioning.Bound
        /// </summary>
        private void BoundDim()
        {
            ReinforcementCalc();
        }

        private void ReinforcementCalc() 
        {

            var Msds = Forces.Msds(Geometry.h, Geometry.d1);

            Mrd_limit = (Kof_lim.μRd * Geometry.b * Math.Pow(Geometry.d, 2) * Material.beton.fcd / 10) / 100;

            if(Msds<= Mrd_limit)
            {
                As1_pot = (KofZaProracunPravougaonogPreseka.ω * Geometry.b * Geometry.d * Material.beton.fcd/10 / Material.armatura.fyd) + (Forces.Nsd / Material.armatura.fyd);
            }
            else
            {
                As2_pot = (Msds * 100 - Mrd_limit * 100) / ((Geometry.d - Geometry.d2) * Material.armatura.fyd);
                As1_pot = Mrd_limit * 100 / (Kof_lim.ζ * Geometry.d * Material.armatura.fyd) + (Forces.Nsd / Material.armatura.fyd) + As2_pot;

                As2_pot = As2_pot < 0 ? 0 : As2_pot;
            }
           
            if ((As1_pot + As2_pot) / Geometry.b / Geometry.h > ρ_max)
                throw new Exception("ρ_max exceeded! Make section bigger");

            if (As2_pot != 0)
            {
                KofZaProracunPravougaonogPreseka = Kof_lim;
            }
            X = KofZaProracunPravougaonogPreseka.ξ * Geometry.d;

            if (Forces.IsMsdNegativ)
            {
                As2_pot = As1_pot;
                As1_pot = (Msds * 100 - Mrd_limit * 100) / ((Geometry.d - Geometry.d2) * Material.armatura.fyd);
            }
        
        }
        public override string ToString()
        {
            return $@"//////Result///////
    Forces:
        {"Msd:",-12}{Forces.Msd,13:F2}{"kNm",-5}
        {"Nsd:",-12}{Forces.Nsd,13:F2}{"kN",-5}
        {"Msds:",-12}{Forces.Msds(Geometry.h, Geometry.d1),13:F2}{"kNm",-5} 
    Material:
        Reinforcement:  {Material.armatura}
        Concrete:       {Material.beton}
    Geometry:
        {"b:",-12}{Geometry.b,13:F2}{Geometry.unit,-5}
        {"h:",-12}{Geometry.h,13:F2}{Geometry.unit,-5}
        {"d1:",-12}{Geometry.d1,13:F2}{Geometry.unit,-5}
        {"d2:",-12}{Geometry.d2,13:F2}{Geometry.unit,-5}
        {"d:",-12}{Geometry.d,13:F2}{Geometry.unit,-5}
    Result:
        {"εc/εs1:",-12}{KofZaProracunPravougaonogPreseka.εc,13:F3}{"/",1}{KofZaProracunPravougaonogPreseka.εs1,-5:F3}{"‰",1}
        {"εs2:",-12}{KofZaProracunPravougaonogPreseka.εs2(Geometry.d, Geometry.d2),13:F3}{"‰",-5}
        {"μSd:",-12}{KofZaProracunPravougaonogPreseka.μRd,13:F3}{"",-5}
        {"x:",-12}{X,13:F2}{"",-5}
        {"As1_req:",-12}{As1_pot,13:F2}{"cm2",-5}
        {"As2_req:",-12}{As2_pot,13:F2}{"cm2",-5}
        {"μSd_lim:",-12}{Kof_lim.μRd,13:F3}";
        }

        private void InitValidations(IElementGeometry geometry, IMaterial material)
        {
            if (geometry.b <= 0)
                throw new Exception("b must be greater 0");
            if (geometry.h < 0)
                throw new Exception("h must be greater or equal to 0");
            if (geometry.d1 <= 0)
                throw new Exception("d1 must be greater 0");
            if (geometry.d2 <= 0)
                throw new Exception("d2 must be greater 0");
            if (2 * geometry.d1 >= geometry.h && geometry.h != 0)
                throw new Exception("2 x d1 must be smaller then h");
            if (material.beton == null)
                throw new Exception("Concrete not defined!");
            if (material.armatura == null)
                throw new Exception("Reinforcement not defined!");

        }


        private void SetKof()
        { 
            if (TypeDim == TypeDimensioning.Bound)
                μSd = coeffService.GetByμ(Forces.Msds(Geometry.h, Geometry.d1), Geometry.b, Geometry.d).μRd;
            else μSd = coeffService.GetNew(Material.beton.εcu2, Material.armatura.eps_ud).μRd;

            KofZaProracunPravougaonogPreseka = coeffService.GetByμ(μSd);
        }
        public void Dispose()
        {
            GC.Collect();
        }

        public static double NA(IMaterial material,IElementGeometry geometry, double As1,double As2)
        {
            var alfa_e = material.armatura.Es * 1000 / material.beton.Ecm ;
            var A =geometry.b * 0.5;
            var B = alfa_e * As2 + alfa_e * As1;
            var C = alfa_e * As2 * (-geometry.d1) + alfa_e * As1 * (-geometry.d);
            var x1 = (-B + Math.Sqrt(Math.Pow(B, 2) - 4 * A * C)) / (2 * A);
            var x2 = (-B - Math.Sqrt(Math.Pow(B, 2) - 4 * A * C)) / (2 * A);
            return Math.Max(x1, x2);
        }

        
    }
}
