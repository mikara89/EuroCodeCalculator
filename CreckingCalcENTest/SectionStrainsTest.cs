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
    public class ModelCrossSectionStrainsTest
    {
        private static CrossSectionStrains Section;


        [ClassInitialize]
        public static void Init(TestContext tc)
        {
            Section = new CrossSectionStrains(
                new Material
                {
                    beton = new BetonModelEC("C25/30"),
                    armatura = ReinforcementType.GetArmatura().Single(r => r.name == "B500B")
                }, new ElementGeometryWithReinf
                {
                    b = 30,
                    h = 30,
                    d1 = 6,
                    d2 = 6,
                    As_1 = 6.8,
                    As_2 = 6.8
                }
            );
            Section = new CrossSectionStrains(
                new Material
                {
                    beton = new BetonModelEC("C25/30"),
                    armatura = ReinforcementType.GetArmatura().Single(r => r.name == "B500B")
                }, new ElementGeometryWithReinf
                {
                    b_eff=175,
                    h_f=15,
                    b = 35,
                    h = 150,
                    d1 = 7,
                    d2 = 7,
                    As_1 = 106.18,
                    As_2 = 0
                }
            );
        }

        [TestMethod]
        public async Task CalcTestV2()
        {
            var s = new Solver(
               new Material
               {
                   beton = new BetonModelEC("C25/30"),
                   armatura = ReinforcementType.GetArmatura().Single(r => r.name == "B500B")
               }, new ElementGeometryWithReinf
               {
                   b_eff = 175,
                   h_f = 15,
                   b = 35, 
                   h = 150,
                   d1 = 7,
                   d2 = 7,
                   As_1 = 106.18,
                   As_2 = 0
               }
           );
            var beton = new BetonModelEC("C25/30");
            var armatura = ReinforcementType.GetArmatura().Single(r => r.name == "B500B");
            await s.Calc(0.001);

            ///Min And Max 
            var Mmin = s.List
                    .OrderBy(n => Math.Abs(n.M_Rd - 0))
                    .Take(2);
            var Nmin = s.List
                    .OrderBy(n => Math.Abs(n.N_Rd - 0))
                    .Take(2);

            Assert.IsNotNull(s.List);
        }
        //
    }
}
