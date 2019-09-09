using CalcModels;
using CreckingENCalc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TabeleEC2.Model;

namespace CreckingCalcENTest
{
    [TestClass]
    public class ModelBeamAndSlabTest
    {
        private static ReinfCreckingBeamModel beam;
        private static ReinfCreckingSlabModel slab;

        [ClassInitialize]
        public static void Init(TestContext tc)
        {
            slab = new ReinfCreckingSlabModel
            {
                unit = UnitDimesionType.cm,
                h = 20,
                c = 2.5,
                a = new double[] { 4.0, 6.0 },
                s = new double[] { 7.5, 15 },
                Ø = new double[] { 1.2, 1.6 }
            };
            beam = new ReinfCreckingBeamModel
            {
                unit = CalcModels.UnitDimesionType.cm,
                b = 30,
                h = 44,
                c = 3.2,
                a = new double[] { 4.0 },
                n = new double[] { 3 },
                Ø = new double[] { 1.6 }
            };
        }
        [TestMethod]
        public void BeamTest()
        {
            var a = beam.Rep_Ø;
        }
        [TestMethod]
        public void CreckTest()
        {
            var a = new CreckingENCalc.Solver(
                new Material
                {
                    beton = new BetonModelEC("C40/50"),
                    armatura = TabeleEC2.ReinforcementType.GetArmatura().Single(r => r.name == "B500B")
                },
                beam,
                new Forces { M3 = 43.90 });

            Assert.IsTrue(a.CalcValid);
        }
    }
}
