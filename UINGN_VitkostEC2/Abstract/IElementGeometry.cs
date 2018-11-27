namespace CalculatorEC2Logic
{
    public interface IElementGeometry
    {
        UnitDimesionType unit { get; set; }
        double b { get; set; }
        double h { get; set; }
        double d1 { get; set; }
        double d2 { get; set; }
        double d { get; }
    }
}
