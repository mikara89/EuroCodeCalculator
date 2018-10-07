using TabeleEC2.Model;

namespace CalculatorEC2Logic
{
    public class Material : IMaterial
    {
        public BetonModelEC beton { get; set; }
        public ReinforcementTypeModelEC armatura { get; set; } 
    }
}
