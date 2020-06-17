namespace CalcModels
{
    public interface IElementGeometryWithReinf : IElementGeometry, IElementGeometryAddYc, IElementGeometryAddMomOfInertia
    {
        double As_1 { get; set; }
        double As_2 { get; set; }
        double Get_b(double z);
        bool IsInverted { get; }
        void Invert(bool isInverted = true);
    }
    public interface IElementGeometryStiffness : IElementGeometry, IElementGeometryAddYc, IElementGeometryAddMomOfInertia
    {
        double L { get; }
        NodeStiffness Lvl { get; }

        
    }
    public enum NodeStiffness
    {
        Column = 0,
        Story1 = 1,
        Story2 = 2,
    }
}
