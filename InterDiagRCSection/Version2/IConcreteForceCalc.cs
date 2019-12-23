namespace InterDiagRCSection
{
    public interface IConcreteForceCalc
    {
        ISectionStrainsModel sectionStrains
        {
            get;
        }
        int n { get; set; }
        double GetDistance();
        double GetForce();
    }
}