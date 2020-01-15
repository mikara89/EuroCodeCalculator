namespace InterDiagRCSection
{
    public interface ICrossSectionStrains
    {
        double F_c { get; } 
        double F_s1 { get; }
        double F_s2 { get; }
        double z_c { get; }
        double z_s1 { get; } 
        double z_s2 { get; }
        double c { get; }
        double M_Rd { get; }
        double N_Rd { get; }
        double sig_c { get; }
        double sig_s1 { get; }
        double sig_s2 { get; }
        double x { get; }
        double eps_c1 { get; }
        double eps_c { get; }
        double eps_s1 { get; }
        double eps_s2 { get; }
    }
}