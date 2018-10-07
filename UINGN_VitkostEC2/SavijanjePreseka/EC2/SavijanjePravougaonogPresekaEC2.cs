using System;
using System.Collections.Generic;
using System.Text;
using TabeleEC2;
using TabeleEC2.Model;

namespace CalculatorEC2Logic.SavijanjePreseka.EC
{

    public class SavijanjePravougaonogPresekaEC2V2: SavijanjeAbstract
    {
        public double h { get; set; }
        public KofZaProracunPravougaonogPresekaModelEC KofZaProracunPravougaonogPreseka { get; set; }
        public TipDimenzionisanja TipDim { get; set; }

        private bool IsMsdNegativ=false;

        public double Msd { get; private set; }
        public double Mrd_limit { get; private set; }
        public double Nsd { get; private set; }
        public double Msds
        {
            get
            {
                return Msd + Nsd * (h/100 / 2 - d1/100);
            }
        }
        public double Mg { get; private set; }
        public double Mq { get; private set; }
        public double Ng { get; private set; }
        public double Nq { get; private set; }
        public BetonModelEC beton { get; set; }
        public ReinforcementTypeModelEC armatura { get; set; }
        public double b { get; private set; }

        public double d1 { get; private set; }
        public double d2 { get; private set; }
        public double d { get { return h - d1; } }

        public double μSd { get; private set; }


        public double As1_pot { get; private set; }
        public double As2_pot { get; private set; } 
        /// <summary>
        /// ρ_max = 4%
        /// </summary>
        public double ρ_max => 0.04;

        public KofZaProracunPravougaonogPresekaModelEC KofZaProracunPravougaonogPreseka_lim
        {
            get { return TabeleEC2.KofZaProracunPravougaonogPresekaEC.GetLimitKofZaProracunPravougaonogPresekaEC(beton); }
        }

        public SavijanjePravougaonogPresekaEC2V2(
            double b,double h,
            double d1, double d2,
            BetonModelEC beton, ReinforcementTypeModelEC armatura,
            double Mg, double Mq, double Ng=0, double Nq=0, KofZaProracunPravougaonogPresekaModelEC kof=null)
        {
            InitValidations(b, h, beton, armatura, d1, d2);
            this.Mg = Mg;
            this.Mq = Mq;
            this.Msd = 1.35 * Mg + 1.5 * Mq;
            this.Ng = Ng;
            this.Nq = Nq;
            this.Nsd = 1.35 * Ng + 1.5 * Nq;
            this.b = b;
            this.h = h;
            this.d1 = d1;
            this.d2 = d2;
            this.beton = beton;
            this.armatura = armatura;
            if (kof != null) { KofZaProracunPravougaonogPreseka = kof; μSd = kof.μRd; }
            
            Start();
        }
        public SavijanjePravougaonogPresekaEC2V2(
            
            double b, double h,
            double d1, double d2, 
            BetonModelEC beton, ReinforcementTypeModelEC armatura,
            double Msd,
            double Nsd=0, 
            KofZaProracunPravougaonogPresekaModelEC kof = null)
        {
            InitValidations(b, h, beton, armatura, d1,d2);
            this.Msd = Msd;
            this.Nsd = Nsd;
            this.b = b;
            this.h = h;
            this.d1 = d1;
            this.d2 = d2;
            this.beton = beton;
            this.armatura = armatura;
            if (kof != null) { KofZaProracunPravougaonogPreseka = kof; μSd = kof.μRd; }
            
            Start();
        }

     
        /// <summary>
        /// If TipDim == TipDimenzionisanja.Slobodno
        /// </summary>
        public override void SlobodnoDim()
        {
            if (μSd > KofZaProracunPravougaonogPreseka_lim.μRd) DvostrukoArmiranje(); else Armiranje();
        }
        public override void sethSlobodnoDim()
        {
            double d_r = 0;
            double h_r = 0;
            d_r = Math.Sqrt(Msds * 100 / (b * KofZaProracunPravougaonogPreseka.μRd * beton.fcd / 10));
            h_r = d_r + this.d1;
            if (h_r % 5 != 0) h = h_r - (h_r % 5) + 5;
            else h = h_r;
        }
        /// <summary>
        /// If TipDim == TipDimenzionisanja.Vezano
        /// </summary>
        public override void VezanoDim()
        {
            if (μSd > KofZaProracunPravougaonogPreseka_lim.μRd) DvostrukoArmiranje(); else Armiranje();
        }

        public override void Armiranje() 
        {
            As1_pot = KofZaProracunPravougaonogPreseka.ω * b * d * beton.fcd / (armatura.fyd * 10)-(Nsd/armatura.fyd);
            if (As1_pot / b / h > ρ_max) throw new Exception("ρ_max prekoraceno! Povećajte poprecni presek");
        }

        public override void DvostrukoArmiranje()
        {
            Mrd_limit = (KofZaProracunPravougaonogPreseka_lim.μRd * b * Math.Pow(d, 2) * beton.fcd / 10)/100;
            As1_pot = Mrd_limit * 100 / (KofZaProracunPravougaonogPreseka_lim.ζ * d * armatura.fyd) - (Nsd / armatura.fyd) + (Msds*100- Mrd_limit*100) / ((d - d2) * armatura.fyd);
            As2_pot = (Msds * 100 - Mrd_limit * 100) / ((d - d2) * armatura.fyd);
            if ((As1_pot+ As2_pot) / b / h > ρ_max) throw new Exception("ρ_max prekoraceno! Povećajte poprecni presek");
        }
        public override string ToString()
        {
            return String.Format(@"///Osnovni podaci///
b/h={5}/{6} cm;
d1={7} cm; d2={14} cm;
μSd={8}; 
Beton:{9}
Armatura:{10}
Msd={11} kNm; Nsd={12} kN;
///Izračunato///
Msds={0} kNm
εc={1}‰ εs1={2}‰
μRd={15}
x={16} cm2
As1_pot={3} cm2
As2_pot={4} cm2
μRd_lim={13}
", Round(Msds),
Round(KofZaProracunPravougaonogPreseka.εc, 3),
Round(KofZaProracunPravougaonogPreseka.εs1, 3),
Round(As1_pot),
Round(As2_pot),
b, h, d1, Round(μSd, 3),
beton.name, armatura.name,
Round(Msd), Round(Nsd), KofZaProracunPravougaonogPreseka_lim.μRd, d2,
Round(KofZaProracunPravougaonogPreseka.μRd, 3), Round(KofZaProracunPravougaonogPreseka.ξ * d));
        }



        public override void SetKof()
        {
            if (TipDim == TipDimenzionisanja.Vezano)
                μSd= TabeleEC2.KofZaProracunPravougaonogPresekaEC.GetμSd(Msds, b, d, beton.fcd / 10);
            else μSd = new KofZaProracunPravougaonogPresekaModelEC(-3.5, 20).μRd;

            KofZaProracunPravougaonogPreseka = TabeleEC2.KofZaProracunPravougaonogPresekaEC.Get_Kof_From_μ(μSd);       
        }

        public override void Start()

        {
            if (Msd < 0) { IsMsdNegativ = true; Msd = Math.Abs(Msd); }
            TipDim = h == 0 ? TipDimenzionisanja.Slobodno : TipDimenzionisanja.Vezano;
            if (KofZaProracunPravougaonogPreseka == null) SetKof();
            if (TipDim == TipDimenzionisanja.Slobodno) SlobodnoDim(); else VezanoDim();
        }
    }
}
