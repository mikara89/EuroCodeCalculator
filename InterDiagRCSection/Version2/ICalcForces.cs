using System.Threading.Tasks;

namespace InterDiagRCSection
{
    public interface ICalcForces
    {
        ISectionStrainsFactory sectionStrains { get; }
        RCSectionForces Calc(CalcForcesType calcType);
    }
}
