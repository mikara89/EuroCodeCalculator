using CalcModels;
using System;

namespace InterDiagRCSection
{
    public class SectionStrainsModel : ISectionStrainsModel
    {

        public readonly IMaterial material; 
        public readonly IElementGeometryWithReinf geometry;

        public double eps_c { get; internal set; }
        public double eps_c1 { get; internal set; }

        public double eps_s1 { get; internal set; }
        public double eps_s2 { get; internal set; }

        private double Set_eps_s2()
        {
            if (this.x >= geometry.h)
                return ((this.eps_c - material.beton.εc2) * (this.c - geometry.d2) / this.c) + material.beton.εc2;
            if (this.x == 0)
                return this.eps_s1 * geometry.d2 / (geometry.h - geometry.d1);
            return this.eps_c / this.x * (this.x - geometry.d2);
        }

        private double Set_eps_c1()
        {

            if (geometry.h == x)
            {
                if (eps_s1 == -1000000 || eps_s2 != material.armatura.eps_ud)
                    return ((this.eps_c - material.beton.εc2) * (geometry.h - c) / c) - material.beton.εc2;
                else
                    return 0;
            }
            else if (Math.Abs(eps_c) < Math.Abs(material.beton.εcu2) && eps_s1 != material.armatura.eps_ud)
                return ((this.eps_c - material.beton.εc2) * (geometry.h - c) / c) - material.beton.εc2;
            else return (this.eps_s1 * (geometry.h - x)) / (geometry.h - x - geometry.d1);

        }
        public double ξ { get; internal set; }
        public double x { get; internal set; }
        public double c => (1 - material.beton.εc2 / material.beton.εcu2) * geometry.h; //=(1-eps_c0/eps_cu3)*h;

        public SectionStrainsModel(IMaterial material, IElementGeometryWithReinf geometry)
        {
            this.material = material;
            this.geometry = geometry;
        }
        /// <summary>
        /// Rotates about point A and B
        /// Full compretion
        /// </summary>
        /// <param name="eps_c">Dilation of concrete at the top edge</param>
        /// <param name="eps_s1">Dilation of reinforsment at the bottom edge</param>
        public virtual void SetByEcEs1(double eps_c, double eps_s1)
        {
            this.eps_c = eps_c;
            this.eps_s1 = eps_s1;
            var ec2 = Math.Abs(eps_c);
            var es1 = eps_s1;
            this.ξ = eps_c == eps_s1 ? 0 : ec2 / (es1 + ec2);
            this.x = geometry.d * this.ξ >= geometry.h ? geometry.h : geometry.d * this.ξ;

            if (eps_c > 0 && eps_s1 == material.armatura.eps_ud)
                this.x = 0;
            this.eps_c1 = Get_eps(0);
            this.eps_s2 = Get_eps(geometry.h - geometry.d2);
        }
        /// <summary>
        /// Rotates about point C
        /// Full compretion
        /// </summary>
        /// <param name="eps_c">Dilation of concrete at the top edge</param>
        public virtual void SetByEcEs1(double eps_c)
        {
            this.x = geometry.h;
            this.eps_c = eps_c;
            this.eps_s1 = Get_eps(geometry.d1);

            this.eps_c1 = Get_eps(0);
            this.eps_s2 = Get_eps(geometry.h - geometry.d2);
            var ec2 = Math.Abs(eps_c);
            var es1 = eps_s1;
            this.ξ = eps_c == eps_s1 ? 0 : ec2 / (es1 + ec2);
        }

        public override string ToString()
        {
            return $"εc2/εs1={eps_c:F3}/{eps_s1:F3}";
        }

        public double Get_eps(double z)
        {

            if (x == geometry.h)
            {
                if (z < geometry.h - c)
                    return (Math.Abs(eps_c) - Math.Abs(material.beton.εc2)) * (geometry.h - c - z) / c - Math.Abs(material.beton.εc2);
                else if (z > geometry.h - c)
                    return (eps_c - material.beton.εc2) * (z - geometry.h + c) / c + material.beton.εc2;
                else
                    return 0;
            }

            else if (x == 0)
            {
                return (eps_s1 - eps_c) * (geometry.h - z) / (geometry.h - geometry.d1) + eps_c;
            }

            else
            {
                if (x > z)
                    return eps_s1 * (geometry.h - x - z) / (geometry.h - x - geometry.d1);
                else if (x < z)
                    return eps_c * (z - geometry.h + x) / x;
                else
                    return 0;
            }
        }
    }



}
