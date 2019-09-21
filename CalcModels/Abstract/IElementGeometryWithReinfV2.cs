namespace CalcModels
{
    public interface IElementGeometryWithReinfV2 : IElementGeometryWithReinf
    {
        double b_eff { get; set; }
        double h_f { get; set; }

        double y1 { get; }
        double y2 { get; } 
    }
    
}
