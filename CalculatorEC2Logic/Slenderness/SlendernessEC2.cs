using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculatorEC2Logic
{
    public class SlendernessEC2 : ISlenderness, IDisposable
    { 
        public bool IsAcOK { get;  private set; }
        public IElementGeometrySlenderness ElementGeometry { get;}
        public IForcesSlenderness Forces { get;}
        public IMaterial Material {get;}
        public double λ_lim { get { return Lamda_lim(); } }
        public double MEd { get; private set; }

        private double ei;
        private double e2;
        private double e0;

        /// <summary>
        /// efektivni koeficient tecenja
        /// </summary>
        public double φ_ef { get; set; }
        /// <summary>
        /// Mehanički koficijen armiranja
        /// </summary>
        public double ω { get; set; }
        /// <summary>
        /// Potrebna armatura u obe zone
        /// </summary>
        public double As { get; private set; }

        /// <summary>
        /// Define vitkost instanc
        /// </summary>
        /// <param name="elementGeomety">Geometry parametars</param>
        /// <param name="forces">Forces parametars</param>
        /// <param name="material">Material parametars</param>
        public SlendernessEC2(
            IElementGeometrySlenderness elementGeomety,
            IForcesSlenderness forces,
            IMaterial material)
        {
            ElementGeometry = elementGeomety;
            Forces = forces;
            Material = material;
        }
        /// <summary>
        /// Calculate λ_limit
        /// </summary>
        /// <returns>value as double</returns>
        public double Lamda_lim()
        {
            var n = Forces.NEd / (ElementGeometry.b * ElementGeometry.h * Material.beton.fcd / 10);
            var A = φ_ef == 0 ? 0.7 : 1 / (1 + 2.0 * φ_ef);
            var B = ω == 0 ? 1.1 : Math.Sqrt(1 + 2 * ω);
            double C = 0.7;
            if (Forces.M_bottom != 0 && Forces.M_top != 0)
            {
                var rm = (Forces.M01 >= Forces.M02) ? Forces.M01 / Forces.M02 : Forces.M02 / Forces.M01;
                C = 1.7 - rm;
            }
            else C = 0.7;
            return 20 * A * B * C / Math.Sqrt(n);
        }

        /// <summary>
        /// Calculating eccentricity cose by II order
        /// </summary>
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
                var Ko = (Material.armatura.fyd / (Material.armatura.Es*100)) * (1 / (0.45 * (ElementGeometry.h - ElementGeometry.d1)));

                var K = Kφ * Kr * Ko;

                e2 = K * Math.Pow(ElementGeometry.li, 2) * 1 / Math.Pow(Math.PI, 2);

            }
            MEd =Forces.MEd(e2);
        }
        /// <summary>
        /// Chack if section setified
        /// </summary>
        public void KontrolaCentPritPreseka()
        {
            var potAc = Forces.NEd / (Material.beton.fcd / 10 + 0.003 * 0.002 * Material.armatura.Es * 1000);
            IsAcOK = potAc <= ElementGeometry.b * ElementGeometry.h ? true : false;
        }
        /// <summary>
        /// Calculating required reinforcement
        /// </summary>
        public void ProracunArmature()
        {
            var Msd_ =Math.Abs(MEd);
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
            var sym = new SymmetricalReinfByClassicMethod(Material, new ElementGeometry()
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
        NEd:         {Forces.NEd:F2}kN
        MEd_top:     {Forces.M_top:F2}kNm
        MEd_bottom:  {Forces.M_bottom:F2}kNm
        M0Ed:        {Forces.M0Ed:F2}kNm
        MEd:         {MEd:F2}kNm
    Material:
        Armatrua:    {Material.armatura.ToString()}
        Beton:       {Material.beton.ToString()}
    Geometry:
        b:           {ElementGeometry.b}{ElementGeometry.unit}
        h:           {ElementGeometry.h}{ElementGeometry.unit}
        d1:          {ElementGeometry.d1}{ElementGeometry.unit}
        L:           {ElementGeometry.L}{ElementGeometry.unit}
        k:           {ElementGeometry.k:F2}
        li:          {ElementGeometry.li:F2}{ElementGeometry.unit}
        λ:           {ElementGeometry.λ:F2}
        λlimit:      {λ_lim:F2}
    Result:
        {nameof(MEd)}: {MEd:F2}kNm
        {nameof(e0)}= Max(h / 30,20mm)= {e0:F2}cm 
        {nameof(ei)}= li / 400= {ei:F2}cm 
        {nameof(e2)}= (1/r)li^2/c= {e2:F2}cm

        M01 = Min(| Mtop |,| Mbottom |) - ei*NEd= {Forces.M01:F2}kNm;
        M02 = Max(| Mtop |,| Mbottom |) + ei*NEd= {Forces.M02:F2}kNm;
        M0Ed= (0.6 M02 + 0.4 M01) ≥ 0.4*M02= {Forces.M0Ed:F2}kNm;
        M2= e2*NEd= {e2/100*Forces.NEd:F2}kNm; [{Red}]
        e0*NEd={e0/100*Forces.NEd}kNm;
        MEd= Max (M02, M0Ed + M2, M01 + 0.5*M2, e0*NEd);
        MEd= {MEd:F2}kNm
        As1=As2={GetAsd(MEd, Forces.NEd):F2}cm2 => As={GetAsd(MEd, Forces.NEd)*2:F2}cm2
        min_As_for_N=0.15*NEd/fyd = { Get_minAs_for_N(Forces.NEd,Material as Material):F2}cm2
        min_As= 0.004* NEd = {Get_minAs_for_section(ElementGeometry.b, ElementGeometry.h):F2}cm2
        max_As = 0.04* NEd = {Get_maxAs_for_section(ElementGeometry.b, ElementGeometry.h):F2}cm2
        min_Ac={Forces.NEd / (Material.beton.fcd / 10 + 0.003 * 0.002 * Material.armatura.Es * 1000):F2}cm2
        min_Ac<Ac=> {Forces.NEd / (Material.beton.fcd / 10 + 0.003 * 0.002 * Material.armatura.Es * 1000):F2}cm2 < { ElementGeometry.b * ElementGeometry.h:F2}cm2 => {IsAcOK}
";
        }
        public void Dispose()
        {
            GC.Collect();
        }
    }
}
