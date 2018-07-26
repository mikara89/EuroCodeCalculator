using System;
using System.Collections.Generic;
using System.Text;

namespace CalculatorEC2Logic
{
    public enum PolozajTpreseka : int
    {
        Polje_1 = 0,
        Oslonac_1 = 1,
        Polje_3 = 2,
        Oslonac_2 = 3,
        Oslonac_i_Polje_4 = 4,

    }
    public class EfetivnaSirinaTPreseka : IDisposable
    {
        public double B_Left { get; }
        public double B_Right { get; }
        public double Be_Left { get; internal set; }
        public double Be_Right { get; internal set; }
        public PolozajTpreseka Polozaj { get; }
        public double L_1 { get; }
        public double L_2 { get; }
        public double B_0 { get; }
        public double B_eff { get; internal set; }
        public double L_e { get; internal set; }

        public EfetivnaSirinaTPreseka(double b_left, double b_right, PolozajTpreseka polozaj, double l_1, double l_2 = 0, double b_0 = 0)
        {
            B_Left = b_left;
            B_Right = b_right;
            Polozaj = polozaj;
            L_1 = l_1;
            L_2 = l_2;
            B_0 = b_0;

            Start();
        }
        public EfetivnaSirinaTPreseka(double b_full, PolozajTpreseka polozaj, double l_1, double l_2 = 0, double b_0 = 0)
        {
            B_Left = (b_full - b_0) / 2;
            B_Right = (b_full - b_0) / 2;
            Polozaj = polozaj;
            L_1 = l_1;
            L_2 = l_2;
            B_0 = b_0;

            Start();
        }

        private void Start()
        {
            OdredjivanjeLe();
            OdredjivanjeKorisneSirineLevoIDesno();
            OdredjivanjeBEfektivno();
        }
        private void OdredjivanjeLe()
        {
            switch (Polozaj)
            {
                case PolozajTpreseka.Polje_1:
                case PolozajTpreseka.Oslonac_1:
                    L_e = 0.85 * L_1;
                    break;
                case PolozajTpreseka.Oslonac_2:
                    if (L_2 == 0)
                        throw new Exception("L2 ne sme biti 0!");
                    L_e = 0.25 * (L_1 + L_2);
                    break;
                case PolozajTpreseka.Polje_3:
                    L_e = 0.70 * L_1;
                    break;
                case PolozajTpreseka.Oslonac_i_Polje_4:
                    L_e = 2 * L_1;
                    break;
                default:
                    throw new Exception("Tip preseka nije definisan");
            }
        }
        private void OdredjivanjeKorisneSirineLevoIDesno()
        {
            Be_Left = L_e / 8 <= B_Left ? L_e / 8 : B_Left;
            Be_Right = L_e / 8 <= B_Right ? L_e / 8 : B_Right;
        }
        public double Beta(double Be_LorR)
        {
            switch (Polozaj)
            {
                case PolozajTpreseka.Oslonac_1:
                    return (0.55 + 0.025 * L_e / Be_LorR);
                case PolozajTpreseka.Polje_1:
                case PolozajTpreseka.Polje_3:
                case PolozajTpreseka.Oslonac_2:
                case PolozajTpreseka.Oslonac_i_Polje_4:
                    return 1;
                default:
                    throw new Exception("Tip preseka nije definisan");
            }
        }
        private void OdredjivanjeBEfektivno()
        {
            B_eff = B_0 + Be_Left * Beta(Be_Left) + Be_Right * Beta(Be_Right);
        }

        public override string ToString()
        {
            return String.Format(@"///Osnovni Podaci///
b levo={0}cm
b desno={1}cm
b0={2}cm
L1={3}cm
L2={4}cm
Polozaj preseka={5}
///Obracun///
b effektivno={6}cm
L_e={7}cm 
be levo={8}cm
be desno={9}cm
", B_Left, B_Right, B_0, L_1, L_2,
Enum.GetName(typeof(PolozajTpreseka), Polozaj),
Round(B_eff), Round(L_e), Round(Be_Left), Round(Be_Right));
        }

        private double Round(double d, int i = 2)
        {
            return Math.Round(d, i);
        }


