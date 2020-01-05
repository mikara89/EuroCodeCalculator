using System;

namespace InterDiagRCSection
{
    public class SectionStrainsModel : ICrossSectionStrains 
    {
        public double ka { get; set; }

        public double kd { get; set; }

        public double M_Rd { get; set; }

        public double N_Rd { get; set; }

        public double sig_c { get; set; }

        public double sig_s1 { get; set; }

        public double sig_s2 { get; set; }

        public double αv { get; set; }

        public double εc1 { get; set; }

        public double εc2 { get; set; }

        public double εs1 { get; set; }

        public double εs2 { get; set; }

        public double ζ { get; set; }

        public double μRd { get; set; }

        public double ξ { get; set; }

        public double ω { get; set; }

        public double c { get; set; }

        public double x { get; set; }

        public double Fc { get; set; }
        public double Fs1 { get; set; }
        public double Fs2 { get; set; }

        public double Zc  { get; set; }

        public double Zs1  { get; set; }

        public double Zs2  { get; set; }

        public void SetByEcEs1(double εc2)
        {
            throw new NotImplementedException();
        }

        public void SetByEcEs1(double εc2, double εs1)
        {
            throw new NotImplementedException();
        }
    }
}
