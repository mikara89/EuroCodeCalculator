using CalculatorEC2Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CalculatorEC2LogicTest
{

    [TestClass]
    public class EfektivnaSirinaPresekaTest 
    {

        [TestMethod]
        public void EfetivnaSirinaPreseka_Oslonac1Return122_75() 
        {
            var b_l = 40.0;
            var b_r = 140.0;
            var pol = PolozajTpreseka.Oslonac_1;
            var l1 = 800;
            var b0 = 20;
            //var obj = new EfetivnaSirinaTPreseka(b_l,b_r,pol,l1,0,b0);           
            using (var obj = EfetivnaSirinaTPresekaFactory.CreateEfetivnaSirinaTPreseka(b_l, b_r, pol, l1, 0, b0))
            {
                var Result_b_eff = 122.75;

                Assert.AreEqual(Result_b_eff, obj.B_eff);
            }
            
        }
        [TestMethod]
        public void EfetivnaSirinaPreseka_Polje_1Return145() 
        {
            var b_l = 40.0;
            var b_r = 140.0;
            var pol = PolozajTpreseka.Polje_1; 
            var l1 = 800;
            var b0 = 20;
            //var obj = new EfetivnaSirinaTPreseka(b_l, b_r, pol, l1, 0, b0);
           
            using (var obj = EfetivnaSirinaTPresekaFactory.CreateEfetivnaSirinaTPreseka(b_l, b_r, pol, l1, 0, b0))
            {
                var Result_b_eff = 145;

                Assert.AreEqual(Result_b_eff, obj.B_eff);
            }
        }
        [TestMethod]
        public void EfetivnaSirinaPreseka_Oslonac2Return116_25()
        {
            var b_l = 40.0;
            var b_r = 140.0;
            var pol = PolozajTpreseka.Oslonac_2;
            var l1 = 800;
            var l2 = 1000;
            var b0 = 20;
            //var obj = new EfetivnaSirinaTPreseka(b_l, b_r, pol, l1, l2, b0);
            using (var obj = EfetivnaSirinaTPresekaFactory.CreateEfetivnaSirinaTPreseka(b_l, b_r, pol, l1, l2, b0))
            {
                var Result_b_eff = 116.25;

                Assert.AreEqual(Result_b_eff, obj.B_eff);
            }
        }
        [TestMethod]
        public void EfetivnaSirinaPreseka_Polje_3Return147_5()
        {
            var b_l = 40.0; 
            var b_r = 140.0;
            var pol = PolozajTpreseka.Polje_3;
            var l1 = 1000;
            var b0 = 20;
            //var obj = new EfetivnaSirinaTPreseka(b_l, b_r, pol, l1, 0, b0);
            using (var obj = EfetivnaSirinaTPresekaFactory.CreateEfetivnaSirinaTPreseka(b_l, b_r, pol, l1, 0, b0))
            {
                var Result_b_eff = 147.5;

                Assert.AreEqual(Result_b_eff, obj.B_eff);
            }
        }
        [TestMethod]
        public void EfetivnaSirinaPreseka_Oslonac_i_Polje4Return110()
        {
            var b_l = 40.0;
            var b_r = 140.0;
            var pol = PolozajTpreseka.Oslonac_i_Polje_4;
            var l1 = 200;
            var b0 = 20;
            //var obj = new EfetivnaSirinaTPreseka(b_l, b_r, pol, l1, 0, b0);
            using (var obj = EfetivnaSirinaTPresekaFactory.CreateEfetivnaSirinaTPreseka(b_l, b_r, pol, l1, 0, b0))
            {
                var Result_b_eff = 110;

                Assert.AreEqual(Result_b_eff, obj.B_eff);
            }
        }
    }
}
