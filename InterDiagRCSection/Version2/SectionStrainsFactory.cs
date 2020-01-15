using CalcModels;
using System;

namespace InterDiagRCSection
{
    /// <summary>
    /// 
    /// </summary>
    public class SectionStrainsFactory : ISectionStrainsFactory
    {
        /// <summary>
        /// Material of given section
        /// </summary>
        public IMaterial material { get; internal set; }
        /// <summary>
        /// Geometry of given section
        /// </summary>
        public IElementGeometryWithReinf geometry { get; internal set; }
        /// <summary>
        /// Diletion of concrete at the top edge of setion
        /// </summary>
        public double eps_c { get; internal set; }
        /// <summary>
        /// Diletion of concrete at the bottom edge of setion
        /// </summary>
        public double eps_c1 { get; internal set; }
        /// <summary>
        /// Diletion of reinforcement at d1 distance of the bottom edge of setion
        /// </summary>
        public double eps_s1 { get; internal set; }
        /// <summary>
        /// Diletion of reinforcement at d2 distance of the top edge of setion
        /// </summary>
        public double eps_s2 { get; internal set; }

        //public double ξ { get; internal set; }
        /// <summary>
        /// Neutral line where delatations are zero
        /// </summary>
        public double x { get; internal set; }
        /// <summary>
        /// Point of rotation of deletions in full compression state
        /// </summary>
        public double c => (1 - material.beton.εc2 / material.beton.εcu2) * geometry.h;

        public SectionStrainsFactory(IMaterial material, IElementGeometryWithReinf geometry)
        {
            this.material = material;
            this.geometry = geometry;
        }
        /// <summary>
        /// Rotates about point A and B
        /// Full compretion
        /// </summary>
        /// <param name="eps_c">Diletion of concrete at the top edge</param>
        /// <param name="eps_s1">Diletion of reinforsment at the bottom edge</param>
        public virtual void SetByEcEs1(double eps_c, double eps_s1)
        {
            this.eps_c = eps_c;
            this.eps_s1 = eps_s1;
            var ec2 = Math.Abs(eps_c);
            var es1 = eps_s1;
            var ξ = eps_c == eps_s1 ? 0 : ec2 / (es1 + ec2);
            this.x = geometry.d * ξ >= geometry.h ? geometry.h : geometry.d * ξ;

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
                if (geometry.h-x > z)
                {
                    if (z==geometry.d1 && eps_s1 == 0)
                        return 0;
                    return eps_s1 * (geometry.h - x - z) / (geometry.h - x - geometry.d1);
                }            
                else if (geometry.h - x <= z)
                    return eps_c * (z - geometry.h + x) / x;
                else
                    return 0;
            }
        }

        /// <summary>
        /// Calculete width in given distance from bottom edge of section
        /// </summary>
        /// <param name="z">distance from bottom edge of section</param>
        /// <returns>width in [cm]</returns>
        public double Get_b(double z)
        {
            return geometry.Get_b(z);
        }
        /// <summary>
        /// Calculete pressure in concrete in giving distance from bottom edge
        /// </summary>
        /// <param name="z">distance from bottom edge of section</param>
        /// <returns>pressure in [MPa]</returns>
        public double Get_sig(double z)
        {
            var eps_c = Get_eps(z);
            if (Math.Abs(eps_c) >= 0 && Math.Abs(eps_c) <= Math.Abs(material.beton.εc2))
                return material.beton.fcd * Math.Abs(1 - Math.Pow((1 - eps_c / material.beton.εc2), material.beton.n));
            else return material.beton.fcd;
        }
        /// <summary>
        /// Calculete pressure in reinforcment by giving parametar
        /// </summary>
        /// <param name="eps_s">diletation in reinforcment</param>
        /// <returns>pressure in [MPa]</returns>
        public double GetSigmaForReinf(double eps_s)  
        {
            if (Math.Abs(eps_s) * material.armatura.Es > material.armatura.fyd * 10)
                return Math.Abs(eps_s) / eps_s * material.armatura.fyd * 10;
            else return eps_s * material.armatura.Es;
        }
    }
}
