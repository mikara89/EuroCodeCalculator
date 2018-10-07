using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using CalculatorEC2Logic;
using Moq;
using TabeleEC2.Model;
using TabeleEC2;
using System.Linq;
using CalculatorEC2Logic.SavijanjePreseka.EC;

namespace CalculatorEC2LogicTest.SavijanjePravougaonogPresekaEC2
{
    [TestClass]
    public class SavijanjePravougaonogPresekaEC2Test
    {
        private string expecResult = @"///Osnovni podaci///
b/h=25/40 cm;
d1=6 cm; d2=4 cm;
μSd=0.298; 
Beton:C25/30
Armatura:B500B
Msd=112.5 kNm; Nsd=67.5 kN;
///Izračunato///
Msds=121.95 kNm
εc=-3.5‰ εs1=2.681‰
μRd=0.298
x=19.25 cm2
As1_pot=8.47 cm2
As2_pot=1.44 cm2
μRd_lim=0.252
";
        private BetonModelEC beton = BetonClasses.GetBetonClassListEC().Single(b => b.name == "C25/30");
        private ReinforcementTypeModelEC armatura = ReinforcementType.GetArmatura().Single(b => b.name == "B500B");

        [TestMethod]
        public void SavijanjePravougaonogPresekaEC2V2Test_Msd()
        {
            string resulte;
            using (var obj = new SavijanjePravougaonogPresekaEC2V2(25, 40, 6, 4, beton, armatura, 112.5, 67.5, null))
            {
                resulte = obj.ToString();
                Assert.AreEqual(expecResult, resulte);
            }
        }

        [TestMethod]
        public void SavijanjePravougaonogPresekaEC2TestV2_MgIMq()
        {
            string resulte;
            using (var obj = new SavijanjePravougaonogPresekaEC2V2(25, 40, 6, 4, beton, armatura, 50, 30, 50, 0, null))
            {
                resulte = obj.ToString();
                Assert.AreEqual(expecResult, resulte);
            }
        }

        [TestMethod]
        public void SavijanjePravougaonogPresekaEC2_V2Test_Msd()
        {
            expecResult = @"//////Result///////
    Forces:
        Msd:        112.5kN
        Nsd:        67.5kNm
        Msds:       121.95kNm
    Material:
        Armatrua:   B500B; fyd: 43.48MPa; fyk: 50MPa; Es: 200GPa
        Beton:      C25/30; fcd: 14.17MPa; fck: 25MPa; Ecm: 31GPa
    Geometry:
        b:          25cm
        h:          40cm
        d1:         6cm
        d2:         4cm
        d:          34cm
    Result:
        εc/εs1:     -3.5‰/2.681‰
        μRd:        0.298
        x:          19.25 cm2
        As1_pot:    8.47 cm2 
        As2_pot:    1.44 cm2
        μRd_lim:    0.252";
            string resulte;
            var m = new Material() { beton = beton, armatura = armatura };
            var g = new ElementGeomety() { b = 25, h = 40, d1 = 6, d2 = 4 };
            var f = new ForcesBendingAndCompressison(112.5, 67.5);
            using (var obj = new SavijanjePravougaonogPresekaEC2_V2(f, g, m))
            {
                resulte = obj.ToString();
                Assert.AreEqual(expecResult, resulte);
            }
        }
    }
}
