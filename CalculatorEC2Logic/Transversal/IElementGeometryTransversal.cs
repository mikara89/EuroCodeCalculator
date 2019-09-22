using CalcModels;
using TabeleEC2.Model;

namespace CalculatorEC2Logic
{
    public interface IElementGeometryTransversal : IElementGeometry
    {
        ReinforcementModelEC As1 { get; set; }
    }
    public class ElementGeometryTransversal : ElementGeometry, IElementGeometryTransversal
    {
        public ReinforcementModelEC As1 { get ; set; }
    }
}
