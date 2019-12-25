using CalcModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TabeleEC2.Model;
using InterDiagRCSection;
using System.Threading.Tasks;
using System;

namespace CreckingCalcENTest
{
    [TestClass]
    public class SectionStrainsTest
    {
        private static SectionStrainsModel Section;
         

        [ClassInitialize]
        public static void Init(TestContext tc)
        {
            Section = new SectionStrainsModel(
                new Material
                {
                    beton = new BetonModelEC("C25/30"),
                    armatura = ReinforcementType.GetArmatura().Single(r => r.name == "B500B")
                }, new ElementGeometryWithReinfT
                {
                    b = 30,
                    h = 30,
                    d1 = 6,
                    d2 = 6,
                    As_1 = 6.8,
                    As_2 = 6.8
                }
            );
        }

        //[DataRow(-3.5, 20, 0.148936170212766, 0.938047559449312, 0.113097932699562, 0.120567375886525, 0.80952380952381, 0.415966386554622, 3.57446808510638, 2.375, 14.1666666666667, 434.782608695652, 434.782608695652, 25.875, 439.389454209066, 17.4492032593934)]
        //[DataRow(-3.5, 5.5, 0.388888888888889, 0.838235294117647, 0.263888888888889, 0.314814814814815, 0.80952380952381, 0.415966386554622, 9.33333333333333, -1.25, 14.1666666666667, 434.782608695652, -250, 7.75, -271.014492753623, 79.3936956521739)]
        //[DataRow(-2.8, -10000000000, 1.875, 0.22, 0.314285714285714, 1.42857142857143, 0.761904761904762, 0.416, 30, -2.42666666666667, 14.1666666666667, -261.333333333333, -434.782608695652, 0.933333333333334, -1748.35884057971, 37.9365242236025)]
        //[DataRow(-2, -10000000000, 0, 1, 0, 0, 0.666666666666667, 0.5, 30, -2, 14.1666666666667, -400, -400, 2, -1819, 0)]
        //[DataRow(0, 20, 0, 1, 0, 0, 0, 0.333333333333333, 0, 5, 0, 434.782608695652, 434.782608695652, 25, 591.304347826087, 0)]
        //[DataRow(-1.6, 20, 0.0740740740740741, 0.973063973063973, 0.0422862368541381, 0.0434567901234568, 0.586666666666667, 0.363636363636364, 1.77777777777778, 3.8, 9.06666666666667, 434.782608695652, 434.782608695652, 25.4, 542.948792270531, 5.89962199775534)]
        //[DataTestMethod]
        //public void CrossSectionStrainsTest(
        //    double εc, double εs1, double ξ,
        //    double ζ, double mi_Rd, double ω, 
        //    double αv, double ka, double x, 
        //    double εs2, double σc, double σs1,
        //    double σs2, double εc1, double N_Rd, 
        //    double M_Rd)


 
        [TestMethod]
        [DataRow(-3.5, 20)]
        [DataRow(-3.5, 7.5)]
        [DataRow(-3.5, 1)]
        [DataRow(-3.3,1000000)]
        public async Task CalcTest(double eps_c, double eps_s1)
        {
            if (eps_s1 == 1000000)
                Section.SetByEcEs1(eps_c);
            else
                Section.SetByEcEs1(eps_c, eps_s1);
           var eps_ofZ=  Section.Get_eps(0);

            Assert.IsTrue(Section.eps_c1== eps_ofZ);
        }
        //
    }
}
