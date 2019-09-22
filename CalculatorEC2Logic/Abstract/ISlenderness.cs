using CalcModels;

namespace CalculatorEC2Logic
{
    public interface ISlenderness 
    {
        void Calculate();
        void KontrolaCentPritPreseka();
        void ProracunArmature();
        bool IsAcOK { get; }
        IElementGeometrySlenderness ElementGeometry { get; }
        IForcesSlenderness Forces { get; }
        IMaterial Material { get; }
        double λ_lim { get; }

    }
}
