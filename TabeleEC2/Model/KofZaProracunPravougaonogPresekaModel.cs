using System;
using System.Collections.Generic;

namespace TabeleEC2.Model
{
    public interface IKofZaProracunPravougaonogPresekaModel
    {

    }

    public class KofZaProracunPravougaonogPresekaModelEC: IKofZaProracunPravougaonogPresekaModel
    {

        public double εc { get; set; }
        public double εs1 { get;  set; }
        public double ξ { get;  set; }
        public double ζ { get;  set; }
        public double μRd { get;  set; }
        public double ω { get;  set; }
        public double kd { get; set; }
        public double αv { get
            {
                if (εc == 0)
                    return 0;
                var e = Math.Abs(εc);
                if (e>0 && e <= 2)
                    return (e / 12) * (6 - e);
                return (3*e -2)/(3*e);
            }}
        public double ka
        {
            get
            {
                if (εc == 0)
                    return 0;
                var e = Math.Abs(εc);
                if (e > 0 && e <= 2)
                    return (8-e) / (4*(6-e));
                return (e *(3*e-4)+2) / (2*e*(3*e-2));
            }
        }

        public bool LomPoBetonu { get { return εs1 < 5.0 ? false : true; } }
        public bool LomPoArmaturi { get { return !LomPoBetonu; } }

        public KofZaProracunPravougaonogPresekaModelEC()
        {

        }
        public KofZaProracunPravougaonogPresekaModelEC(double εc, double εs1)
        {
            SetByEcEs1(εc, εs1);
        }

        public KofZaProracunPravougaonogPresekaModelEC(double μSd) 
        {
            var k= KofZaProracunPravougaonogPresekaEC.Get_Kof_From_μ(μSd);
            SetByEcEs1(k.εc, k.εs1);
        }
        public void SetByEcEs1(double εc, double εs1)
        {

            this.εc = εc;
            this.εs1 = εs1;
            var ec = Math.Abs(εc);
            var es1 = εs1;
            this.ξ = ec / (es1 + ec);
            this.ζ = 1 - (this.ξ * this.ka);
            this.ω = 0.85 * this.αv * this.ξ;
            this.μRd = 0.85 * this.αv * this.ξ * this.ζ;
        }
        public void SetByξ(double ξ)
        {
            if (ξ < 0.149 && ξ>0)
            {
                this.εs1 = 20;
                this.εc =-(( ξ / (1 - ξ)) * εs1);
                SetByEcEs1(this.εc, this.εs1);
                return;
            }
            if (ξ > 0.149)
            {
                this.εc = -3.5;
                this.εs1 = ((1 - ξ)/ ξ) * Math.Abs(εc);
                SetByEcEs1(this.εc, this.εs1);
                return;
            }
            SetByEcEs1(-3.5, 20);
        }

        public override string ToString()
        {
            return $"εc/εs1={εc:F3}/{εs1:F3}";
        }

    }
}
