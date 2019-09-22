using CalcModels;

namespace CalculatorEC2Logic
{
    public interface IElementGeometrySlenderness: IElementGeometry
    {
        double L { get; set; }
        double li { get; }
        double λ { get; }
        double Ix { get; }
        double Ac { get; }
        double ic { get; }
        double k { get; }
    }
}
