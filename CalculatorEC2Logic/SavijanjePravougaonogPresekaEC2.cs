using Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using TabeleEC2.Model;

namespace CalculatorEC2Logic
{
    public enum TipDimenzionisanja:int
    {
        Slobodno=1,
        Vezano=2,
    }
//    public class SavijanjePravougaonogPresekaEC2:IDisposable
//    {

//        public double Msd { get; private set; }
//        public double Mrd_limit { get; private set; }
//        public double Nsd { get; private set; }
//        public double Msds
//        {
//            get
//            {
//                return Msd + Nsd * (h/100 / 2 - d1/100);
//            }
//        }
//        public double Mg { get; private set; }
//        public double Mq { get; private set; }
//        public double Ng { get; private set; }
//        public double Nq { get; private set; }
//        public BetonModelEC beton { get; set; }
//        public ReinforcementTypeModelEC armatura { get; set; }
//        public double b { get; private set; }
//        public double h { get; private set; }
//        public double d1 { get; private set; }
//        public double d2 { get; private set; }
//        public double d { get { return h - d1; } }
//        public TipDimenzionisanja TipDim { get; private set; }
//        public double μSd { get; private set; }
//        private bool IsMsdNegativ = false;
//        public KofZaProracunPravougaonogPresekaModelEC KofZaProracunPravougaonogPreseka { get; private set; }

//        public double As1_pot { get; private set; }
//        public double As2_pot { get; private set; } 
//        /// <summary>
//        /// ρ_max = 4%
//        /// </summary>
//        public double ρ_max => 0.04;
//        //public double μ_lim => 0.252;
//        public KofZaProracunPravougaonogPresekaModelEC KofZaProracunPravougaonogPreseka_lim
//        {
//            get { return TabeleEC2.KofZaProracunPravougaonogPresekaEC.GetLimitKofZaProracunPravougaonogPresekaEC(beton); }
//        }

//        public SavijanjePravougaonogPresekaEC2(
//            double b,double h,
//            double d1, double d2,
//            BetonModelEC beton, ReinforcementTypeModelEC armatura,
//            double Mg, double Mq, double Ng=0, double Nq=0, KofZaProracunPravougaonogPresekaModelEC kof=null)
//        {
//            InitValidations(b, h, beton, armatura, d1, d2);
//            this.Mg = Mg;
//            this.Mq = Mq;
//            this.Msd = 1.35 * Mg + 1.5 * Mq;
//            this.Ng = Ng;
//            this.Nq = Nq;
//            this.Nsd = 1.35 * Ng + 1.5 * Nq;
//            this.b = b;
//            this.h = h;
//            this.d1 = d1;
//            this.d2 = d2;
//            this.beton = beton;
//            this.armatura = armatura;
//            if (kof != null) { KofZaProracunPravougaonogPreseka = kof; μSd = kof.μRd; }
            
//            Start();
//        }
//        public SavijanjePravougaonogPresekaEC2(
            
//            double b, double h,
//            double d1, double d2, 
//            BetonModelEC beton, ReinforcementTypeModelEC armatura,
//            double Msd,
//            double Nsd=0, 
//            KofZaProracunPravougaonogPresekaModelEC kof = null)
//        {
//            InitValidations(b, h, beton, armatura, d1,d2);
//            this.Msd = Msd;
//            this.Nsd = Nsd;
//            this.b = b;
//            this.h = h;
//            this.d1 = d1;
//            this.d2 = d2;
//            this.beton = beton;
//            this.armatura = armatura;
//            if (kof != null) { KofZaProracunPravougaonogPreseka = kof; μSd = kof.μRd; }
            
//            Start();
//        }

//        private void Start()
//        {
//            if (Msd < 0) { IsMsdNegativ = true; Msd = Math.Abs(Msd); }
//            TipDim = h == 0 ? TipDimenzionisanja.Slobodno : TipDimenzionisanja.Vezano;
//            if(KofZaProracunPravougaonogPreseka==null) SetKof();
//            if (TipDim == TipDimenzionisanja.Slobodno) SlobodnoDim(); else VezanoDim();
            
//        }

//        /// <summary>
//        /// If TipDim == TipDimenzionisanja.Slobodno
//        /// </summary>
//        private void SlobodnoDim()
//        {
//            double d_r = 0;
//            double h_r = 0;
//            d_r = Math.Sqrt(Msds * 100 / (b * KofZaProracunPravougaonogPreseka.μRd * beton.fcd / 10));
//            h_r = d_r + this.d1;
//            if (h_r % 5 != 0) h = h_r - (h_r % 5) + 5;
//            else h = h_r;
//            if (μSd > KofZaProracunPravougaonogPreseka_lim.μRd) DvostrukoArmiranje(); else Armiranje();
//        }
//        /// <summary>
//        /// If TipDim == TipDimenzionisanja.Vezano
//        /// </summary>
//        private void VezanoDim()
//        {
//            if (μSd > KofZaProracunPravougaonogPreseka_lim.μRd) DvostrukoArmiranje(); else Armiranje();
           
//        }

//        private void Armiranje() 
//        {
//            As1_pot = KofZaProracunPravougaonogPreseka.ω * b * d * beton.fcd / (armatura.fyd * 10)-(Nsd/armatura.fyd);
//            if (As1_pot / b / h > ρ_max) throw new Exception("ρ_max prekoraceno! Povećajte poprecni presek");
//            if (IsMsdNegativ) { As2_pot = As1_pot; As1_pot = 0;  }
//        }

//        private void DvostrukoArmiranje()
//        {
//            Mrd_limit = (KofZaProracunPravougaonogPreseka_lim.μRd * b * Math.Pow(d, 2) * beton.fcd / 10)/100;
//            As1_pot = Mrd_limit * 100 / (KofZaProracunPravougaonogPreseka_lim.ζ * d * armatura.fyd) - (Nsd / armatura.fyd) + (Msds*100- Mrd_limit*100) / ((d - d2) * armatura.fyd);
//            As2_pot = (Msds * 100 - Mrd_limit * 100) / ((d - d2) * armatura.fyd);
//            if ((As1_pot+ As2_pot) / b / h > ρ_max) throw new Exception("ρ_max prekoraceno! Povećajte poprecni presek");
//            if (IsMsdNegativ) { As2_pot = As1_pot; As1_pot = (Msds * 100 - Mrd_limit * 100) / ((d - d2) * armatura.fyd); }
//        }
//        public override string ToString()
//        {
//            return String.Format(@"///Osnovni podaci///
//b/h={5}/{6} cm;
//d1={7} cm; d2={14} cm;
//μSd={8}; 
//Beton:{9}
//Armatura:{10}
//Msd={11} kNm; Nsd={12} kN;
/////Izračunato///
//Msds={0} kNm
//εc={1}‰ εs1={2}‰
//μRd={15}
//x={16} cm2
//As1_pot={3} cm2
//As2_pot={4} cm2
//μRd_lim={13}
//", Round(Msds),
//Round(KofZaProracunPravougaonogPreseka.εc,3),
//Round(KofZaProracunPravougaonogPreseka.εs1,3), 
//Round(As1_pot),
//Round(As2_pot),
//b,h,d1,Round(μSd,3),
//beton.name,armatura.name,
//Round(Msd),Round(Nsd),KofZaProracunPravougaonogPreseka_lim.μRd,d2,
//Round(KofZaProracunPravougaonogPreseka.μRd,3), Round(KofZaProracunPravougaonogPreseka.ξ*d));
//        }
//        private double Round(double d,int i=2)
//        {
//            return Math.Round(d,i);
//        }
//        private void InitValidations(double b, double h, BetonModelEC beton, ReinforcementTypeModelEC armatura, double d1, double d2)
//        {
//            if (b <= 0)
//                throw new Exception("b must be greater 0");
//            if (h <0)
//                throw new Exception("h must be greater or equal to 0");
//            if (d1 <= 0)
//                throw new Exception("d1 must be greater 0");
//            if (d2 <= 0)
//                throw new Exception("d2 must be greater 0");
//            if (2 * d1 >= h && h!=0)
//                throw new Exception("2 x d1 must be smoller then h");
//            if (beton == null)
//                throw new Exception("Beton not defined!");
//            if (armatura == null)
//                throw new Exception("Armatura not defined!");
            
//        }


//        private void SetKof()
//        {
//            if (TipDim == TipDimenzionisanja.Vezano)
//                μSd= TabeleEC2.KofZaProracunPravougaonogPresekaEC.GetμSd(Msds, b, d, beton.fcd / 10);
//            else μSd =new KofZaProracunPravougaonogPresekaModelEC(-3.5,20).μRd;

//            KofZaProracunPravougaonogPreseka= TabeleEC2.KofZaProracunPravougaonogPresekaEC.Get_Kof_From_μ(μSd);       
//        }
//        public void Dispose()
//        {
//            GC.Collect();
//        }
//    }


