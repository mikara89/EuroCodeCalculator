namespace CalcModels
{
    public class Material : IMaterial
    {
        public IBetonModel beton { get; set; }
        public IReinforcementTypeModel armatura { get; set; } 
    }
}
