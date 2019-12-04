using CalculatorEC2Logic.Transversal;
using System;
using System.Collections.Generic;
using System.Linq;
using CalcModels;

namespace CalculatorEC2Logic
{
    public class TransversalCalcEC2 : IDisposable, ITransversalCalc 
    {
        public IElementGeometryTransversal Geometry { get; private set; }
        public IForcesTransversal Forces { get; private set; }
        public IMaterial Material { get; private set; }
        public int m;
        public List<string> Errors = new List<string>();
        public List<int> List_m = new List<int>() { 2 };
        public double s { get; set; }
        public double s_max { get; private set; }
        public double s_min { get; private set; }

        public double sp { get; set; }
        public double sp_max { get; private set; }
        public double sp_min { get; private set; }
        private double Θ = 45 * Math.PI / 180;

        public double As1 { get=>Geometry.As1.cm2_total; }
        public ReinforcementModelEC Asw_Model { get; set; }
        public double Asw { get { return Asw_Model != null ? Asw_Model.cm2_total : 0; } }
        public double Asw_min { get; private set; }
        public double As_add { get; private set; }

        public double Vrd_max2 { get; private set; }


        /// <summary>
        /// Konačna nosivost preseka sa usvojenom armaturom
        /// </summary>
        public double Vrd { get; set; }
        public double IskoriscenostArmature { get; private set; }
        public double IskoriscenostBetona { get; private set; }

        private double σcp;
        /// <summary>
        /// Trans. sila koju presek moze da prihvati bez armature
        /// </summary>
        public double Vrd_c { get; set; }
        /// <summary>
        /// Max trans. sila koju presek moze da prihvati
        /// </summary>
        public double Vrd_max { get; private set; }
        /// <summary>
        /// Da li maga gresaka u proracunu
        /// </summary>
        public bool IsDimOk { get; private set; }

        public double ρ1 { get; private set; }
        public double k { get; private set; }
        public List<double> List_s { get; private set; }
        public double Vrd_s { get; private set; }

        public TransversalCalcEC2(
            IElementGeometryTransversal geometry,
            IForcesTransversal forces,
            IMaterial material          
            )
        {
            this.Geometry = geometry;
            this.Forces = forces;
            this.Material = material;

            InitValidations(geometry, material);

            Calc_Vrd_c();

            GetS();
            GetSp();
            GetListOfM();

            if (forces.Ved <= Vrd_c && Errors.Count() == 0)
            {
                minArmatura();
                CalAdditionalTransverseReinforcement();
            }  
        }

        private void InitValidations(
            IElementGeometryTransversal geometry,
            IMaterial material)
        {
            if (geometry.b <= 0)
                throw new Exception("b must be greater 0");
            if (geometry.h <= 0)
                throw new Exception("h must be greater 0");
            if (geometry.d1 <= 0)
                throw new Exception("d1 must be greater 0");
            if (2 * geometry.d1 >= geometry.h)
                throw new Exception("2 x d1 must be smaller then h");
            if (material.beton == null)
                throw new Exception("Beton not defined!");
            if (material.armatura == null)
                throw new Exception("Armatura not defined!");
            if (geometry.As1 == null)
                throw new Exception("Longitudinal reinforcement not defined!");

            ρ1 = As1 / geometry.b / geometry.d;

            if (ρ1 > 0.02)
                Errors.Add("ρ1 is " + Math.Round(ρ1, 3) + " > 0.02. \nLower As1 or make b or h greater!");

            k = 1.0 + Math.Sqrt(200 / (geometry.d * 10));
            if (k > 2.0)
                Errors.Add("Conditions for the minimum cross-section height is not met! \nh-d1 have to be geater then 20cm");

            σcp = Forces.Ned / (geometry.b * geometry.h);
            if (σcp > 2 * material.beton.fcd)
                Errors.Add("Conditions for minimal cross-section of concrete are not satisfied");
        }

        private void Calc_Vrd_c()
        {

            var γc = 1.5;
            var Crd_c = 0.18 / γc;

            var k1 = 0.15;

            Vrd_c = (Crd_c * k * Math.Pow((100 * ρ1 * Material.beton.fck), 1.0 / 3.0) + (k1 * σcp)) * Geometry.b * 10 * Geometry.d * 10 / 1000;
            
            var ν_min = 0.035 * Math.Pow(k, 3.0 / 2.0) * Math.Pow(Material.beton.fck, 1.0 / 2.0);
            var lim_conc = (ν_min + k1 * σcp) * Geometry.b * 10 * Geometry.d * 10 / 1000;
            if (Vrd_c < lim_conc)
                Errors.Add("Conditions for the maximum cross-section of concrete is not satisfied, Vrd_c < ((ν_min + k1* σcp) * b*10 * d*10/1000)=" + Vrd_c.Round() + "<" + lim_conc.Round());

            var ν = 0.6 * (1.0 - (Material.beton.fck / 250.0));///fck u MPa
            Vrd_max = 0.5 * ν * Material.beton.fcd / 10 * Geometry.b * Geometry.d;
            ///ako je false mora da povecamo presek ili klasu betona
            if (Forces.Ved > Vrd_max)
                Errors.Add("Conditions for the maximum Ved is not satisfied. \nMake cross-section bigger or change class of concrete");
        }