    public class SavijanjePravougaonogPresekaEC2_V2 : IDisposable 
    {

        public double Mrd_limit { get; private set; }

        public IForcesBendingAndCompressison Forces { get; set; } 
        public IMaterial Material { get; set; }
        public IElementGeometry geometry { get; set; }
        public TipDimenzionisanja TipDim { get; private set; }
        public double μSd { get; private set; }

        public KofZaProracunPravougaonogPresekaModelEC KofZaProracunPravougaonogPreseka;

        public double As1_pot { get; private set; }
        public double As2_pot { get; private set; }
        /// <summary>
        /// ρ_max = 4%
        /// </summary>
        private double ρ_max => 0.04;
        private KofZaProracunPravougaonogPresekaModelEC KofZaProracunPravougaonogPreseka_lim
        {
            get { return TabeleEC2.KofZaProracunPravougaonogPresekaEC
                    .GetLimitKofZaProracunPravougaonogPresekaEC(Material.beton); }
        }

        public SavijanjePravougaonogPresekaEC2_V2(
            IForcesBendingAndCompressison Forces,
            IElementGeometry geometry,
            IMaterial material,
            KofZaProracunPravougaonogPresekaModelEC kof = null)
        {
            InitValidations(geometry,material);
            this.Forces = Forces;
            this.geometry = geometry;
            this.Material = material;
            if (kof != null) { KofZaProracunPravougaonogPreseka = kof; μSd = kof.μRd; }

            Start();
        }

