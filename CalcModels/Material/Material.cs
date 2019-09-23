namespace CalcModels
{
    public class Material : IMaterial
    {
        public IBetonModelEC beton { get; set; }
        public IReinforcementTypeModel armatura { get; set; } 
    }
}