        public void CalculateArmature(int m, double s, ReinforcementModelEC Asw_Model)
        {
            if (s == 0) this.s = s_max;
            else this.s = s;
            if (this.s > s_max || this.s < s_min)
                throw new Exception("The distance between the stirring is too little or too large!");
            this.m = m;
            if (!List_m.Any(n => n == m))
                throw new Exception("Type of stirrup m=" + m + " is not allowed");
            if (Forces.Ved <= Vrd_c)
            {
                minArmatura();
                CalAdditionalTransverseReinforcement();
                return;
            }
            if (Forces.Ved > Vrd_c)
            {
                Armatura(m, s, Asw_Model);
                CalAdditionalTransverseReinforcement();
                return;
            }

        }
        private void Armatura(int m, double s, ReinforcementModelEC Asw_Model)
        {
            var a_min = 22.0;
            var a_max = 45.0;
            var z = 0.9 * Geometry.d;
            Θ = 0.5* Math.Asin(Forces.Ved * 1000 / (0.20 * (1.0 -Material.beton.fck / 250.0) * Material.beton.fck *Geometry.b * 10 * z * 10));

            if (Θ.Angle() < 22.0)
                Θ = a_min.Radians();
            else if (Double.IsNaN(Θ) || Θ.Angle() > 45)
                Θ = a_max.Radians();

            this.Asw_Model = Asw_Model;
            this.m = m;
            this.s = s;
            var cot = (1 / Math.Tan(Θ));

            Vrd_s = (Asw / s) * z * Material.armatura.fyd * m * (1 / Math.Tan(Θ));

            var ν = 0.6 * (1.0 - (Material.beton.fck / 250.0));///fck u MPa
            var v1 = ν;
            var αcw = 1;
            Vrd_max2 = (αcw * Geometry.b * z * v1 * Material.beton.fcd / 10) / (Math.Tan(Θ) + (1 / Math.Tan(Θ)));
            Vrd = (new List<double>() { Vrd_s, Vrd_max2 }).Min();
            IskoriscenostArmature =Math.Round(Forces.Ved / Vrd_s*100,2);
            IskoriscenostBetona = Math.Round(Forces.Ved / Vrd_max2*100,2);
        }

        private void CalAdditionalTransverseReinforcement()
        {
            var alfa = 90 * Math.PI / 180;
            As_add = (0.5 * Forces.Ved * ((1 / Math.Tan(Θ)) - (1 / Math.Tan(alfa)))) / Material.armatura.fyd;
        }

        private double GetS()
        {
            double result = 30;
            if (Forces.Ved <= 0.2 * Vrd_max)
                result = (new List<double>() { 0.8 * Geometry.d, 30 }).Min();
            if (0.2 * Vrd_max <= Forces.Ved && Forces.Ved <= 0.67 * Vrd_max)
                result = (new List<double>() { 0.6 * Geometry.d, 30 }).Min();
            if (Forces.Ved >= 0.67 * Vrd_max)
                result = (new List<double>() { 0.3 * Geometry.d, 20 }).Min();
            s_max = result - (result % 2.5);
            s_min = 5;

            List_s = new List<double>() { s_min };
            for (double i = s_min; i <= s_max; i = i + 2.5)
            {
                if (!List_s.Any(n => n == i))
                    List_s.Add(i);
            }

            return result;
        }

        private double GetSp()
        {
            double result = 30;
            if (Forces.Ved <= 0.2 * Vrd_max)
                result = (new List<double>() { 1.0 * Geometry.d, 80 }).Min();
            if (0.2 * Vrd_max <= Forces.Ved && Forces.Ved <= 0.67 * Vrd_max)
                result = (new List<double>() { 0.6 * Geometry.d, 30 }).Min();
            if (Forces.Ved >= 0.67 * Vrd_max)
                result = (new List<double>() { 0.3 * Geometry.d, 20 }).Min();

            sp_max = result;
            sp_min = 5;
            return result;
        }

        private void GetListOfM()
        {
            var a = 2;
            do
            {
                if (a * sp_min < Geometry.b - (2 * Geometry.d1))
                    List_m.Add(a + 1);
                a++;

            } while (a * sp_min < Geometry.b - (2 * Geometry.d1));
        }

        private void minArmatura()
        {
            s = s_max;
            m = List_m.Min();
            Asw_min = Material.beton.ρ * s * Geometry.b / m;
        }


        public string Result()
        {
            var error = "Errors: ";
            Errors.ForEach(x => error += x+Environment.NewLine);
            return $@"//////Result///////
    Forces:
        VEd:        {Forces.Ved:F2}kN
        VRd:        {Vrd:F2}kNm
        VRd,c:      {Vrd_c:F2}kNm
        VRd,max:    {Vrd_max:F2}kNm
        Θ:          {Θ.Angle():F2}
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
        Iskor. arm: {IskoriscenostArmature:F2}%({Vrd_s:F2}kN)
        Iskor. bet: {IskoriscenostBetona:F2}%({Vrd_max2:F2}kN)
        As1:        {As1:F2}
        Asw/s:      {Asw/s*100:F2} cm2/m 
        Asw_min:    {Asw_min:F2} cm2 
        As_add:     {As_add:F2} cm2
        "+ error;

        }
        public override string ToString()
        {
            return Result();
        }
        public void Dispose()
        {
            GC.Collect();
        }
    }
}
