using CalculatorEC2Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TabeleEC2;

namespace CalculatorEC2LogicTest
{
    [TestClass]
    public class SymmetricalReinfByClassicMethodUnitTest 
    {
        [TestMethod]
        [DataRow(0.5, 0.6, 0.4, 0.5)]
        [DataRow(0.3, 0.2, 0.2, 0.3)]
        [DataRow(0.9, 0.6, 0.9, 1.0)]
        [DataRow(0.2, 1.0, 0.2, 0.3)]
        public void SymmetricalReinfByClassicMethod_testting(double mi,double ni,double result1,double result2)
        {
            var material = new Material()
            {
                beton = TabeleEC2.BetonClasses.GetBetonClassListEC().First(n => n.name == "C25/30"),
                armatura = ReinforcementType.GetArmatura().First(n => n.name == "B500B"),
            };
            var geometry = new ElementGeomety()
            {
                b=20,
                d1=4,
                h=40,
            };
            var sym = new SymmetricalReinfByClassicMethod(material, geometry);
            
            var w= sym.Get_ω(mi, ni);
            Assert.IsTrue(result1<w && result2>w);
        }
    }
}
