using System;
using CalculatorEC2Logic;
using TabeleEC2;
using System.Linq;
using TabeleEC2.Model;
using System.Diagnostics;
using static CalculatorEC2Logic.SymmetricalReinfByMaxAndMinPercentageReinf;

namespace EuroCodeCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            var beton = BetonClasses.GetBetonClassListEC().Single(b => b.name == "C25/30");
            var armatura = ReinforcementType.GetArmatura().Single(b => b.name == "B500B");
            var m = new Material() { beton = beton, armatura = armatura };
            var g = new ElementGeomety() { b = 25, h = 40, d1 = 6, d2 = 4 };
            var f = new ForcesBendingAndCompressison(112.5, 67.5);
            using (var obj = new SavijanjePravougaonogPresekaEC2_V2(f, g, m))
            {
                Console.WriteLine(obj.ToString());
            }
            Console.ReadKey();
        }
        private static void testSteper()
        {

            var beton = TabeleEC2.BetonClasses.GetBetonClassListEC().First(n => n.name == "C25/30");
            var arm = ReinforcementType.GetArmatura().First(n => n.name == "B500B");
            //var b = new SavijanjePravougaonogPresekaEC2_V2(25,40,4,4, beton,arm,30,20,20);
            var b = new CalculatorEC2Logic.SavijanjePreseka.EC2.SavijanjePravougaonogPresekaEC2_V2(25, 40, 6, 6, beton, arm, 50, 30, 50);
            b.Calc();
            foreach (var item in b.Steper.Steps)
            {
                Console.WriteLine("Step number " + item.StepNumber);
                Console.WriteLine("--" + item.Description + "--");
                Console.WriteLine("  " + item.StepText);
                Console.WriteLine();


            }
            Console.ReadKey();
        }
        private static void ec2_TacnPostupak()
        {
            double Msd = 180;
            double Mu;
            double Du = 0;
            double b_eff = 20;
            double b_w = 20;
            double h_f = 15;
            double d = 44;
            double s;
            double δ = h_f / d;
            double Nu = 0;
            double bi = 0;

            BetonModelEC beton = BetonClasses.GetBetonClassListEC().Single(b => b.name == "C25/30");
            ReinforcementTypeModelEC arm = ReinforcementType.GetArmatura().Single(a => a.name == "B500B");
            //var fcd = 2.05;
            double As = (Du - Nu) / arm.fyd;

            KofZaProracunPravougaonogPresekaModelEC kof1 = new KofZaProracunPravougaonogPresekaModelEC();
            KofZaProracunPravougaonogPresekaModelEC kof2 = new KofZaProracunPravougaonogPresekaModelEC();

            bool done = false;
            s = δ;
            int i = 0;
            var x = 0.0;
            var s_add = 0.1;
            var μSd = (Msd*100) / (b_eff * Math.Pow(d, 2) * beton.fcd / 10);
            var kof_test = KofZaProracunPravougaonogPresekaEC.Get_Kof_From_μ(μSd);
            if (kof_test.ξ <= s)
            {
                var kof = kof_test;
                x = kof.ξ * d;
                As= (Msd * 100) / (kof.ζ * d * arm.fyd);
            }
            do
            {
                i++;
                kof1.SetByξ(s);
                x = s * d;
                var Du1 = kof1.αv * b_eff * s * d * beton.fcd / 10;
                var zb1 = d * (1 - kof1.ka * s);
                var Ebd = ((s - δ) / s) * kof1.εc;
                kof2.SetByEcEs1(Ebd, 20);

                var Du2 = kof2.αv * (b_eff - b_w) * (x - h_f) * beton.fcd / 10;
                var zb2 = d - h_f - kof2.ka * (x - h_f);

                Mu = Du1 * zb1 / 100 - Du2 * zb2 / 100;

                if(i > 30) break;

           
                    if (Mu * 1.0005 < Msd) { s += s_add; continue; }
                    if (Mu > Msd * 1.001) { s_add = s_add / 2; s -= s_add; continue; }

                if (Mu * 1.0005 >= Msd && Mu <= Msd * 1.001) { Du = Du1 - Du2; As = (Du - Nu) / arm.fyd; done = true; }

            } while (!done);
            if(i>30 && done == false)
            {
                Console.WriteLine("Can't calculate!");
                Console.ReadKey();
                return;
            }
            bi = Mu / (kof1.μRd * Math.Pow(d, 2) * beton.fcd / 10);
            As = (Du - Nu) / arm.fyd;
            x = kof1.ξ * d;
            Console.WriteLine("n= "+i);
            Console.WriteLine("As= "+As);
            Console.WriteLine("x= " + x);
            Console.WriteLine("Mu= " + Mu);
            Console.ReadKey();

            GC.Collect();
        }

        private static void PBAB_TacnPostupak() 
        {
            double Msd = 770;
            double Mu;
            double Du;
            double b_eff = 60;
            double b_w = 30;
            double h_f = 10;
            double d = 51;
            double s;
            double δ = h_f / d;
            //BetonModel beton = BetonClasses.GetBetonClassList().Single(b => b.name == "C25/30");
            ReinforcementTypeModelEC arm = ReinforcementType.GetArmatura().Single(a => a.name == "B500B");
            var fcd = 2.05;

            KofZaProracunPravougaonogPresekaModelEC kof1 = new KofZaProracunPravougaonogPresekaModelEC();
            KofZaProracunPravougaonogPresekaModelEC kof2 = new KofZaProracunPravougaonogPresekaModelEC();

            bool done = false;
            s = δ;
            int i = 0;
            var s_add = 0.1;
            do
            {
                i++;
                //kof1.SetByS_PBAB(s);
                var x = s * d;
                var Du1 = kof1.αv * b_eff * s * d * fcd;
                var zb1 = d * (1 - kof1.ka * s);
                var Ebd = ((s - δ) / s) * kof1.εc;
                kof2.SetByEcEs1(Ebd, 10);

                var Du2 = kof2.αv * (b_eff - b_w) * (x - h_f) * fcd;
                var zb2 = d - h_f - kof2.ka * (x - h_f);

                Mu = Math.Round(Du1 * zb1 / 100 - Du2 * zb2 / 100, 2);

                if (Mu < Msd) { s += s_add; continue; }
                //if (Mu > Msd) { s -= 0.09; continue; }
                if (Mu > Math.Round(Msd * 1.001, 2)) { s_add = s_add / 2; s -= s_add; continue; }
                if (Mu >= Msd && Mu <= Math.Round(Msd * 1.001, 2)) { done = true; }

            } while (!done);


            Console.WriteLine(i);
            Console.ReadKey();
            GC.Collect();
        }
       
        private static void EC_T_Presek_PomocuFormule() 
        {
            double Msd =6100;
            double b_eff = 175;
            double b_w = 35;
            double h_f = 15;
            double d = 143;
            double s;
            double δ = h_f / d;
            var μSd = 0.0; 
            var bi = b_eff;

            BetonModelEC beton = BetonClasses.GetBetonClassListEC().Single(b => b.name == "C25/30");
            ReinforcementTypeModelEC arm = ReinforcementType.GetArmatura().Single(a => a.name == "B500B");
            var fcd = beton.fcd/10;

            KofZaProracunPravougaonogPresekaModelEC kof1 = new KofZaProracunPravougaonogPresekaModelEC();
            KofZaProracunPravougaonogPresekaModelEC kof2 = new KofZaProracunPravougaonogPresekaModelEC();
            KofZaProracunPravougaonogPresekaModelEC kof_zvezdica = new KofZaProracunPravougaonogPresekaModelEC();
             
            //kof1.
            bool done = false;
            s = δ;
            kof1.SetByξ(s);
            kof2 = kof1;
            int i = 0;
            //var s_add = 0.1;
            do
            {
                var x = kof2.ξ * d;
                var Eb = ((x - h_f) / x) * kof2.εc;
                kof_zvezdica.SetByEcEs1(Eb, 20);
                if (kof2.εc == 0)///na pocetku ce uvek biti kof2 0;
                    bi = 1 * b_eff;
                else bi = (1 - (kof_zvezdica.αv / kof1.αv) * (1 - (h_f / (kof2.ξ * d))) * (1 - (b_w / b_eff))) * b_eff;


                μSd = Msd * 100 / (bi * Math.Pow(d, 2) * fcd);
                var kof3 = KofZaProracunPravougaonogPresekaEC.Get_Kof_From_μ(μSd); 
                if (Math.Round(kof2.ξ,3) == Math.Round(kof3.ξ,3))
                {
                    done = true;
                    continue;
                }
                kof2 = kof3;
            } while (!done);

        }
        private static void EfSirPres()
        {
            
            //var e = new KofZaProracunPravougaonogPresekaModel();
            //e.SetByS(15.0 / 143.0);
            //var a = KofZaProracunPravougaonogPreseka.GetItem_Full(0.189, 0);//predpostavljemo
            //                                                                //var b = KofZaProracunPravougaonogPreseka.GetItem_Full(0.104, 0);
            //double lamda_b1 = 1 - (e.αv / a.αv) * (1 - (15 / (a.ξ * 143)) * (1 - (35 / 175)));

            Console.WriteLine("Oslonac A");
            using (var p = EfetivnaSirinaTPresekaFactory.CreateEfetivnaSirinaTPreseka(40, 140, PolozajTpreseka.Oslonac_1, l_1: 800, l_2: 0, b_0: 20))
            {
                Console.WriteLine(p.ToString());
                Console.WriteLine("");
            }
            Console.WriteLine("Polje A-B");
            using (var p = EfetivnaSirinaTPresekaFactory.CreateEfetivnaSirinaTPreseka(40, 140, PolozajTpreseka.Polje_1, l_1: 800, l_2: 0, b_0: 20))
            {
                Console.WriteLine(p.ToString());
                Console.WriteLine("");
            }
            Console.WriteLine("Oslonac B");
            using (var p = EfetivnaSirinaTPresekaFactory.CreateEfetivnaSirinaTPreseka(40, 140, PolozajTpreseka.Oslonac_2, l_1: 800, l_2: 1000, b_0: 20))
            {
                Console.WriteLine(p.ToString());
                Console.WriteLine("");
            }
            Console.WriteLine("Polje B-C");
            using (var p = EfetivnaSirinaTPresekaFactory.CreateEfetivnaSirinaTPreseka(40, 140, PolozajTpreseka.Polje_3, l_1: 1000, l_2: 0, b_0: 20))
            {
                Console.WriteLine(p.ToString());
                Console.WriteLine("");
            }
            Console.WriteLine("Oslonac C");
            using (var p = EfetivnaSirinaTPresekaFactory.CreateEfetivnaSirinaTPreseka(40, 140, PolozajTpreseka.Oslonac_i_Polje_4, l_1: 200, l_2: 0, b_0: 20))
            {
                Console.WriteLine(p.ToString());
                Console.WriteLine("");
            }
        }
        private static void EfSirPres1()
        {

            //var e = new KofZaProracunPravougaonogPresekaModel();
            //e.SetByS(15.0 / 143.0);
            //var a = KofZaProracunPravougaonogPreseka.GetItem_Full(0.189, 0);//predpostavljemo
            //                                                                //var b = KofZaProracunPravougaonogPreseka.GetItem_Full(0.104, 0);
            //double lamda_b1 = 1 - (e.αv / a.αv) * (1 - (15 / (a.ξ * 143)) * (1 - (35 / 175)));

            Console.WriteLine("Start");
            using (var p = EfetivnaSirinaTPresekaFactory.CreateEfetivnaSirinaTPreseka(250, 250, PolozajTpreseka.Polje_1, l_1: 712, l_2: 0, b_0: 20))
            {
                Console.WriteLine(p.ToString());
                Console.WriteLine("");
            }
        }

        private double Getω(KofZaProracunPravougaonogPresekaModelEC Kof, double Vsd, double b, double d1, double h, BetonModelEC beton)
        {

            var zs = (h / 2 - d1);

            var Kof_lim = KofZaProracunPravougaonogPresekaEC.GetLimitKofZaProracunPravougaonogPresekaEC(beton);

            var Mrd_limit = (Kof_lim.μRd * b * Math.Pow((h - d1), 2) * beton.fcd / 10) / 100;

            var ω1 = Kof_lim.μRd * ((1 / Kof_lim.ζ) - (1 / 0.89)) + Kof.μRd * 1 / 0.89 + Vsd * (zs / (0.89 * (h - d1)) - 1);

            return ω1;
        }
        private static void test22()
        {
            var g = new ElementGeometySlenderness() {
                b = 25,
                h = 25,
                k = OjleroviSlucajeviIzvijanja.GetK(Izvijanja.Pokretan_I_Ukljeste),
                d1=4,
                L=400,
            };
            var f = new ForcesSlenderness(g.li,g.h)
            {
                NEd=85.725,
                M_top=0,
                M_bottom=0,
            };
            var m = new Material()
            {
                beton = TabeleEC2.BetonClasses.GetBetonClassListEC().First(n => n.name == "C30/37"),
                armatura = ReinforcementType.GetArmatura().First(n => n.name == "B500B"),
            };
            var v = new VitkostEC2_V2(g,f,m);

            v.Calculate();
            v.KontrolaCentPritPreseka();
            v.ProracunArmature();
                
        }
        private static void test11()
        {
            var g = new ElementGeometySlenderness()
            {
                b = 25,
                h = 25,
                k = OjleroviSlucajeviIzvijanja.GetK(Izvijanja.Pokretan_I_Ukljeste),
                d1 = 4,
                L = 800,
            };
            var f = new ForcesSlenderness(g.li,g.h)
            {
                NEd = 100,
                //M_top = -50,
                //M_bottom = 50,
            };
            var m = new Material()
            {
                beton = TabeleEC2.BetonClasses.GetBetonClassListEC().First(n => n.name == "C25/30"),
                armatura = ReinforcementType.GetArmatura().First(n => n.name == "B500B"),
            };
            var v = new VitkostEC2_V2(g, f, m);
            //v.φ_ef = 3;
            v.Calculate();
            v.KontrolaCentPritPreseka();
            v.ProracunArmature();

        }
        private static void test12()
        {
            var g = new ElementGeometySlenderness()
            {
                b = 30 ,
                h = 40,
                k = OjleroviSlucajeviIzvijanja.GetK(Izvijanja.Pokretan_I_Ukljeste),
                d1 = 4,
                L = 350,
            };
            var f = new ForcesSlenderness(g.li, g.h)
            {
                NEd = 457.5,
                M_top = 20.25,
                //M_bottom = 50,
            };
            var m = new Material()
            {
                beton = TabeleEC2.BetonClasses.GetBetonClassListEC().First(n => n.name == "C25/30"),
                armatura = ReinforcementType.GetArmatura().First(n => n.name == "B500B"),
            };
            var v = new VitkostEC2_V2(g, f, m);
            //v.φ_ef = 3;
            v.Calculate();
            v.KontrolaCentPritPreseka();
            v.ProracunArmature();

        }
        private static void testqq()
        {
            var g = new ElementGeomety()
            {
                b = 60,
                d1 = 4,
                h = 60,
            };
            var m = new Material()
            {
                beton = TabeleEC2.BetonClasses.GetBetonClassListEC().First(n => n.name == "C30/37"),
                armatura = ReinforcementType.GetArmatura().First(n => n.name == "B500B"),
            };
            var s1 = new SymmetricalReinfByMaxAndMinPercentageReinf(m,g);

            var r1= s1.Get_ρ(862.12, 569.7, g.b, g.h);
            var As1 = (r1 * g.b * g.h)/2;
            var p1 = Math.Round(r1 * 100, 2);
            var s2 = new SymmetricalReinfByClassicMethod(m,g);
            var r2 = s2.Get_ω2(862.12, 569.7);
            var As2 = r2 * g.b * g.d*m.beton.fcd/10/m.armatura.fyd;
            var p2 = Math.Round(As2 / g.b / g.h*2*100,2);
            Console.WriteLine($"{Math.Round(As1, 2)}({p1}%)cm2 == {Math.Round(As2, 2)}({p2}%)cm2");
            Console.ReadKey();
        }
        private static void test()
        {
            var m = new Material()
            {
                beton = TabeleEC2.BetonClasses.GetBetonClassListEC().First(n => n.name == "C30/37"),
                armatura = ReinforcementType.GetArmatura().First(n => n.name == "B500B"),
            };
            var s = new Generate_ρ_LineForDiagram(m);

            var r = s.ListOfDotsInLineOfDiagram;

        }
        private static void testAs() 
        {
            var g = new ElementGeomety()
            {
                b = 60,
                d1 = 4,
                h = 60,
            };
            var m = new Material()
            {
                beton = TabeleEC2.BetonClasses.GetBetonClassListEC().First(n => n.name == "C30/37"),
                armatura = ReinforcementType.GetArmatura().First(n => n.name == "B500B"),
            };
            m.beton.α = 1;
            var s1 = new SymmetricalReinfByClassicMethod(m, g);

            //var As1 = s1.Get_As(47.8, 1620); 
            //var w= s1.Get_ω2(47.8, 1620);
            var As1 = s1.Get_As(2204.74, 569.7);
            var w = s1.Get_ω2(2204.74, 569.7);
            var As2= g.b * (g.h - g.d1) * w * m.beton.fcd / (m.armatura.fyd * 10);
            Console.WriteLine($"{As1} == {As2}");
            Console.ReadKey();

        }
    }
}



