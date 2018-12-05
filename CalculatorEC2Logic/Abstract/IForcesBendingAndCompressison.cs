namespace CalculatorEC2Logic
{
    public interface IForcesBendingAndCompressison 
    {
        double Msd { get; }
        double Nsd { get;}
        double Msds(double h, double d1);
        bool IsMsdNegativ { get; }
    }
}
