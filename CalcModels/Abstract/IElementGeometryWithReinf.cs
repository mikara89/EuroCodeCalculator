namespace CalcModels
{
    public interface IElementGeometryWithReinfold : IElementGeometry, IElementGeometryT
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
    public interface IElementGeometryAddYc
    {

        double y1 { get; }
        double y2 { get; }
    }
    public interface IElementGeometryAddMomOfInertia 
    {
        double I { get; } 
    }

}