        public void Dispose()
        {
            GC.Collect();
        }
    } 

    public abstract class EfetivnaSirinaTPresekaV2:IDisposable  
    {
        public  double B_Left { get; internal set; }
        public double B_Right { get; internal set; }
        public double Be_Left { get; internal set; }
        public double Be_Right { get; internal set; }
        public PolozajTpreseka Polozaj { get; internal set; }
        public double L_1 { get; internal set; }
        public double L_2 { get; internal set; }
        public double B_0 { get; internal set; }
        public double B_eff { get; internal set; }
        public double L_e { get; internal set; }

        public abstract double Beta(double Be_LorR);
        public abstract void OdredjivanjeLe();

        protected void Start()
        {
            OdredjivanjeLe();
            OdredjivanjeKorisneSirineLevoIDesno();
            OdredjivanjeBEfektivno();
        }
        

        private void OdredjivanjeKorisneSirineLevoIDesno()
        {
            Be_Left = L_e / 8 <= B_Left ? L_e / 8 : B_Left;
            Be_Right = L_e / 8 <= B_Right ? L_e / 8 : B_Right;
        }
        

        private void OdredjivanjeBEfektivno()
        {
            B_eff = B_0 + Be_Left * Beta(Be_Left) + Be_Right * Beta(Be_Right);
        }

        public override string ToString()
        {
            return String.Format(@"///Osnovni Podaci///
b levo={0}cm
b desno={1}cm
b0={2}cm
L1={3}cm
L2={4}cm
Polozaj preseka={5}
///Obracun///
b effektivno={6}cm
L_e={7}cm 
be levo={8}cm
be desno={9}cm
", B_Left, B_Right, B_0, L_1, L_2,
Enum.GetName(typeof(PolozajTpreseka), Polozaj),
Round(B_eff), Round(L_e), Round(Be_Left), Round(Be_Right));
        }

        protected double Round(double d, int i = 2)
        {
            return Math.Round(d, i);
        }


        public void Dispose()
        {
            GC.Collect();
        }
    }

    public class EfetivnaSirinaTPresekaOslonac1 : EfetivnaSirinaTPresekaV2
    {
        public EfetivnaSirinaTPresekaOslonac1(double b_left, double b_right, PolozajTpreseka polozaj, double l_1, double b_0 = 0)
        {

            B_Left = b_left;
            B_Right = b_right;
            Polozaj = polozaj;
            L_1 = l_1;
            B_0 = b_0;

            Start();
        }
        public EfetivnaSirinaTPresekaOslonac1(double b_full, PolozajTpreseka polozaj, double l_1, double l_2 = 0, double b_0 = 0)
        {
            B_Left = (b_full - b_0) / 2;
            B_Right = (b_full - b_0) / 2;
            Polozaj = polozaj;
            L_1 = l_1;
            L_2 = l_2;
            B_0 = b_0;

            Start();
        }

        public override void OdredjivanjeLe()=> L_e = 0.85 * L_1;

        public override double Beta(double Be_LorR)=> (0.55 + 0.025 * L_e / Be_LorR);

        public override string ToString()
        {
            return String.Format(@"///Osnovni Podaci///
b levo={0}cm
b desno={1}cm
b0={2}cm
L1={3}cm
Polozaj preseka={5}
///Obracun///
b effektivno={6}cm
L_e={7}cm 
be levo={8}cm
be desno={9}cm
", B_Left, B_Right, B_0, L_1, L_2,
Enum.GetName(typeof(PolozajTpreseka), Polozaj),
Round(B_eff), Round(L_e), Round(Be_Left), Round(Be_Right));
        }

    }
    public class EfetivnaSirinaTPresekaPolje1 : EfetivnaSirinaTPresekaV2
    {
        public EfetivnaSirinaTPresekaPolje1(double b_left, double b_right, PolozajTpreseka polozaj, double l_1, double b_0 = 0)
        {

            B_Left = b_left;
            B_Right = b_right;
            Polozaj = polozaj;
            L_1 = l_1;
            B_0 = b_0;

            Start();
        }
        public EfetivnaSirinaTPresekaPolje1(double b_full, PolozajTpreseka polozaj, double l_1, double b_0 = 0)
        {
            B_Left = (b_full - b_0) / 2;
            B_Right = (b_full - b_0) / 2;
            Polozaj = polozaj;
            L_1 = l_1;
            B_0 = b_0;

            Start();
        } 

