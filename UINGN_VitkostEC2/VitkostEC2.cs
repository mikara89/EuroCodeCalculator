using System;
using System.Collections.Generic;
using System.Linq;
using TabeleEC2;
using TabeleEC2.Model;

namespace CalculatorEC2Logic
{
    public class VitkostEC2 : IDisposable
    {
        private double Es = 200000;//200GPa 200000
        /// <summary>
        /// N od stalnog i sopstvenog[kN]
        /// </summary>
        public double Ng { get; private set; }
        /// <summary>
        /// N od korisnog/promenjivog[kN]
        /// </summary>
        public double Nq { get; private set; }
        /// <summary>
        /// M od stalnog i sopstvenog[kNm]
        /// </summary>
        public double Mg { get; private set; }
        /// <summary>
        /// M od korisnog/promenjivog[kNm]
        /// </summary>
        public double Mq { get; private set; }
        /// <summary>
        /// ukupno N [kN]
        /// </summary>
        public double Nsd { get { return 1.35 * Ng + 1.5 * Nq; } }
        /// <summary>
        /// ukupno M [kNm]
        /// </summary>
        public double Msd { get { return 1.35 * Mg + 1.5 * Mq; } }
        public double Msd_II { get; private set; }
        public double Msd_I { get; private set; }
        public double Nsd_II { get; private set; }
        public double L { get; private set; }
        public double bx { get; private set; }
        public double by { get; private set; }
        public double λ { get; private set; }
        public double λ_lim { get; private set; }
        public double li { get; private set; }
        public double k { get; private set; }
        public double fcd { get { return beton.fcd / 10; } }
        public double fyd { get { return armatura.fyd; } }
        public BetonModelEC beton { get; private set; }
        public ReinforcementTypeModelEC armatura { get; private set; }
        public double usv_As { get; private set; }
        /// <summary>
        /// zastitni sloj + Øuzengije+ 1/2 Ø sipke [cm]
        /// </summary>
        public double d1 { get; private set; }
        public double d
        {
            get
            {
                return bx - d1;
            }
        }
        public bool IsAcOK { get; private set; }
        public VitkostEC2(Izvijanja izvijanje, double Ng_sila,
            double Nq_sila, double L_visina, double b_sirina_x, double b_sirina_y,
            BetonModelEC beton, ReinforcementTypeModelEC armatura, double d1, double Mg_sila = 0, double Mq_sila = 0)
        {

            this.beton = beton;
            this.armatura = armatura;
            Ng = Ng_sila;
            Nq = Nq_sila;
            Mg = Mg_sila;
            Mq = Mq_sila;
            L = L_visina;
            bx = b_sirina_x;
            by = b_sirina_y;
            this.d1 = d1;
            k = OjleroviSlucajeviIzvijanja.GetK(izvijanje);
            li = k * L;
            λ = li / (by / 2 / Math.Sqrt(3));
            λ_lim = Lamda_lim();



            Calculate();
            ProracunArmature();
            KontrolaCentPritPreseka();
        }

        public double Lamda_lim()
        {
            var n = Nsd / (bx * by * fcd);
            var A = 0.7; /*implementirati GetA();*/
            var B = 1.1; /*implementirati GetB();*/
            var C = 0.7; /*implementirati GetC();*/
            return 20 * A * B * C / Math.Sqrt(n);
        }

        ///1 geometriska inperfekcija
        public void Calculate()
        {
            var ϴ0 = 1 / 200;
            var αh = 2 / Math.Sqrt(L / 100);
            var αm = 1;
            var list = new List<double>() { αh * li / 400, by / 30, 20 / 10.0 };
            var ei = list.Max();

            if (λ <= λ_lim)
            {
                Msd_I = ei * Nsd / 100;
            }
            else
            {
                //// drugi red
                var ρ = 0.01;
                var n_bal = 0.4;
                var n = Nsd / (bx * by * fcd);
                var nu = 1 + (ρ * fyd / fcd);

                var Kφ = 1;
                var Kr = (nu - n) / (nu - n_bal);
                var Ko = (fyd * 10 / Es) * (1 / (0.45 * d));

                var K = Kφ * Kr * Ko;

                var e2 = K * Math.Pow(li, 2) * 1 / Math.Pow(Math.PI, 2);

                var e0 = Msd / Nsd;

                var e_tot = e0 + e2 + ei;

                Msd_II = Nsd * e_tot / 100;
                Nsd_II = Nsd;
            }

        }
        private void KontrolaCentPritPreseka()
        {
            var potAc = Nsd_II / (fcd + 0.003 * 0.002 * Es);
            IsAcOK = potAc <= bx * by ? true : false;
        }
        private void ProracunArmature()
        {
            var Msd_ = Msd_I == 0 ? Msd_II : Msd_I;

            var minAs_prema_Preseku = 0.003 * bx * by;
            var minAs_prema_Nsd = 0.15 * Nsd / fyd;


            //var μSd = Msd_ * 100 / (bx * Math.Pow(d, 2) * fcd);

            var μSd = TabeleEC2.KofZaProracunPravougaonogPresekaEC.GetμSd(Msd_, bx, d, fcd);

            var ω = TabeleEC2.KofZaProracunPravougaonogPresekaEC.GetItem_Full(μSd).ω;

            var Asd = ω * bx * d * fcd / fyd;

            var A_list = new List<double>() { minAs_prema_Preseku, minAs_prema_Nsd, Asd };
            usv_As = A_list.Max();
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }

