using CalcModels;
using System;

namespace InterDiagRCSection
{
    public class CrossSectionStrains :ICrossSectionStrains
    {
        private readonly bool Invert;
        private readonly IMaterial material;
        private readonly IElementGeometryWithReinf geometry;


        //public double Fc => 1 * x * geometry.b * sig_c / 10;
        public double[] Fc { get
            {

                return new double[] {
                    1 * x * geometry.b * sig_c / 10,
                    (geometry.b_eff - geometry.b) <= 0 ? 0 : 1 * (x < geometry.h_f?x:geometry.h_f) * (geometry.b_eff - geometry.b)* sig_c / 10
                };
            } }
        public double Fs2
        {
            get
            {
                if (!Invert)
                    return geometry.As_2 * sig_s2 / 10;
                else return geometry.As_2 * sig_s1 / 10;
            }
        }
        public double Fs1
        {
            get
            {
                if (!Invert)
                    return geometry.As_1 * sig_s1 / 10;
                else return geometry.As_1 * sig_s2 / 10;
            }
        }

        //public double N_Rd => -(Fc - Fs2 - Fs1);
        public double N_Rd { get
            {
                double sumN=0;
                foreach (var item in Fc)
                {
                    sumN += item;
                }
                
                return -(sumN - Fs2 - Fs1);
            } }
        public double M_Rd
        {
            get
            {
                if (!Invert)
                {
                    if (x > geometry.h_f)
                        return (Fc[0] * (geometry.y1 - ka * x)
                        + (Fc[1] * (geometry.y1 - ka * geometry.h_f))
                        - (Fs2 * (geometry.y2 - geometry.d2))
                        + (Fs1 * (geometry.y1 - geometry.d1))) / 100;
                    else 
                        return ((Fc[0]+ Fc[1]) * (geometry.y1 - ka * x)
                        - (Fs2 * (geometry.y2 - geometry.d2))
                        + (Fs1 * (geometry.y1 - geometry.d1))) / 100;
                }
                else
                {
                    return -(((Fc[0]+ Fc[1]) * (geometry.y2 - ka * x))
                    + (Fs2 * (geometry.y2 - geometry.d2))
                    - (Fs1 * (geometry.y1 - geometry.d1))) / 100;
                }
                   
            }
            //=((0.85*[@x]*[@σc]/10*b*(h/2-[@ka]*[@x]))-(As_2*[@σs2]/10*(h/2-d_2))+(As_1*[@σs1]/10*(h/2-d_1)))/100
            //=-((0.85*[@x]*[@σc]/10*b*(h/2-[@ka]*[@x]))+(As_2*[@σs1]/10*(h/2-d_2))-(As_1*[@σs2]/10*(h/2-d_1)))/100
        }
        public double εc2 { get; internal set; }
        public double εc1 { get; internal set; }

