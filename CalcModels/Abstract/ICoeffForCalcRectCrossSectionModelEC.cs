namespace CalcModels
{
    public interface ICoeffForCalcRectCrossSectionModelEC
    {
        double ka { get; }
        double kd { get; set; }
        double αv { get; }
        double εc { get; set; }
        double εs1 { get; set; }
        double ζ { get; set; }
        double μRd { get; set; }
        double ξ { get; set; }
        double ω { get; set; }

        void SetByEcEs1(double εc, double εs1);
        void SetByX(double X, double d);
        void SetByξ(double ξ);
        string ToString();
        double εs2(double d, double d2);
    }
}