using System;
using System.Collections.Generic;
using System.Linq;
using TabeleEC2.Model;

namespace CalculatorEC2Logic
{
    public class TransverzalneSileEC2:IDisposable
    {
        //TODO
        public override string ToString()
        {
            return Result();
        }

        private double b;
        private double h;
        private BetonModelEC beton;
        private ReinforcementTypeModelEC armatura;
        private readonly ReinforcementModelEC as1_Model;
        private double d1;
        public double d { get { return h - d1; } }
        public int m;
        public List<string> Errors = new List<string>();
        public List<int> List_m= new List<int>() {2 };
        public double s { get; set; }
        public double s_max { get; private set; } 
        public double s_min { get; private set; }

        public double sp { get; set; }
        public double sp_max { get; private set; }
        public double sp_min { get; private set; }
        private double Θ= 45 * Math.PI / 180;

        public double _Θ { get {return (Θ * 180/ Math.PI); } set { Θ =value * Math.PI / 180; } } 

        public double As1 { get; } 
        public ReinforcementModelEC Asw_Model { get; set; }
        public double Asw { get { return Asw_Model!=null? Asw_Model.cm2_total:0; } }
        public double Asw_min { get; private set; }
        public double As_add { get; private set; }
        public double Vg { get; }
        public double Vq { get; }
        public double Ned { get { return 1.35 * Ng + 1.5 * Nq; } }
        private double _Ved;
        public double Ved { get
            {
                if (Vg != 0 || Vq != 0)
                    return 1.35 * Vg + 1.5 * Vq;
                return _Ved;
            } set { _Ved = value; } }

        
        /// <summary>
        /// Trans. sila koju presek moze da prihvati bez armature
        /// </summary>
        public double Vrd { get; set; }
        public double IskoriscenostArmature { get; private set; }
        public double IskoriscenostBetona { get; private set; }

        private double σcp;

        public double Vrd_c { get; set; }
        public double Vrd_max { get; private set; }
        public bool IsDimOk { get; private set; }
        public double Ng { get; private set; }
        public double Nq { get; private set; }

        public double ρ1 { get; private set; }
        public double k { get; private set; }
        public List<double> List_s { get; private set; }

        public TransverzalneSileEC2(
            double b,
            double h,
            BetonModelEC beton,
            ReinforcementTypeModelEC armatura,
            ReinforcementModelEC As1_model,  
            double Vg,
            double Vq,
            double d1,
            double Ng = 0,
            double Nq = 0            
            )
        {
            InitValidations(b,h,beton,armatura,As1_model,d1);
            this.b = b;
            this.h = h;
            this.beton = beton;
            this.armatura = armatura;
            as1_Model = As1_model;
            As1 = as1_Model.cm2_total;
            this.Vg = Vg;
            this.Vq = Vq;
            this.d1 = d1;
            this.Ng = Ng;
            this.Nq = Nq;

            Validations();
            Calc_Vrd_c();

            //if (Errors.Count() != 0)
                //return;
            GetS();
            GetSp();
            GetListOfM();

            if (Ved <= Vrd_c && Errors.Count() == 0)
            {
                minArmatura();
                CalAdditionalTransverseReinforcement();
            }
        }

        public TransverzalneSileEC2(
            double b,
            double h,
            BetonModelEC beton,
            ReinforcementTypeModelEC armatura,
            ReinforcementModelEC As1_model,
            double Ved,
            double d1,
            double Ng = 0,
            double Nq = 0
            )
        {
            InitValidations(b, h, beton, armatura, As1_model, d1);
            this.b = b;
            this.h = h;
            this.beton = beton;
            this.armatura = armatura;
            as1_Model = As1_model;
            As1 = as1_Model.cm2_total;
            this.Ved = Ved;
            this.d1 = d1;

            Validations();
            Calc_Vrd_c();

            //if (Errors.Count() != 0)
                //return;
            GetS();
            GetSp();
            GetListOfM();

            if (Ved <= Vrd_c && Errors.Count() == 0)
            {
                minArmatura();
                CalAdditionalTransverseReinforcement();
            }

        }

        private void InitValidations(double b, double h, BetonModelEC beton, ReinforcementTypeModelEC armatura, ReinforcementModelEC as1_model, double d1)
        {
            if (b <= 0)
                throw new Exception("b must be greater 0");
            if (h <= 0)
                throw new Exception("h must be greater 0");
            if (d1 <= 0)
                throw new Exception("d1 must be greater 0");
            if (2*d1 >= h)
                throw new Exception("2 x d1 must be smoller then h");
            if (beton == null)
                throw new Exception("Beton not defined!");
            if (armatura == null)
                throw new Exception("Armatura not defined!");
            if (as1_model == null)
                throw new Exception("Longitudinal reinforcement not defined!");
        }

        private void Validations()
        {
            ρ1 = As1 / b / d;

            if (ρ1 > 0.02)
                Errors.Add("ρ1 is " + Math.Round(ρ1,3) + " > 0.02. \nLower As1 or make b or h greater!");

            k = 1.0 + Math.Sqrt(200 / (d * 10));
            if (k > 2.0)
                Errors.Add("Conditions for the minimum cross-section height is not met! \nh-d1 have to be geater then 20cm");

            σcp = Ned / (b * h);
            if (σcp > 2*beton.fcd)
                Errors.Add("Conditions for minimal cross-section of concrete are not satisfied");

        }

