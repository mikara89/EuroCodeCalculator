namespace CalcModels
{
    public interface IElementGeometryWithReinf : IElementGeometry, IElementGeometryAddYc
    {
        double As_1 { get; set; }
        double As_2 { get; set; }
        double Get_b(double z);
        bool IsInverted { get; }
        void Invert();
    }
}
