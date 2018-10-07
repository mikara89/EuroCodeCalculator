using System;
using System.Collections.Generic;
using System.Linq;
using TabeleEC2;
using TabeleEC2.Model;

namespace CalculatorEC2Logic
{
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
        public double MEd { get; private set; } 

        private double _φ_ef;
        private double _ω;
        private double ei;
        private double e2;
        private double e0;

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
            var n = Forces.NEd / (ElementGeometry.b * ElementGeometry.h * Material.beton.fcd / 10);
            var A = _φ_ef == 0 ? 0.7 : 1 / (1 + 2.0 * _φ_ef);
            var B = _ω == 0 ? 1.1 : Math.Sqrt(1 + 2 * _ω);
            double C = 0.7;
            if (Forces.M_bottom != 0 && Forces.M_top != 0)
            {
                var rm = (Forces.M_bottom >= Forces.M_top) ? Forces.M_bottom / Forces.M_top: Forces.M_top / Forces.M_bottom;
                C = 1.7 - rm;
            }
            else C = 0.7;
            return 20 * A * B * C / Math.Sqrt(n);
        }

        public void Calculate()
        {

            ei = ElementGeometry.li / 400;
            e0 = new List<double>() { ElementGeometry.h / 30, 20 / 10.0 }.Max();

            if (ElementGeometry.λ > λ_lim)
            {
                //// drugi red
                var ρ = 0.01;
                var n_bal = 0.4;
                var n = Forces.NEd / (ElementGeometry.b * ElementGeometry.h * Material.beton.fcd / 10);
                var nu = 1 + (ρ * Material.armatura.fyd / Material.beton.fcd / 10);

                var Kφ = 1;
                var Kr = (nu - n) / (nu - n_bal);
                Kr = Kr >= 1 ? Kr : 1;///if Kr>=1 then use it else use 1 
                var t1 = (Material.armatura.fyd * 10 / (Material.armatura.Es * 1000));
                var t2 = (1 / (0.45 * (ElementGeometry.h - ElementGeometry.d1)));
                var Ko = (Material.armatura.fyd /** 10*/ / (Material.armatura.Es*100 /** 1000*/)) * (1 / (0.45 * (ElementGeometry.h - ElementGeometry.d1)));

                var K = Kφ * Kr * Ko;

                e2 = K * Math.Pow(ElementGeometry.li, 2) * 1 / Math.Pow(Math.PI, 2);

            }
            MEd =Forces.MEd(e2);
        }
        public void KontrolaCentPritPreseka()
        {
            var potAc = Forces.NEd / (Material.beton.fcd / 10 + 0.003 * 0.002 * Material.armatura.Es * 1000);
            IsAcOK = potAc <= ElementGeometry.b * ElementGeometry.h ? true : false;
        }
        public void ProracunArmature()
        {
            var Msd_ = MEd;
            var Vsd = Forces.NEd / (ElementGeometry.b * (ElementGeometry.h - ElementGeometry.d1) * Material.beton.fcd / 10);
            var mIsd = Msd_  / (ElementGeometry.b * Math.Pow((ElementGeometry.h - ElementGeometry.d1),2) * Material.beton.fcd / 10);
            var minAs_for_section = Get_minAs_for_section(ElementGeometry.b , ElementGeometry.h);
            var minAs_for_N = Get_minAs_for_N(Forces.NEd , Material as Material);

            var Asd = GetAsd(MEd, Forces.NEd); 

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
            return 0.15 * Forces.NEd / material.armatura.fyd;
        }



        public override string ToString()
        {
            var Red = Math.Round(e2 / 100 * Forces.NEd, 2) == 0? "I red" : "II red";
            return $@"//////Result///////
    Forces:
        NEd:         {Forces.NEd}kN
        MEd_top:     {Forces.M_top}kNm
        MEd_bottom:  {Forces.M_bottom}kNm
        M0Ed:        {Forces.M0Ed}kNm
    Material:
        Armatrua:    {Material.armatura.ToString()}
        Beton:       {Material.beton.ToString()}
    Geometry:
        b:           {ElementGeometry.b}{ElementGeometry.unit}
        h:           {ElementGeometry.h}{ElementGeometry.unit}
        d1:          {ElementGeometry.d1}{ElementGeometry.unit}
        L:           {ElementGeometry.L}{ElementGeometry.unit}
        k:           {Math.Round( ElementGeometry.k,3)}
        li:          {Math.Round(ElementGeometry.li, 2)}{ElementGeometry.unit}
        λ:           {Math.Round(ElementGeometry.λ, 2)}
        λlimit:      {Math.Round(λ_lim,2)}
    Result:
        {nameof(MEd)}: {Math.Round(MEd, 2)}kNm
        {nameof(e0)}= Max(h / 30,20mm)= {Math.Round(e0,2)}cm 
        {nameof(ei)}= li / 400= {Math.Round(ei, 2)}cm 
        {nameof(e2)}= (1/r)li^2/c= {Math.Round(e2, 2)}cm

        M01 = Min(| Mtop |,| Mbottom |) - ei*NEd= {Math.Round(Forces.M01, 2)}kNm;
        M02 = Max(| Mtop |,| Mbottom |) + ei*NEd= {Math.Round(Forces.M02, 2)}kNm;
        M0Ed= (0.6 M02 + 0.4 M01) ≥ 0.4M02= {Forces.M0Ed}kNm;
        M2= e2*NEd= {Math.Round(e2/100*Forces.NEd,2)}kNm; [{Red}]
        e0*NEd={e0/100*Forces.NEd}kNm;
        MEd= Max (M02, M0Ed + M2, M01 + 0.5*M2, e0*NEd)= {Math.Round(MEd, 2)}kNm

        As1=As2={Math.Round(GetAsd(MEd, Forces.NEd), 2)}cm2 => As={Math.Round(GetAsd(MEd, Forces.NEd), 2)*2}cm2
        min_As_for_N=0.15*NEd/fyd = {Math.Round( Get_minAs_for_N(Forces.NEd,Material as Material),2)}cm2
        min_As= 0.004* NEd = {Math.Round(Get_minAs_for_section(ElementGeometry.b, ElementGeometry.h), 2)}cm2
        max_As = 0.04* NEd = {Math.Round(Get_maxAs_for_section(ElementGeometry.b, ElementGeometry.h), 2)}cm2
        min_Ac={Math.Round(Forces.NEd / (Material.beton.fcd / 10 + 0.003 * 0.002 * Material.armatura.Es * 1000), 2)}cm2
        min_Ac<Ac=> {Math.Round(Forces.NEd / (Material.beton.fcd / 10 + 0.003 * 0.002 * Material.armatura.Es * 1000), 2)}cm2 < { Math.Round(ElementGeometry.b * ElementGeometry.h, 2)}cm2 => {IsAcOK}
";
        }
        public void Dispose()
        {
            GC.Collect();
        }
    }
}
