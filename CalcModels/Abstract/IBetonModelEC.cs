namespace CalcModels
{
    public interface IBetonModel
    {
        double Ecm { get; }
        double fcd { get; }
        int fck { get; }
        int fck_cube { get; }
        int fcm { get; }
        double fctk005 { get; }
        double fctk095 { get; }
        double fctm { get; }
        double n { get; set; }
        string name { get; }
        double ni { get; set; }
        double αcc { get; set; }
        double εc2 { get; set; }
        double εc3 { get; set; }
        double εcu2 { get; set; }
        double εcu3 { get; set; }
        double ξ_lim { get; }
        double ρ { get; }
        double GetSigma_f(double eps_b);
        string ToString();
    }
}