    public interface Vitkost
    {
        void Calculate();
        void KontrolaCentPritPreseka();
        void ProracunArmature();

    }
    public class VitkostEC2_V2 : Vitkost, IDisposable
    {
        private event Action OnChangeSomethingRecalculate;
        public bool IsAcOK { get; private set; }
        public IElementGeometrySlenderness ElementGeometry { get; private set; }
        public IForcesSlenderness Forces { get; private set; }
        public IMaterial Material { get; private set; }
        public double λ_lim { get { return Lamda_lim(); } }
        //public double Msd_I { get; private set; }
        public double MEd { get; private set; } 
       // public double Msd_II { get; private set; }

        private double _φ_ef;
        private double _ω;
        private double ei;
        private double e2;
        private double e0;
        private double e_tot;

        /// <summary>
        /// efektivni koeficient tecenja
        /// </summary>
        public double φ_ef { get => _φ_ef; set { _φ_ef = value; OnChangeSomethingRecalculate?.Invoke(); } }
        /// <summary>
        /// Mehanički koficijen armiranja
        /// </summary>
        public double ω { get => _ω; set { _ω = value; OnChangeSomethingRecalculate?.Invoke(); } }

        public double As { get; private set; }

        public VitkostEC2_V2(IElementGeometrySlenderness elementGeomety, IForcesSlenderness forces, IMaterial material)
        {
            ElementGeometry = elementGeomety;
            Forces = forces;
            Material = material;

            if (OnChangeSomethingRecalculate == null)
                OnChangeSomethingRecalculate += () => { Calculate(); };
        }

        public double Lamda_lim()
        {
            var n = Forces.N / (ElementGeometry.b * ElementGeometry.h * Material.beton.fcd / 10);
            var A = _φ_ef == 0 ? 0.7 : 1 / (1 + 2.0 * _φ_ef);
            var B = _ω == 0 ? 1.1 : Math.Sqrt(1 + 2 * _ω);
            double C = 0.7;
            if (Forces.M_bottom != 0 && Forces.M_top != 0)
            {
                var rm = (Forces.M_bottom >= Forces.M_top) ? Forces.M_bottom / Forces.M_top : Forces.M_top / Forces.M_bottom;
                C = 1.7 - rm;
            }
            else C = 0.7;
            return 20 * A * B * C / Math.Sqrt(n);
        }

        ///1 geometriska inperfekcija
        //public void Calculate()
        //{
        //    double ϴ0 = 1 / 200;
        //    double αh = 2 / Math.Sqrt(ElementGeometry.L / 100);
        //    var αm = 1;
        //    var list = new List<double>() { αh * ElementGeometry.li / 400, ElementGeometry.h / 30, 20 / 10.0 };
        //    var ei = list.Max();

        //    if (ElementGeometry.λ <= λ_lim)
        //    {
        //        Msd_I = ei * Forces.N / 100 + Forces.M0Ed;
        //    }
        //    else
        //    {
        //        //// drugi red
        //        var ρ = 0.01;
        //        var n_bal = 0.4;
        //        var n = Forces.N / (ElementGeometry.b * ElementGeometry.h * Material.beton.fcd / 10);
        //        var nu = 1 + (ρ * Material.armatura.fyd / Material.beton.fcd / 10);

        //        var Kφ = 1;
        //        var Kr = (nu - n) / (nu - n_bal);

        //        var t1 = (Material.armatura.fyd * 10 / (Material.armatura.Es * 1000));
        //        var t2 = (1 / (0.45 * (ElementGeometry.h - ElementGeometry.d1)));
        //        var Ko = (Material.armatura.fyd * 10 / (Material.armatura.Es * 1000)) * (1 / (0.45 * (ElementGeometry.h - ElementGeometry.d1)));

        //        var K = Kφ * Kr * Ko;

        //        var e2 = K * Math.Pow(ElementGeometry.li, 2) * 1 / Math.Pow(Math.PI, 2);

        //        var e0 = Forces.M0Ed * 100 / Forces.N;

