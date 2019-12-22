namespace InterDiagRCSection
{
    public interface ICalcForces
    {
        RCSectionForces Calc(CalcForcesType calcType);
        void SetStrains(ISectionStrainsModel sectionStrains);
    }



}
