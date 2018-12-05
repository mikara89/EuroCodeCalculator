using Extensions;
using System;
using TabeleEC2.Model;

namespace CalculatorEC2Logic
{
    public enum TypeDimensioning:int
    {
        Free=1,
        Bound=2,
    }
    public class BendingRectangularCrossSectionEC2 : IDisposable 
    {

        public double Mrd_limit { get; private set; }

        public IForcesBendingAndCompressison Forces { get; set; } 
        public IMaterial Material { get; set; }
        public IElementGeometry Geometry { get; set; }
        public TypeDimensioning TypeDim { get; private set; }
        public double μSd { get; private set; }

        public KofZaProracunPravougaonogPresekaModelEC KofZaProracunPravougaonogPreseka;

        public double As1_pot { get; private set; }
        public double As2_pot { get; private set; }
        /// <summary>
        /// ρ_max = 4%
        /// </summary>
        private double ρ_max => 0.04;
        private KofZaProracunPravougaonogPresekaModelEC Kof_lim
        {
            get { return TabeleEC2.KofZaProracunPravougaonogPresekaEC
                    .GetLimitKofZaProracunPravougaonogPresekaEC(Material.beton); }
        }

        public BendingRectangularCrossSectionEC2(
            IForcesBendingAndCompressison forces,
            IElementGeometry geometry,
            IMaterial material,
            KofZaProracunPravougaonogPresekaModelEC kof = null)
        {
            InitValidations(geometry,material); 
            this.Forces = forces;
            this.Geometry = geometry;
            this.Material = material;
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
            As1_pot = Mrd_limit * 100 / (Kof_lim.ζ * Geometry.d * Material.armatura.fyd) - (Forces.Nsd / Material.armatura.fyd) + (Msds * 100 - Mrd_limit * 100) / ((Geometry.d - Geometry.d2) * Material.armatura.fyd);
            As2_pot = (Msds * 100 - Mrd_limit * 100) / ((Geometry.d - Geometry.d2) * Material.armatura.fyd);
            As2_pot = As2_pot < 0 ? 0 : As2_pot;
            if ((As1_pot + As2_pot) / Geometry.b / Geometry.h > ρ_max)
                throw new Exception("ρ_max prekoraceno! Povećajte poprecni presek");
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
        Msd:        {Forces.Msd.Round()}kN
        Nsd:        {Forces.Nsd.Round()}kNm
        Msds:       {Forces.Msds(Geometry.h, Geometry.d1).Round()}kNm
    Material:
        Armatrua:   {Material.armatura.ToString()}
        Beton:      {Material.beton.ToString()}
    Geometry:
        b:          {Geometry.b}{Geometry.unit}
        h:          {Geometry.h}{Geometry.unit}
        d1:         {Geometry.d1}{Geometry.unit}
        d2:         {Geometry.d2}{Geometry.unit}
        d:          {Geometry.d}{Geometry.unit}
    Result:
        εc/εs1:     {KofZaProracunPravougaonogPreseka.εc.Round(3)}‰/{KofZaProracunPravougaonogPreseka.εs1.Round(3)}‰
        μRd:        {KofZaProracunPravougaonogPreseka.μRd.Round(3)}
        x:          {(KofZaProracunPravougaonogPreseka.ξ * Geometry.d).Round()} cm2
        As1_pot:    {As1_pot.Round() } cm2 
        As2_pot:    {As2_pot.Round() } cm2
        μRd_lim:    {Kof_lim.μRd }";
        }

        private  double Round( double d, int i = 2)
        {
            return Math.Round(d, i);
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
                throw new Exception("Beton not defined!");
            if (material.armatura == null)
                throw new Exception("Armatura not defined!");

        }


        private void SetKof()
        {
            if (TypeDim == TypeDimensioning.Bound)
                μSd = TabeleEC2.KofZaProracunPravougaonogPresekaEC.GetμSd(Forces.Msds(Geometry.h, Geometry.d1), Geometry.b, Geometry.d, Material.beton.fcd / 10);
            else μSd = new KofZaProracunPravougaonogPresekaModelEC(-3.5, 20).μRd;

            KofZaProracunPravougaonogPreseka = TabeleEC2.KofZaProracunPravougaonogPresekaEC.Get_Kof_From_μ(μSd);
        }
        public void Dispose()
        {
            GC.Collect();
        }
    }
}
