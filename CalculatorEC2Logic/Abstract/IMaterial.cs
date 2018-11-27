using TabeleEC2.Model;

namespace CalculatorEC2Logic
{
    public interface IMaterial
    {
        BetonModelEC beton { get; set; } 
        ReinforcementTypeModelEC armatura { get; set; } 
    }
}