        public override void OdredjivanjeLe() => L_e = 0.85 * L_1;

        public override double Beta(double Be_LorR) => 1;

        public override string ToString()
        {
            return String.Format(@"///Osnovni Podaci///
b levo={0}cm
b desno={1}cm
b0={2}cm
L1={3}cm
Polozaj preseka={5}
///Obracun///
b effektivno={6}cm
L_e={7}cm 
be levo={8}cm
be desno={9}cm
", B_Left, B_Right, B_0, L_1, L_2,
Enum.GetName(typeof(PolozajTpreseka), Polozaj),
Round(B_eff), Round(L_e), Round(Be_Left), Round(Be_Right));
        }

    }
    public class EfetivnaSirinaTPresekaOslonac2 : EfetivnaSirinaTPresekaV2
    {
        public EfetivnaSirinaTPresekaOslonac2(double b_left, double b_right, PolozajTpreseka polozaj, double l_1,double l_2, double b_0 = 0)
        {

            B_Left = b_left;
            B_Right = b_right;
            Polozaj = polozaj;
            L_1 = l_1;
            L_2 = l_2;
            B_0 = b_0;

            Start();
        }
        public EfetivnaSirinaTPresekaOslonac2(double b_full, PolozajTpreseka polozaj, double l_1, double l_2, double b_0 = 0)
        {
            B_Left = (b_full - b_0) / 2;
            B_Right = (b_full - b_0) / 2;
            Polozaj = polozaj;
            L_1 = l_1;
            L_2 = L_2;
            B_0 = b_0;

            Start();
        }

        public override void OdredjivanjeLe() => L_e = 0.25 * (L_1 + L_2);

        public override double Beta(double Be_LorR) => 1;

        public override string ToString()
        {
            return String.Format(@"///Osnovni Podaci///
b levo={0}cm
b desno={1}cm
b0={2}cm
L1={3}cm
L2={4}cm
Polozaj preseka={5}
///Obracun///
b effektivno={6}cm
L_e={7}cm 
be levo={8}cm
be desno={9}cm
", B_Left, B_Right, B_0, L_1, L_2,
Enum.GetName(typeof(PolozajTpreseka), Polozaj),
Round(B_eff), Round(L_e), Round(Be_Left), Round(Be_Right));
        }

    }
    public class EfetivnaSirinaTPresekaPolje3 : EfetivnaSirinaTPresekaV2
    {
        public EfetivnaSirinaTPresekaPolje3(double b_left, double b_right, PolozajTpreseka polozaj, double l_1, double b_0 = 0)
        {

            B_Left = b_left;
            B_Right = b_right;
            Polozaj = polozaj;
            L_1 = l_1;
            B_0 = b_0;

            Start();
        }
        public EfetivnaSirinaTPresekaPolje3(double b_full, PolozajTpreseka polozaj, double l_1, double b_0 = 0)
        {
            B_Left = (b_full - b_0) / 2;
            B_Right = (b_full - b_0) / 2;
            Polozaj = polozaj;
            L_1 = l_1;
            B_0 = b_0;

            Start();
        }

        public override void OdredjivanjeLe() => L_e = 0.70 * L_1;

        public override double Beta(double Be_LorR) => 1;

