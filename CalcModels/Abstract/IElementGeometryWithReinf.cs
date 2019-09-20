namespace CalcModels
{
    public interface IElementGeometryWithReinf : IElementGeometry
    {
        double As_1 { get; set; } 
        double As_2 { get; set; }
    }
    
}