        private void Calc_Vrd_c()
        {
            
            var γc = 1.5;
            var Crd_c = 0.18 / γc;
            
            var k1 = 0.15;

            Vrd_c = (Crd_c * k * Math.Pow((100 * ρ1 * beton.fck), 1.0 / 3.0) + (k1 * σcp)) * b*10 * d*10/1000;

            var ν_min = 0.035 * Math.Pow(k, 3.0 /2.0) * Math.Pow(beton.fck, 1.0 / 2.0);
            if(Vrd_c < ((ν_min + k1* σcp) * b*10 * d*10/1000))
                Errors.Add("Conditions for the maximum cross-section of concrete is not satisfied, Vrd_c < ((ν_min + k1* σcp) * b*10 * d*10/1000)="+ Round(Vrd_c)+"<"+ Round((ν_min + k1 * σcp) * b * 10 * d * 10 / 1000));

            var ν = 0.6 * (1.0 -(beton.fck / 250.0));///fck u MPa
            Vrd_max = 0.5 * ν * beton.fcd/10 * b * d;
            ///ako je false mora da povecamo presek ili klasu betona
            if(Ved > Vrd_max)
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
            if (Ved <= Vrd_c)
            {
                minArmatura();
                CalAdditionalTransverseReinforcement();
                return;
            }
            if (Ved > Vrd_c)
            {
                Armatura(m,s, Asw_Model);
                CalAdditionalTransverseReinforcement();
                return;
            }
                
        }
        public void Armatura(int m, double s, ReinforcementModelEC Asw_Model)
        {
            this.Asw_Model = Asw_Model; 
            this.m = m;
            this.s = s;
            var cot = (1 / Math.Tan(Θ));
            var z = 0.9 * d;
            var Vrd_s = (Asw / s) * z * armatura.fyd * m * (1 / Math.Tan(Θ));

            var ν = 0.6 * (1.0 - (beton.fck / 250.0));///fck u MPa
            var v1 = ν;
            var αcw = 1;
            var Vrd_max2 = (αcw * b*10* z*10 * v1 * beton.fcd) / (Math.Tan(Θ) + (1/ Math.Tan(Θ)))/1000; 
            Vrd = (new List<double>() {Vrd_s, Vrd_max2}).Min(); 
            IskoriscenostArmature = Round(Ved / Vrd_s);
            IskoriscenostBetona = Round(Ved / Vrd_max2);
        }

        private void CalAdditionalTransverseReinforcement()
        {
            var alfa = 90 * Math.PI / 180;
            As_add = (0.5 * Ved * ((1 / Math.Tan(Θ)) - (1 / Math.Tan(alfa)))) / armatura.fyd;
        }

        private double GetS()
        {
            double result = 30;
            if (Ved <= 0.2 * Vrd_max)
                result = (new List<double>() { 0.8 * d, 30 }).Min();
            if (0.2 * Vrd_max <= Ved && Ved <= 0.67 * Vrd_max)
                result = (new List<double>() { 0.6 * d, 30 }).Min();
            if (Ved >= 0.67 * Vrd_max)
                result = (new List<double>() { 0.3 * d, 20 }).Min();
            s_max = result - (result % 2.5);
            s_min = 5;
           
            List_s = new List<double>() { s_min};
            for (double i = s_min; i <= s_max; i = i + 2.5)
            {
                if(!List_s.Any(n=>n==i))
                    List_s.Add(i) ;
            }

            return result;
        }

        private double GetSp()
        {
            double result = 30;
            if (Ved <= 0.2 * Vrd_max)
                result = (new List<double>() { 1.0 * d, 80 }).Min();
            if (0.2 * Vrd_max <= Ved && Ved <= 0.67 * Vrd_max)
                result = (new List<double>() { 0.6 * d, 30 }).Min();
            if (Ved >= 0.67 * Vrd_max)
                result = (new List<double>() { 0.3 * d, 20 }).Min();

            sp_max = result;
            sp_min = 5;
            return result;
        }

        private void GetListOfM()
        {
            var a = 2;
            do
            {
                if (a * sp_min < b - (2 * d1))
                    List_m.Add(a + 1);
                a++;

            } while (a * sp_min < b - (2 * d1));
        }

        private void minArmatura()
        {
            s = s_max;
            m = List_m.Min();
            Asw_min = beton.ρ * s * b / m;
        }


        private string Result()
        {
            return String.Format(@"///Osnovni Podaci////
b/h={6}/{7} cm
d1={8} cm
Beton:{9}
Armatura:{10}
///Proracun///
Ved={0} kN
Vrd={1} kN
Vrd_c={2} kN
Vrd_max={3} kN
IskoriscenostArmature={4}%
IskoriscenostBetona={5}%
As1={11}cm2;
Asw={12}cm2
Asw_min={13}cm2;
As_add={14}cm2
", Round(Ved),
Round(Vrd),
Round(Vrd_c), 
Round(Vrd_max),
IskoriscenostArmature*100,
IskoriscenostBetona*100,
b,h,d1,beton.name,armatura.name,
Round(As1), Round(Asw),
Round(Asw_min), Round(As_add)

);
        }

        private double Round(double D, int percision=2)
        {
            return Math.Round(D, percision);
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
