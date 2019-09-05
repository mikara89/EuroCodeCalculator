using TabeleEC2.Model;

namespace CalcModels
{
    public class Material : IMaterial
    {
        public BetonModelEC beton { get; set; }
        public ReinforcementTypeModelEC armatura { get; set; } 
    }
}
