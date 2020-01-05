using System.Threading.Tasks;

namespace InterDiagRCSection
{
    public interface IConcreteForceCalc
    {
        ISectionStrainsFactory sectionStrains
        {
            get;
        }
        int n { get; set; }
        double GetDistance();
        double GetForce();
    }
}