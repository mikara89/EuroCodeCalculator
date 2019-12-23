namespace InterDiagRCSection
{
    public interface ICalcForces
    {
        ISectionStrainsModel sectionStrains { get; }
        RCSectionForces Calc(CalcForcesType calcType);
    }
}