        private void Start()
        {
            TipDim = geometry.h == 0 ? TipDimenzionisanja.Slobodno : TipDimenzionisanja.Vezano;
            if (KofZaProracunPravougaonogPreseka == null) SetKof();
            if (TipDim == TipDimenzionisanja.Slobodno) SlobodnoDim(); else VezanoDim();
        }

        /// <summary>
        /// If TipDim == TipDimenzionisanja.Slobodno
        /// </summary>
        private void SlobodnoDim()
        {
            double d_r = 0;
            double h_r = 0;
            d_r = Math.Sqrt(Forces.Msds(geometry.h,geometry.d) * 100 / (geometry.b * KofZaProracunPravougaonogPreseka.μRd * Material.beton.fcd / 10));
            h_r = d_r + this.geometry.d1;
            if (h_r % 5 != 0) geometry.h = h_r - (h_r % 5) + 5;
            else geometry.h = h_r;
            Armiranje();
        }
        /// <summary>
        /// If TipDim == TipDimenzionisanja.Vezano
        /// </summary>
        private void VezanoDim()
        {
            Armiranje();
        }

        private void Armiranje() 
        {
            var Msds = Forces.Msds(geometry.h, geometry.d1);
            Mrd_limit = (KofZaProracunPravougaonogPreseka_lim.μRd * geometry.b * Math.Pow(geometry.d, 2) * Material.beton.fcd / 10) / 100;
            As1_pot = Mrd_limit * 100 / (KofZaProracunPravougaonogPreseka_lim.ζ * geometry.d * Material.armatura.fyd) - (Forces.Nsd / Material.armatura.fyd) + (Msds * 100 - Mrd_limit * 100) / ((geometry.d - geometry.d2) * Material.armatura.fyd);
            As2_pot = (Msds * 100 - Mrd_limit * 100) / ((geometry.d - geometry.d2) * Material.armatura.fyd);
            As2_pot = As2_pot < 0 ? 0 : As2_pot;
            if ((As1_pot + As2_pot) / geometry.b / geometry.h > ρ_max)
                throw new Exception("ρ_max prekoraceno! Povećajte poprecni presek");
            if (Forces.IsMsdNegativ)
            {
                As2_pot = As1_pot;
                As1_pot = (Msds * 100 - Mrd_limit * 100) / ((geometry.d - geometry.d2) * Material.armatura.fyd);
            }
        }
        public override string ToString()
        {
            return $@"//////Result///////
    Forces:
        Msd:        {Forces.Msd.Round()}kN
        Nsd:        {Forces.Nsd.Round()}kNm
        Msds:       {Forces.Msds(geometry.h, geometry.d1).Round()}kNm
    Material:
        Armatrua:   {Material.armatura.ToString()}
        Beton:      {Material.beton.ToString()}
    Geometry:
        b:          {geometry.b}{geometry.unit}
        h:          {geometry.h}{geometry.unit}
        d1:         {geometry.d1}{geometry.unit}
        d2:         {geometry.d2}{geometry.unit}
        d:          {geometry.d}{geometry.unit}
    Result:
        εc/εs1:     {KofZaProracunPravougaonogPreseka.εc.Round(3)}‰/{KofZaProracunPravougaonogPreseka.εs1.Round(3)}‰
        μRd:        {KofZaProracunPravougaonogPreseka.μRd.Round(3)}
        x:          {(KofZaProracunPravougaonogPreseka.ξ * geometry.d).Round()} cm2
        As1_pot:    {As1_pot.Round() } cm2 
        As2_pot:    {As2_pot.Round() } cm2
        μRd_lim:    {KofZaProracunPravougaonogPreseka_lim.μRd }";
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
                throw new Exception("2 x d1 must be smoller then h");
            if (material.beton == null)
                throw new Exception("Beton not defined!");
            if (material.armatura == null)
                throw new Exception("Armatura not defined!");

        }


        private void SetKof()
        {
            if (TipDim == TipDimenzionisanja.Vezano)
                μSd = TabeleEC2.KofZaProracunPravougaonogPresekaEC.GetμSd(Forces.Msds(geometry.h, geometry.d1), geometry.b, geometry.d, Material.beton.fcd / 10);
            else μSd = new KofZaProracunPravougaonogPresekaModelEC(-3.5, 20).μRd;

            KofZaProracunPravougaonogPreseka = TabeleEC2.KofZaProracunPravougaonogPresekaEC.Get_Kof_From_μ(μSd);
        }
        public void Dispose()
        {
            GC.Collect();
        }
    }
}
