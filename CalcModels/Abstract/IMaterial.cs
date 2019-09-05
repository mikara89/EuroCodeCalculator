using TabeleEC2.Model;

namespace CalcModels
{
    public interface IMaterial
    {
        BetonModelEC beton { get; set; } 
        ReinforcementTypeModelEC armatura { get; set; } 
    }
}
