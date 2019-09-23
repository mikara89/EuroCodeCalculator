using CalcModels;
using System;

namespace CalcModels
{
    public class CoeffForCalcRectCrossSectionModelEC : ICoeffForCalcRectCrossSectionModelEC
    {
        private readonly IMaterial material;
        private readonly IElementGeometry geomerty;

        public double εc { get; set; }
        public double εs1 { get; set; }
        public double ξ { get; set; }
        public double ζ { get; set; }
        public double μRd { get; set; }
        public double ω { get; set; }
        public double kd { get; set; }
        public double αv
        {
            get
            {
                if (εc == 0)
                    return 0;
                var e = Math.Abs(εc);
                if (e > 0 && e <= 2)
                    return (e / 12) * (6 - e);
                return (3 * e - 2) / (3 * e);
            }
        }
        public double ka
        {
            get
            {
                if (εc == 0)
                    return 0;
                var e = Math.Abs(εc);
                if (e > 0 && e <= 2)
                    return (8 - e) / (4 * (6 - e));
                return (e * (3 * e - 4) + 2) / (2 * e * (3 * e - 2));
            }
        }

        public CoeffForCalcRectCrossSectionModelEC(IMaterial material, IElementGeometry geomerty)
        {
            this.material = material ??
                throw new ArgumentNullException(nameof(material));
            this.geomerty = geomerty ??
                throw new ArgumentNullException(nameof(geomerty));
        }
        public CoeffForCalcRectCrossSectionModelEC(double εc, double εs1, IMaterial material, IElementGeometry geomerty)
        {
            this.material = material ??
                throw new ArgumentNullException(nameof(material));
            this.geomerty = geomerty ??
                throw new ArgumentNullException(nameof(geomerty));
            SetByEcEs1(εc, εs1);
        }

        public void SetByEcEs1(double εc, double εs1)
        {

            this.εc = εc;
            this.εs1 = εs1;
            var ec = Math.Abs(εc);
            var es1 = εs1;
            this.ξ = ec / (es1 + ec);
            this.ζ = 1 - (this.ξ * this.ka);
            this.ω = material.beton.ni * this.αv * this.ξ;
            this.μRd = material.beton.ni * this.αv * this.ξ * this.ζ;
        }

        public double εs2(double d, double d2)
        {
            var x = this.ξ * d;
            return this.εc * (x - d2) / x;
        }

        public void SetByξ(double ξ)
        {
            var test = new CoeffForCalcRectCrossSectionModelEC(material.beton.εcu2, material.armatura.eps_ud, material, geomerty);
            if (ξ < test.ξ && ξ > 0)
            {
                this.εs1 = material.armatura.eps_ud;
                this.εc = -((ξ / (1 - ξ)) * εs1);
                SetByEcEs1(this.εc, this.εs1);
                return;
            }
            if (ξ > test.ξ)
            {
                this.εc = material.beton.εcu2;
                this.εs1 = ((1 - ξ) / ξ) * Math.Abs(εc);
                SetByEcEs1(this.εc, this.εs1);
                return;
            }
            SetByEcEs1(material.beton.εcu2, material.armatura.eps_ud);
        }
        public void SetByX(double X, double d)
        {
            SetByξ(X / d);
        }
        public override string ToString()
        {
            return $"εc/εs1={εc:F3}/{εs1:F3}";
        }
    }
}
