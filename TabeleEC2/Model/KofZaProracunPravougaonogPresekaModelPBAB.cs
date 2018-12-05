using System;

namespace TabeleEC2.Model
{
    public class KofZaProracunPravougaonogPresekaModelPBAB : IKofZaProracunPravougaonogPresekaModel
    {
        public KofZaProracunPravougaonogPresekaModelPBAB()
        {

        }
        public KofZaProracunPravougaonogPresekaModelPBAB(double k)
        {
            var _k = KofZaProracunPravougaonogPresekaPBAB.Get_Kof_From_k(k);
            SetByS_PBAB(_k.s);
        }

        public KofZaProracunPravougaonogPresekaModelPBAB(double εb, double εa1)
        {
            this.εb = εb;
            this.εa1 = εa1;
        }
        public double εb { get; set; }
        public double εa1 { get; set; }

        public double s
        {
            get
            {
                if (εb == 0)
                    return 0;
                return (εb) / (εb - εa1);
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
        public double η
        {
            get
            {
                if (εb == 0)
                    return 0;
                var e = Math.Abs(εb);
                if (e > 0 && e <= 2)
                    return (8 - e) / (4 * (6 - e));
                return (e * (3 * e - 4) + 2) / (2 * e * (3 * e - 2));
            }
        }
        public double ζ
        {
            get
            {
                if (εb == 0)
                    return 0;
                return 1 - (η * s);
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

        public void SetByS_PBAB(double s)
        {
            if (s < 0.259 && s > 0)
            {
                this.εa1 = 10;
                this.εb = ((s / (1 - s)) * εa1); ;
                return;
            }
            if (s > 0.259)
            {
                this.εb = 3.5;
                this.εa1 = ((1 - s) / s) * Math.Abs(εb);
                return;
            }
            this.εb = 3.5;
            this.εa1 = 10;
        }
    }
}
