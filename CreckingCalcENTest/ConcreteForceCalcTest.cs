﻿
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TabeleEC2.Model;
using InterDiagRCSection;
using System.Threading.Tasks;
using System;
using CalcModels;
using System.Diagnostics;

namespace CreckingCalcENTest
{ 
    [TestClass]
    public class ConcreteForceCalcTest
    {
        private static SectionStrainsFactory Section;


        [ClassInitialize]
        public static void Init(TestContext tc)
        {
            Section = new SectionStrainsFactory(
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

        [TestMethod]
        public async Task CalcRectTest()
        {
            Section = new SectionStrainsFactory(
               new Material
               {
                   beton = new BetonModelEC("C25/30"),
                   armatura = ReinforcementType.GetArmatura().Single(r => r.name == "B500B")
               }, new ElementGeometryWithReinfI
               {
                   b = 30,
                   h = 30,
                   d1 = 6,
                   d2 = 6,
                   As_1 = 6.8,
                   As_2 = 6.8
               }
           );

            var Calc = new CalcForces(new ConcreteForceCalc(Section));
            Section.SetByEcEs1(-2);
            var Fc= Calc.Calc(CalcForcesType.ConcreteForces);
            var Fs1 = Calc.Calc(CalcForcesType.ReinforcementForces1);
            var Fs2 = Calc.Calc(CalcForcesType.ReinforcementForces2);

            Assert.IsTrue(Fc.F.Round(2) == -1275 && Fc.Z.Round(2)==0.0);
            Assert.IsTrue(Fs1.F.Round(2) == -272.0 && Fs1.Z.Round(2) == 9.0);
            Assert.IsTrue(Fs2.F.Round(2) == -272.0 && Fs2.Z.Round(2) == 9.0);
        }
        [TestMethod]
        public async Task CalcTSectionTest()
        {
            Section = new SectionStrainsFactory(
               new Material
               {
                   beton = new BetonModelEC("C25/30"),
                   armatura = ReinforcementType.GetArmatura().Single(r => r.name == "B500B")
               }, new ElementGeometryWithReinfI
               {
                   b_eff_top=50,
                   h_f_top=8,
                   b = 30,
                   h = 30,
                   d1 = 6,
                   d2 = 6,
                   As_1 = 6.8,
                   As_2 = 6.8
               }
           );
            Stopwatch sw = new Stopwatch();

            var Calc = new CalcForces(new ConcreteForceCalc(Section));
            Section.SetByEcEs1(-2);
            var Fc = Calc.Calc(CalcForcesType.ConcreteForces);
            var Fs1 = Calc.Calc(CalcForcesType.ReinforcementForces1);
            var Fs2 = Calc.Calc(CalcForcesType.ReinforcementForces2);

            Assert.IsTrue(Fc.F.Round(2) == -1275 && Fc.Z.Round(2) == 0.0);
            Assert.IsTrue(Fs1.F.Round(2) == -272.0 && Fs1.Z.Round(2) == 9.0);
            Assert.IsTrue(Fs2.F.Round(2) == -272.0 && Fs2.Z.Round(2) == 9.0);
        }
    }
}
