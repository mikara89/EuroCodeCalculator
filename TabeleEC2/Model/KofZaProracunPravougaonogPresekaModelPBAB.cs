using CalcModels;
using System;

namespace TabeleEC2.Model
{
    public class KofZaProracunPravougaonogPresekaModelPBAB
    {
        public IMaterial Material { get;internal set; } 
        protected KofZaProracunPravougaonogPresekaModelPBAB()
        {

        }

        protected KofZaProracunPravougaonogPresekaModelPBAB(double s) 
        {
            SetByS_PBAB(s);
        }

        protected KofZaProracunPravougaonogPresekaModelPBAB(double εb, double εa1)
        {
            this.εb = εb;
            this.εa1 = εa1;
        }
        public double εb { get; set; }
        public double x(double h) => s * h;
        public double εa1 { get; set; }
        public double εa2 { get; set; }


        public double Set_εa2(double alfa2)  
        {
            εa2= (s - alfa2) / s * εb;
            return εa2;
        }
        public double Set_εa2(double a2, double h)
        {
            return Set_εa2(a2 / h);
        }

        public double s
        {
            get
            {
                if (εb == 0)
                    return 0;
                return (Math.Abs(εb)) / (Math.Abs(εb) + εa1);
            }
        }
        public double αb1 
        {
            get
            {
                return alfa(εb);
            }
        }
        public double αb
        {
            get
            {
                if (εb == 0)
                    return 0;
                var e = Math.Abs(εb);
                if (e > 0 && e <= 2)
                    return (e / 12) * (6 - e);
                return (3 * e - 2) / (3 * e);
            }
        }
        public double η1
        {
            get
            {
                return calc_η(εb);
            }
        }
        protected double calc_η(double ε)
        {
            if (εb == 0)
                return 0;
            var e = Math.Abs(ε);
            if (e > 0 && e <= 2)
                return (8 - e) / (4 * (6 - e));
            return (e * (3 * e - 4) + 2) / (2 * e * (3 * e - 2));
        }
        public double ζ
        {
            get
            {
                if (εb == 0)
                    return 0;
                return 1 - (η1 * s);
            }
        }

        public double μ_1M
        {
            get
            {
                if (εb == 0)
                    return 0;
                return αb * s * 100;
            }
        }

        public double k
        {
            get
            {
                if (εb == 0)
                    return 0;
                return Math.Sqrt(1 / (αb * ζ * s));
            }
        }
        public bool LomPoBetonu { get { return εa1 < 10 ? true : false; } }
        public bool LomPoArmaturi { get { return !LomPoBetonu; } }


        private void SetByS_PBAB(double s)
        {
            if (s <= 0 || s > 1.167)
                throw new Exception("Invalid parametar s: " + s);
            if (s < 0.259 && s > 0)
            {
                this.εa1 = 10;
                this.εb = ((s / (1 - s)) * εa1);
                return ;
            }
            if (s > 0.259)
            {
                this.εb =- 3.5;
                this.εa1 = ((1 - s) / s) * Math.Abs(εb);
                return ;
            }
            this.εb = -3.5;
            this.εa1 = 10;
        }

        public static KofZaProracunPravougaonogPresekaModelPBAB SetBy_εb_or_εa1_PBAB(double εb, double εa1)
        {
            return new KofZaProracunPravougaonogPresekaModelPBAB(εb, εa1);
        }
        public static KofZaProracunPravougaonogPresekaModelPBAB SetBy_S_PBAB(double s) 
        {
            return new KofZaProracunPravougaonogPresekaModelPBAB(s:s);
        }
        protected double alfa(double eps)
        {
            if (eps == 0)
                return 0;
            var e = Math.Abs(eps);
            if (e > 0 && e <= 2)
                return (e / 12) * (6 - e);
            return (3 * e - 2) / (3 * e);
        }
        protected void set(double εb, double εa1)
        {
            this.εb = εb;
            this.εa1 = εa1;
        }
    }
}
