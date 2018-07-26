using System;
using System.Collections.Generic;
using System.Text;
using TabeleEC2;
using TabeleEC2.Model;

namespace CalculatorEC2Logic
{
    public class SavijanjeTPresekaEC2
    {
        public double b_w { get; }
        public double b_eff { get; }
        public double h { get; internal set; }
        public double h_f { get; }
        public double d { get { return h - d1; } }
        public double d1 { get; }
        public double d2 { get; }
        public double Msd { get; internal set; }
        public BetonModelEC beton { get; }
        public ReinforcementTypeModelEC armatura { get; }
        public double Nsd { get; internal set; }
        public double Msds { get; internal set; }
        public TipDimenzionisanja TipDim { get; private set; }
        public double μSd { get; private set; }
        public KofZaProracunPravougaonogPresekaModelEC KofZaProracunPravougaonogPreseka { get; private set; }
        public double ρ_max => 0.04;
        //public double μ_lim => 0.252;
        public KofZaProracunPravougaonogPresekaModelEC KofZaProracunPravougaonogPreseka_lim
        {
            get { return TabeleEC2.KofZaProracunPravougaonogPresekaEC.GetLimitKofZaProracunPravougaonogPresekaEC(beton); }
        }

        public double b_i { get; private set; }
        public double Mrd_limit { get; private set; }
        public double As1_pot { get; private set; }
        public double As2_pot { get; private set; }
        public double x { get; private set; }

        public SavijanjeTPresekaEC2(
            double b_w, double b_eff,
            double h, double h_f, 
            double d1, double Mg,
            double Mq, BetonModelEC beton,
            ReinforcementTypeModelEC armatura,
            double d2 = 0, double Ng = 0,
            double Nq = 0,
            KofZaProracunPravougaonogPresekaModelEC kof = null)
        {
            this.b_w = b_w;
            this.b_eff = b_eff;
            this.h = h;
            this.h_f = h_f;
            this.d1 = d1;
            this.d2 = d2;
            this.Msd =1.35* Mg + 1.5* Mq;
            this.Nsd = 1.35 * Ng + 1.5 * Nq;
            this.Msds = Msd + Nsd * (h / 100 / 2 - d1 / 100);
            if (kof != null) { KofZaProracunPravougaonogPreseka = kof; μSd = kof.μRd; }
        }
        public SavijanjeTPresekaEC2(
            double b_w, double b_eff,
            double h, double h_f,
            double d1, double Msd, BetonModelEC beton,
            ReinforcementTypeModelEC armatura,
            double d2 = 0, double Nsd = 0,
            KofZaProracunPravougaonogPresekaModelEC kof=null) 
        {
            this.b_w = b_w;
            this.b_eff = b_eff;
            this.h = h;
            this.h_f = h_f;
            this.d1 = d1;
            this.d2 = d2;
            this.Msd = Msd;
            this.beton = beton;
            this.armatura = armatura;
            this.Nsd = Nsd;
            this.Msds = Msd + Nsd * (h / 100 / 2 - d1 / 100);
            if (kof != null) { KofZaProracunPravougaonogPreseka = kof; μSd = kof.μRd; }
        }

        private void SetKof()
        {
            if (TipDim == TipDimenzionisanja.Vezano)
                μSd = TabeleEC2.KofZaProracunPravougaonogPresekaEC.GetμSd(Msds, b_eff, d, beton.fcd / 10);
            else μSd = TabeleEC2.KofZaProracunPravougaonogPresekaEC.GetμSd();

            KofZaProracunPravougaonogPreseka = TabeleEC2.KofZaProracunPravougaonogPresekaEC.GetItem_Full(μSd);
        }

        private void Start()
        {
            TipDim = h == 0 ? TipDimenzionisanja.Slobodno : TipDimenzionisanja.Vezano;
            if (KofZaProracunPravougaonogPreseka == null) SetKof();
            if (TipDim == TipDimenzionisanja.Slobodno) SlobodnoDim();
            else {
                x = KofZaProracunPravougaonogPreseka.ξ * d;
                if (μSd < KofZaProracunPravougaonogPreseka_lim.μRd && x <= h_f)
                    b_i = b_eff;
                else CistTPresek(); ///neutralna osa u rebru
                VezanoDim();
                    };

        }

        private void CistTPresek()
        {
            var z = d - h_f / 2;

            KofZaProracunPravougaonogPreseka = new KofZaProracunPravougaonogPresekaModelEC();
            KofZaProracunPravougaonogPreseka.SetByξ(h_f / d);
            x = KofZaProracunPravougaonogPreseka.ξ * d;
            var Start_e = KofZaProracunPravougaonogPreseka;
            KofZaProracunPravougaonogPresekaModelEC Next_e = KofZaProracunPravougaonogPreseka;
            var lamda_b = 1 - (Start_e.αv / Next_e.αv) * (1 - (h_f / (Next_e.ξ * d)) * (1 - (b_eff / b_w)));
            do
            {


            } while (Start_e.ξ!= Next_e.ξ);
        }

        /// <summary>
        /// If TipDim == TipDimenzionisanja.Slobodno
        /// </summary>
        private void SlobodnoDim()
        {
            double d_r = 0;
            double h_r = 0;
            d_r = Math.Sqrt(Msds * 100 / (b_eff * KofZaProracunPravougaonogPreseka.μRd * beton.fcd / 10));
            h_r = d_r + this.d1;
            if (h_r % 5 != 0) h = h_r - (h_r % 5) + 5;
            else h = h_r;
            if (μSd > KofZaProracunPravougaonogPreseka_lim.μRd) DvostrukoArmiranje(); else Armiranje();
        }
        /// <summary>
        /// If TipDim == TipDimenzionisanja.Vezano
        /// </summary>
        private void VezanoDim()
        {
            if (μSd > KofZaProracunPravougaonogPreseka_lim.μRd) DvostrukoArmiranje(); else Armiranje();

        }

        private void Armiranje()
        {
            As1_pot = KofZaProracunPravougaonogPreseka.ω * b_i * d * beton.fcd / (armatura.fyd * 10) - (Nsd / armatura.fyd);
            if (As1_pot / b_i / h > ρ_max) throw new Exception("ρ_max prekoraceno! Povećajte poprecni presek");
        }

        private void DvostrukoArmiranje()
        {
            Mrd_limit = (KofZaProracunPravougaonogPreseka_lim.μRd * b_i * Math.Pow(d, 2) * beton.fcd / 10) / 100;
            As1_pot = Mrd_limit * 100 / (KofZaProracunPravougaonogPreseka_lim.ζ * d * armatura.fyd) - (Nsd / armatura.fyd) + (Msds * 100 - Mrd_limit * 100) / ((d - d2) * armatura.fyd);
            As2_pot = (Msds * 100 - Mrd_limit * 100) / ((d - d2) * armatura.fyd);
            if ((As1_pot + As2_pot) / b_i / h > ρ_max) throw new Exception("ρ_max prekoraceno! Povećajte poprecni presek");
        }


    }
}
