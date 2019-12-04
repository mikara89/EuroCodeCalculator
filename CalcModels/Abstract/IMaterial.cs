
namespace CalcModels
{
    public interface IMaterial
    {
        IBetonModel beton { get; set; }
        IReinforcementTypeModel armatura { get; set; } 
    }
    
}