        //        var e_tot = e0 + e2 + ei;

        //        var Msd2 = Forces.N * e_tot / 100;

        //        Msd_II = Forces.M0Ed + Msd2;
        ////    }

        //}

        public void Calculate()
        {
            double ϴ0 = 1.0 / 200.0;
            double αh = 2 / Math.Sqrt(ElementGeometry.L/100);
            var αm = 1;
            double ϴi = ϴ0 * αh * αm;
            var list = new List<double>() { (ϴi*ElementGeometry.li)/2, αh * ElementGeometry.li / 400, ElementGeometry.h / 30, 20 / 10.0 };
            ei = list.Max();

            if (ElementGeometry.λ > λ_lim)
            {
                //// drugi red
                var ρ = 0.01;
                var n_bal = 0.4;
                var n = Forces.N / (ElementGeometry.b * ElementGeometry.h * Material.beton.fcd / 10);
                var nu = 1 + (ρ * Material.armatura.fyd / Material.beton.fcd / 10);

                var Kφ = 1;
                var Kr = (nu - n) / (nu - n_bal);

                var t1 = (Material.armatura.fyd * 10 / (Material.armatura.Es * 1000));
                var t2 = (1 / (0.45 * (ElementGeometry.h - ElementGeometry.d1)));
                var Ko = (Material.armatura.fyd * 10 / (Material.armatura.Es * 1000)) * (1 / (0.45 * (ElementGeometry.h - ElementGeometry.d1)));

                var K = Kφ * Kr * Ko;

                e2 = K * Math.Pow(ElementGeometry.li, 2) * 1 / Math.Pow(Math.PI, 2);

                e0 = Forces.M0Ed * 100 / Forces.N;
            }
            e_tot = e0 + e2 + ei;

            MEd = Forces.N * e_tot / 100;
            
        }
            public void KontrolaCentPritPreseka()
        {
            var potAc = Forces.N / (Material.beton.fcd / 10 + 0.003 * 0.002 * Material.armatura.Es * 1000);
            IsAcOK = potAc <= ElementGeometry.b * ElementGeometry.h ? true : false;
        }
        public void ProracunArmature()
        {
            var Msd_ = MEd;
            var Vsd = Forces.N / (ElementGeometry.b * (ElementGeometry.h - ElementGeometry.d1) * Material.beton.fcd / 10);
            var mIsd = Msd_  / (ElementGeometry.b * Math.Pow((ElementGeometry.h - ElementGeometry.d1),2) * Material.beton.fcd / 10);
            var minAs_for_section = Get_minAs_for_section(ElementGeometry.b , ElementGeometry.h);
            var minAs_for_N = Get_minAs_for_N(Forces.N , Material as Material);

            var Asd = GetAsd(MEd, Forces.N); 

            var A_list = new List<double>() { minAs_for_section, minAs_for_N, Asd };
            As = A_list.Max();
            KontrolaCentPritPreseka();
        }
        private double GetAsd(double MEd, double NEd)
        {
            var sym = new SymmetricalReinfByClassicMethod(Material, new ElementGeomety()
            {
                b = ElementGeometry.b,
                h = ElementGeometry.h,
                d1 = ElementGeometry.d1,
                unit = ElementGeometry.unit
            }
            );
            var w = sym.Get_ω2(MEd, NEd);
            return ElementGeometry.b * (ElementGeometry.h - ElementGeometry.d1) * w * Material.beton.fcd / (Material.armatura.fyd * 10);
        }
        private double Get_minAs_for_section(double b, double h)
        {
            return 0.004 * ElementGeometry.b * ElementGeometry.h;
        }
        private double Get_maxAs_for_section(double b, double h)
        {
            return 0.04 * ElementGeometry.b * ElementGeometry.h;
        }
        private double Get_minAs_for_N(double NEd, Material material)  
        {
            return 0.15 * Forces.N / material.armatura.fyd;
        }



