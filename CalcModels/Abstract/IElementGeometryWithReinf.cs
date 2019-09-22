namespace CalcModels
{
    public interface IElementGeometryWithReinf : IElementGeometry, IElementGeometryT
    {
        double As_1 { get; set; } 
        double As_2 { get; set; }
    }
    public interface IElementGeometryT:IElementGeometry
    {
        double b_eff { get; set; }
        double h_f { get; set; }

        double y1 { get; }
        double y2 { get; }
    }


}
