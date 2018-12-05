namespace CalculatorEC2Logic
{
    public interface IForcesSlenderness
    {
        double M_top { get; set; }
        double M_bottom { get; set; }
        double NEd { get; set; }
        double M0Ed { get; }
        double MEd(double e2 = 0);
        double M02 { get; }
        double M01 { get; }
    }
}
