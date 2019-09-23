
namespace CalcModels
{
    public interface IMaterial
    {
        IBetonModelEC beton { get; set; }
        IReinforcementTypeModel armatura { get; set; } 
    }
    
}