        public double εs1 { get; internal set; }
        public double εs2 { get; internal set; }
        /// <summary>
        /// Stress in concrete in Mpa
        /// </summary>
        public double sig_c
        {
            get
            {
                if (Math.Abs(this.εc2) == 0 || Math.Abs(this.εc2) <= Math.Abs(material.beton.εc2))
                    return material.beton.fcd * Math.Abs(1 - (1 - Math.Pow(this.εc2 / material.beton.εc2, material.beton.n)));
                else return material.beton.fcd;
                //        = IF(AND([@[εc
                //(‰)]]<= 0,ABS([@[εc
                //(‰)]])<=ABS(eps_c0)),fcd* ABS(1-(1-POWER([@[εc
                //(‰)]]/eps_c0,n))),fcd)
            }
        }
        public double sig_s1
        {
            get
            {
                if (Math.Abs(εs1) * material.armatura.Es > material.armatura.fyd * 10)
                    return Math.Abs(εs1) / εs1 * material.armatura.fyd * 10;
                else return εs1 * material.armatura.Es;
                //= IF(ABS([@[εs1
                //(‰)]]* Es)> fyd,IF([@[εs1
                //  (‰)]]*Es < 0,-fyd,fyd),[@[εs1
                //    (‰)]]*Es)
            }
        }
        public double sig_s2
        {
            get
            {
                if (Math.Abs(εs2) * material.armatura.Es > material.armatura.fyd * 10)
                    return Math.Abs(εs2) / εs2 * material.armatura.fyd * 10;
                else return εs2 * material.armatura.Es;
                //= IF(ABS([@[εs1
                //(‰)]]* Es)> fyd,IF([@[εs1
                //  (‰)]]*Es < 0,-fyd,fyd),[@[εs1
                //    (‰)]]*Es)
            }

        }
        private double Set_εs1()
        {
            //=-(ABS(eps_c0)-((ABS(eps_c0)-[@εc1])*(h-d_1-C_dot)/(h-C_dot)))
            return -(Math.Abs(material.beton.εc2) - ((Math.Abs(material.beton.εc2) - this.εc1) * (geometry.h - geometry.d1 - this.c) / (geometry.h - this.c)));
        }
        private double Set_εs2()
        {
            if (this.x >= geometry.h)
                return ((this.εc2 - material.beton.εc2) * (this.c - geometry.d2) / this.c) + material.beton.εc2;
            if (this.x == 0)
                return this.εs1 * geometry.d2 / (geometry.h - geometry.d1);
            return this.εc2 / this.x * (this.x - geometry.d2);
            //      = IF([@x] >= h, (([@[εc(‰)]]-eps_c0)*(C_dot - d_1) / C_dot)+eps_c0
            //      ,IF([@x] <= 0,([@[εs1(‰)]]-[@[εc(‰)]])*d_2 / d +[@[εc(‰)]],
            //      [@[εc(‰)]]/[@x]*([@x]-d_1)))
        }
        private double Set_εc1()
        {

            if (geometry.h == x)
            {
                if (εs1 == -1000000 || εs2 != material.armatura.eps_ud)
                    return ((this.εc2 - material.beton.εc2) * (geometry.h - c) / c) - material.beton.εc2;
                else
                    return 0;
            }
            else if (Math.Abs(εc2) < Math.Abs(material.beton.εcu2) && εs1 != material.armatura.eps_ud)
                return ((this.εc2 - material.beton.εc2) * (geometry.h - c) / c) - material.beton.εc2;
            else return (this.εs1 * (geometry.h - x)) / (geometry.h - x - geometry.d1);


            //        = IF(AND(ABS([@[εc(‰)]]) < ABS(eps_cu3), [@[εs1(‰)]]<>20),
            //          (([@[εc(‰)]]-eps_c0)*(h-C_dot)/C_dot)-eps_c0,
            //          ([@[εs1(‰)]]*(h-[@x]))/(h-[@x]-d_1))

            //                = IF(ABS([@[εc(‰)]])< ABS(eps_cu3),
            //                  (([@[εc(‰)]]-eps_c0)*(h - C_dot) / C_dot)-eps_c0,0)

        }
        public double ξ { get; internal set; }
        public double ζ { get; internal set; }
        public double μRd { get; internal set; }
        public double ω { get; internal set; }
        public double kd { get; internal set; }
        public double x { get; internal set; }
        public double c => (1 - material.beton.εc2 / material.beton.εcu2) * geometry.h; //=(1-eps_c0/eps_cu3)*h;
        public virtual double αv
        {
            get
            {
                if (εc2 == 0)
                    return 0;
                var e = Math.Abs(εc2);
                //if (this.x == geometry.h)
                //{
                //    e = Math.Abs(material.beton.εcu2);
                //    return (3 * e - 2) / (3 * e);;
                //}
                if (e > 0 && e <= Math.Abs(material.beton.εc2))
                    return (e / 12) * (6 - e);
                return (3 * e - 2) / (3 * e);
                //   =IF(ABS([@[εc
                //   (‰)]])<= ABS(eps_c0),(ABS(A186) / 12) * (6 - ABS([@[εc
                //   (‰)]])),IF([@[εc
                //   (‰)]]= 0,0,(3 * ABS([@[εc
                //   (‰)]])-2)/ (3 * ABS([@[εc
                //   (‰)]]))))
            }
        }
        public virtual double ka
        {
            get
            {
                var e = Math.Abs(εc2);
                //if (εc2 == -2 && εs2 == -2)
                //    return 0.500;
                //if (this.x == geometry.h)
                //{
                //    e = Math.Abs(material.beton.εcu2);
                //    return (e * (3 * e - 4) + 2) / (2 * e * (3 * e - 2));
                //}
                if (this.x == geometry.h)
                {
                    var max_ka = 0.5;
                    var ecu2 = Math.Abs(material.beton.εcu2);
                    var min_ka = (ecu2 * (3 * ecu2 - 4) + 2) / (2 * ecu2 * (3 * ecu2 - 2));
                    var ec2= Math.Abs(material.beton.εc2);
                   
                    return (((ecu2-e) *(max_ka-min_ka))/(ecu2-ec2))+min_ka;
                }
                //if (this.x == geometry.h)
                //{
                //    e = Math.Abs(material.beton.εcu2);
                //    return (e * (3 * e - 4) + 2) / (2 * e * (3 * e - 2));
                //}
                if (e == 0 || e <= Math.Abs(material.beton.εc2))
                    return (8 - e) / (4 * (6 - e));
                return (e * (3 * e - 4) + 2) / (2 * e * (3 * e - 2));
                //                   = IF([@[εc
                //                  (‰)]]<= eps_c0,(8 - ABS(A186)) / (4 * (6 - ABS(A186))),IF([@[εc
                //                  (‰)]]= 0,0,(ABS(A186) * (3 * ABS(A186) - 4) + 2) / (2 * ABS(A186) * (3 * ABS(A186) - 2))))
            }
        }

