//using System;
//using System.Collections.Generic;
//using System.Text;
//using TabeleEC2;
//using TabeleEC2.Model;
//using static TabeleEC2.Model.KofZaProracunPravougaonogPresekaModelEC;

//namespace CalculatorEC2Logic.SavijanjePreseka.PBAB
//{

//    public class SavijanjePravougaonogPresekaPBAB : SavijanjeAbstract
//    {
//        public double h { get; set; }
//        public KofZaProracunPravougaonogPresekaModelPBAB KofZaProracunPravougaonogPreseka { get; set; } 
//        public TipDimenzionisanja TipDim { get; set; }
//        public double Mu { get; private set; }
//        public double Mabu { get; private set; }
//        public double Nu { get; private set; }
//        public double Mau
//        {
//            get
//            {
//                return Mu + Nu * (h / 100 / 2 - a1 / 100);
//            }
//        }
//        public double Mg { get; private set; }
//        public double Mp { get; private set; }
//        public double Ng { get; private set; }
//        public double Np { get; private set; }
//        public BetonModelPBAB beton { get; set; }
//        public ReinforcementTypeModel armatura { get; set; }
//        public double b { get; private set; }

//        public double a1 { get; private set; }
//        public double a2 { get; private set; }
//        public double d { get { return h - a1; } }

//        public double k { get; private set; }


//        public double Aa1_pot { get; private set; }
//        public double Aa2_pot { get; private set; }
//        /// <summary>
//        /// ρ_max = 4%
//        /// </summary>
//        public double ρ_max => 0.04;

//        public KofZaProracunPravougaonogPresekaModelPBAB KofZaProracunPravougaonogPreseka_lim
//        {
//            get { return TabeleEC2.KofZaProracunPravougaonogPreseka.GetLimitKofZaProracunPravougaonogPresekaPBAB(); }
//        }

//        public SavijanjePravougaonogPresekaPBAB(
//            double b, double h,
//            double a1, double a2,
//            BetonModelPBAB beton, ReinforcementTypeModel armatura,
//            double Mg, double Mp, double Ng = 0, double Np = 0, KofZaProracunPravougaonogPresekaModelPBAB kof = null)
//        {
//            InitValidations(b, h, beton, armatura, a1, a2);
//            this.Mg = Mg;
//            this.Mp = Mp;
//            this.Mu = 1.35 * Mg + 1.5 * Mp;
//            this.Ng = Ng;
//            this.Np = Np;
//            this.Nu = 1.35 * Ng + 1.5 * Np;
//            this.b = b;
//            this.h = h;
//            this.a1 = a1;
//            this.a2 = a2;
//            this.beton = beton;
//            this.armatura = armatura;
//            if (kof != null) { KofZaProracunPravougaonogPreseka = kof; k = kof.k; }

//            Start();
//        }
//        public SavijanjePravougaonogPresekaPBAB(

//            double b, double h,
//            double a1, double a2,
//            BetonModelPBAB beton, ReinforcementTypeModel armatura,
//            double Mu,
//            double Nu = 0,
//            KofZaProracunPravougaonogPresekaModelPBAB kof = null)
//        {
//            InitValidations(b, h, beton, armatura, a1, a2);
//            this.Mu = Mu;
//            this.Nu = Nu;
//            this.b = b;
//            this.h = h;
//            this.a1 = a1;
//            this.a2 = a2;
//            this.beton = beton;
//            this.armatura = armatura;
//            if (kof != null) { KofZaProracunPravougaonogPreseka = kof; k = kof.k; }

//            Start();
//        }


//        /// <summary>
//        /// If TipDim == TipDimenzionisanja.Slobodno
//        /// </summary>
//        public override void SlobodnoDim()
//        {
//            if (k > KofZaProracunPravougaonogPreseka_lim.k) DvostrukoArmiranje(); else Armiranje();
//        }
//        public override void sethSlobodnoDim()
//        {
//            double d_r = 0;
//            double h_r = 0;
//            d_r = Math.Sqrt(Mau * 100 / Math.Sqrt(b * KofZaProracunPravougaonogPreseka.fc));
//            h_r = d_r + this.a1;
//            if (h_r % 5 != 0) h = h_r - (h_r % 5) + 5;
//            else h = h_r;
//        }
//        /// <summary>
//        /// If TipDim == TipDimenzionisanja.Vezano
//        /// </summary>
//        public override void VezanoDim()
//        {
//            if (k > KofZaProracunPravougaonogPreseka_lim.k) DvostrukoArmiranje(); else Armiranje();
//        }

//        public override void Armiranje()
//        {
//            Aa1_pot = KofZaProracunPravougaonogPreseka.μ_1M * b * d * beton.fb / (armatura.fyd * 10) - (Nu / armatura.fyd);
//            if (Aa1_pot / b / h > ρ_max) throw new Exception("ρ_max prekoraceno! Povećajte poprecni presek");
//        }

//        public override void DvostrukoArmiranje()
//        {
//            Mabu = (KofZaProracunPravougaonogPreseka_lim.k * b * Math.Pow(d, 2) * beton.fb / 10) / 100;
//            Aa1_pot = Mabu * 100 / (KofZaProracunPravougaonogPreseka_lim.ζ * d * armatura.fyd) - (Nu / armatura.fyd) + (Mau * 100 - Mabu * 100) / ((d - a2) * armatura.fyd);
//            Aa2_pot = (Mau * 100 - Mabu * 100) / ((d - a2) * armatura.fyd);
//            if ((Aa1_pot + Aa2_pot) / b / h > ρ_max) throw new Exception("ρ_max prekoraceno! Povećajte poprecni presek");
//        }
//        public override string ToString()
//        {
//            return String.Format(@"///Osnovni podaci///
//b/h={5}/{6} cm;
//a1={7} cm; a2={14} cm;
//k={8}; 
//Beton:{9}
//Armatura:{10}
//Mu={11} kNm; Nu={12} kN;
/////Izračunato///
//Mau={0} kNm
//εb={1}‰ εa1={2}‰
//As1_pot={3} cm2
//As2_pot={4} cm2
//k_lim={13}
//", Round(Mau),
//KofZaProracunPravougaonogPreseka.εb,
//KofZaProracunPravougaonogPreseka.εa1,
//Round(Aa1_pot),
//Round(Aa2_pot),
//b, h, a1, Round(k, 3),
//beton.name, armatura.name,
//Round(Mu), Round(Nu), KofZaProracunPravougaonogPreseka_lim.k, a2);
//        }




//        public override void SetKof()
//        {
//            if (TipDim == TipDimenzionisanja.Vezano)
//                k = TabeleEC2.KofZaProracunPravougaonogPreseka.GetμSd(Mau, b, d, beton.fb / 10);
//            else k = TabeleEC2.KofZaProracunPravougaonogPreseka.GetμSd();

//            KofZaProracunPravougaonogPreseka = TabeleEC2.KofZaProracunPravougaonogPreseka.GetItem_Full(k);
//        }

//        public override void Start()
//        {
//            TipDim = h == 0 ? TipDimenzionisanja.Slobodno : TipDimenzionisanja.Vezano;
//            if (KofZaProracunPravougaonogPreseka == null) SetKof();
//            if (TipDim == TipDimenzionisanja.Slobodno) SlobodnoDim(); else VezanoDim();
//        }
//    }

//}