        public override string ToString()
        {
            return String.Format(@"///Osnovni Podaci///
b levo={0}cm
b desno={1}cm
b0={2}cm
L1={3}cm
Polozaj preseka={5}
///Obracun///
b effektivno={6}cm
L_e={7}cm 
be levo={8}cm
be desno={9}cm
", B_Left, B_Right, B_0, L_1, L_2,
Enum.GetName(typeof(PolozajTpreseka), Polozaj),
Round(B_eff), Round(L_e), Round(Be_Left), Round(Be_Right));
        }

    }
    public class EfetivnaSirinaTPresekaOslonacIPolje4 : EfetivnaSirinaTPresekaV2
    {
        public EfetivnaSirinaTPresekaOslonacIPolje4(double b_left, double b_right, PolozajTpreseka polozaj, double l_1, double b_0 = 0)
        {

            B_Left = b_left;
            B_Right = b_right;
            Polozaj = polozaj;
            L_1 = l_1;
            B_0 = b_0;
             
            Start();
        }
        public EfetivnaSirinaTPresekaOslonacIPolje4(double b_full, PolozajTpreseka polozaj, double l_1, double l_2 = 0, double b_0 = 0)
        {
            B_Left = (b_full - b_0) / 2;
            B_Right = (b_full - b_0) / 2;
            Polozaj = polozaj;
            L_1 = l_1;
            L_2 = l_2;
            B_0 = b_0;

            Start();
        }

        public override void OdredjivanjeLe() => L_e = 2 * L_1;

        public override double Beta(double Be_LorR) => 1;

        public override string ToString()
        {
            return String.Format(@"///Osnovni Podaci///
b levo={0}cm
b desno={1}cm
b0={2}cm
L1={3}cm
Polozaj preseka={5}
///Obracun///
b effektivno={6}cm
L_e={7}cm 
be levo={8}cm
be desno={9}cm
", B_Left, B_Right, B_0, L_1, L_2,
Enum.GetName(typeof(PolozajTpreseka), Polozaj),
Round(B_eff), Round(L_e), Round(Be_Left), Round(Be_Right));
        }

    }

    public static class EfetivnaSirinaTPresekaFactory
    {
        public static EfetivnaSirinaTPresekaV2 CreateEfetivnaSirinaTPreseka(
            double b_left,
            double b_right,
            PolozajTpreseka polozaj, 
            double l_1,
            double l_2 = 0, 
            double b_0 = 0)
        {
            switch (polozaj)
            {
                case PolozajTpreseka.Oslonac_1:
                    return new EfetivnaSirinaTPresekaOslonac1(b_left, b_right, polozaj, l_1, b_0);
                case PolozajTpreseka.Polje_1:
                    return new EfetivnaSirinaTPresekaPolje1(b_left, b_right, polozaj, l_1, b_0);
                case PolozajTpreseka.Oslonac_2:
                    return new EfetivnaSirinaTPresekaOslonac2(b_left, b_right, polozaj, l_1, l_2, b_0);
                case PolozajTpreseka.Polje_3:
                    return new EfetivnaSirinaTPresekaPolje3(b_left, b_right, polozaj, l_1, b_0);
                case PolozajTpreseka.Oslonac_i_Polje_4:
                    return new EfetivnaSirinaTPresekaOslonacIPolje4(b_left, b_right, polozaj, l_1, b_0);
                default:
                    throw new ArgumentException("Tip preseka nije definisan");
            }
            
        }

        public static EfetivnaSirinaTPresekaV2 CreateEfetivnaSirinaTPreseka(
            double b_full,
            PolozajTpreseka polozaj,
            double l_1,
            double l_2 = 0,
            double b_0 = 0)
        {
            switch (polozaj)
            {
                case PolozajTpreseka.Oslonac_1:
                    return new EfetivnaSirinaTPresekaOslonac1(b_full, polozaj, l_1, b_0);
                case PolozajTpreseka.Polje_1:
                    return new EfetivnaSirinaTPresekaPolje1(b_full, polozaj, l_1, b_0);
                case PolozajTpreseka.Oslonac_2:
                    return new EfetivnaSirinaTPresekaOslonac2(b_full, polozaj, l_1, l_2, b_0);
                case PolozajTpreseka.Polje_3:
                    return new EfetivnaSirinaTPresekaPolje3(b_full, polozaj, l_1, b_0);
                case PolozajTpreseka.Oslonac_i_Polje_4:
                    return new EfetivnaSirinaTPresekaOslonacIPolje4(b_full, polozaj, l_1, b_0);
                default:
                    throw new ArgumentException("Tip preseka nije definisan");
            }
        }
    }
}