        public CrossSectionStrains(IMaterial material, IElementGeometryWithReinf geometry, bool Invert = false) 
        {
            this.material = material;
            this.geometry = geometry;
            this.Invert = Invert;
        }
        /// <summary>
        /// Rotates about point A adn B
        /// Full compretion
        /// </summary>
        /// <param name="εc2">Dilation of concrete at the top edge</param>
        /// <param name="εs1">Dilation of reinforsment at the bottom edge</param>
        public virtual void SetByEcEs1(double εc2, double εs1)
        {

            this.εc2 = εc2;
            this.εs1 = εs1;
            var ec2 = Math.Abs(εc2);
            var es1 = εs1;
            this.ξ = εc2 == εs1 ? 0 : ec2 / (es1 + ec2);
            this.ζ = 1 - (this.ξ * this.ka);
            this.ω = this.αv * this.ξ;
            this.μRd = this.αv * this.ξ * this.ζ;
            this.x = geometry.d * this.ξ >= geometry.h ? geometry.h : geometry.d * this.ξ; //IF([@ξ]*d>=h,h,ABS([@ξ])*d)

            if (εc2 > 0 && εs1 == material.armatura.eps_ud)
                this.x = 0;
            this.εc1 = Set_εc1();
            this.εs2 = Set_εs2();
        }
        /// <summary>
        /// Rotates about point C
        /// Full compretion
        /// </summary>
        /// <param name="εc2">Dilation of concrete at the top edge</param>
        public virtual void SetByEcEs1(double εc2)
        {
            this.x = geometry.h;
            this.εs1 = -1000000;
            this.εc2 = εc2;
            this.εc1 = Set_εc1();
            this.εs1 = Set_εs1();
            this.εs2 = Set_εs2();
            var ec2 = Math.Abs(εc2);
            var es1 = εs1;
            this.ξ = εc2 == εs1 ? 0 : ec2 / (es1 + ec2);
            this.ζ = 1 - (this.ξ * this.ka);
            this.ω = this.αv * this.ξ;
            this.μRd = this.αv * this.ξ * this.ζ;
        }

        public override string ToString()
        {
            return $"εc2/εs1={εc2:F3}/{εs1:F3}";
        }

    }


}
