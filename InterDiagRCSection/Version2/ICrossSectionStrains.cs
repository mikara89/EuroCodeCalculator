namespace InterDiagRCSection
{
    public interface ICrossSectionStrains
    {
        double c { get; }
        double ka { get; }
        double kd { get; }
        double M_Rd { get; }
        double N_Rd { get; }
        double sig_c { get; }
        double sig_s1 { get; }
        double sig_s2 { get; }
        double x { get; }
        double αv { get; }
        double εc1 { get; }
        double εc2 { get; }
        double εs1 { get; }
        double εs2 { get; }
        double ζ { get; }
        double μRd { get; }
        double ξ { get; }
        double ω { get; }

        void SetByEcEs1(double εc2);
        void SetByEcEs1(double εc2, double εs1);
        string ToString();
    }
}