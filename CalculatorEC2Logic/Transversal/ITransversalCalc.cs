using CalcModels;

namespace CalculatorEC2Logic
{
    public interface ITransversalCalc
    {
        bool IsDimOk { get; }
        void CalculateArmature(int m, double s, ReinforcementModelEC Asw_Model,double? teta=45, double alfa=90);
    }
}