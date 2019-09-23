using CalcModels;

namespace CalcModels 
{
    public interface ICoeffService
    {
        IMaterial Material { get; }

        CoeffForCalcRectCrossSectionModelEC GetByμ(double μSd, int percision = 4);
        CoeffForCalcRectCrossSectionModelEC GetByξ(double ξ);
        CoeffForCalcRectCrossSectionModelEC GetNew();
        CoeffForCalcRectCrossSectionModelEC GetNew(double eps_c, double eps_s1);
        CoeffForCalcRectCrossSectionModelEC GetByμ(double Msd, double b, double d);
        CoeffForCalcRectCrossSectionModelEC GetByξ_lim();
    }
}