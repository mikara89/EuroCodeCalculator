namespace InterDiagRCSection
{
    public interface ISectionStrainsModel
    {
        double c { get; }
        double eps_c { get; }
        double eps_c1 { get; }
        double eps_s1 { get; }
        double eps_s2 { get; }
        double x { get; }
        double ξ { get; }

        double Get_eps(double z);
        void SetByEcEs1(double eps_c);
        void SetByEcEs1(double eps_c, double eps_s1);
        string ToString();
    }
}