        public override string ToString()
        {
            return $@"//////Result///////
    Forces:
        NEd: {Forces.N}kN
        MEd_top: {Forces.M_top}kNm
        MEd_bottom: {Forces.M_bottom}kNm
        M0Ed: {Forces.M0Ed}kNm
    Material:
        Armatrua: {Material.armatura.ToString()}
        Beton: {Material.beton.ToString()}
    Geometry:
        b: {ElementGeometry.b}{ElementGeometry.unit}
        h: {ElementGeometry.h}{ElementGeometry.unit}
        d1: {ElementGeometry.d1}{ElementGeometry.unit}
        L: {ElementGeometry.L}{ElementGeometry.unit}
        li: {Math.Round(ElementGeometry.li, 2)}{ElementGeometry.unit}
        λ: {Math.Round(ElementGeometry.λ, 2)}
        λlimit: {Math.Round(λ_lim,2)}
    Result:
        {nameof(MEd)}: {Math.Round(MEd, 2)}kNm
        {nameof(e_tot)}={nameof(e0)}+{nameof(e2)}+{nameof(ei)} 
        {nameof(e_tot)}={Math.Round(e0, 2)}+{Math.Round(e2, 2)}+{Math.Round(ei, 2)}= {Math.Round(e_tot, 2)}cm 
        MEd= NEd*e_tot[m]= {Forces.N}*{Math.Round(e_tot, 2) / 100}= {Math.Round(MEd, 2)}kNm
        As1=As2={Math.Round(GetAsd(MEd, Forces.N), 2)}cm2 => As={Math.Round(GetAsd(MEd, Forces.N), 2)*2}cm2
        min_As = {Math.Round(Get_minAs_for_section(ElementGeometry.b, ElementGeometry.h), 2)}cm2 ; max_As = {Math.Round(Get_maxAs_for_section(ElementGeometry.b, ElementGeometry.h), 2)}cm2
        min_Ac={Math.Round(Forces.N / (Material.beton.fcd / 10 + 0.003 * 0.002 * Material.armatura.Es * 1000), 2)}cm
        min_Ac<Ac=> {Math.Round(Forces.N / (Material.beton.fcd / 10 + 0.003 * 0.002 * Material.armatura.Es * 1000), 2)}cm < { Math.Round(ElementGeometry.b * ElementGeometry.h, 2)}cm => {IsAcOK}
";
        }
        public void Dispose()
        {
            GC.Collect();
        }
    }







    public interface IElementGeometrySlenderness: IElementGeometry
    {
        Izvijanja izvijanje { get; set; }
        double L { get; set; }
        double li { get; }
        double λ { get; }
        double Ix { get; }
        double Ac { get; }
        double ic { get; }
    }

    public enum UnitDimesionType
    {
        cm,
        m,
        mm,
    }
    public interface IElementGeometry
    {
        UnitDimesionType unit { get; set; }
        double b { get; set; }
        double h { get; set; }
        double d1 { get; set; }
    }
    public class ElementGeomety : IElementGeometry
    {
        public string GetUnits()
        {
            switch (unit)
            {
                case UnitDimesionType.cm:
                    return nameof(UnitDimesionType.cm);
                case UnitDimesionType.m:
                    return nameof(UnitDimesionType.m);
                case UnitDimesionType.mm:
                    return nameof(UnitDimesionType.mm);
                default:
                    throw new ArgumentOutOfRangeException("Error in setting units");
            }
        }
        public double b { get; set; }
        public double h { get; set; }
        public double d1 { get; set; }
        public double d { get => h - d1; }
        public UnitDimesionType unit { get; set; } = UnitDimesionType.cm;
    }
    public class ElementGeometySlenderness : IElementGeometrySlenderness
    {
        public double b { get; set; }
        public double h { get; set; }
        public Izvijanja izvijanje { get; set; }
        public double d1 { get; set; }
        public double L { get; set; }
        public double Ix { get { return b * Math.Pow(h, 3) / 12; } }
        public double Ac { get { return b * h; } }
        public double ic { get { return Math.Sqrt(Ix / Ac); } }
        public double li => OjleroviSlucajeviIzvijanja.GetK(izvijanje) * L;
        public double λ { get { return li / ic; } }
        public UnitDimesionType unit { get; set; } = UnitDimesionType.cm;
    }
    public interface IForcesSlenderness
    {
        double M_top { get; set; }
        double M_bottom { get; set; }
        double M0Ed { get; }
        double N { get; set; }
    }
    public class ForcesSlenderness : IForcesSlenderness
    {
        public double M_top { get; set; }
        public double M_bottom { get; set; }
        public double M0Ed
        {
            get
            {
                if (M_top >= M_bottom)
                {
                    var result = 0.6 * Math.Abs(M_top) + 0.4 * Math.Abs(M_bottom);
                    return result >= 0.4 * Math.Abs(M_top) ? result : 0.4 * Math.Abs(M_top);
                }
                else
                {
                    var result = 0.6 * Math.Abs(M_bottom) + 0.4 * Math.Abs(M_top);
                    return result >= 0.4 * Math.Abs(M_bottom) ? result : 0.4 * Math.Abs(M_bottom);
                }

            }
        }
        public double N { get; set; }
    }
    public interface IMaterial
    {
        BetonModelEC beton { get; set; } 
        ReinforcementTypeModelEC armatura { get; set; } 
    }
    public class Material : IMaterial
    {
        public BetonModelEC beton { get; set; }
        public ReinforcementTypeModelEC armatura { get; set; } 
    }
}
