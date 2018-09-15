using System;
using System.Collections.Generic;
using System.Text;

namespace CalculatorEC2Logic
{ 
    public enum Izvijanja : int
    {
        Pokretan_I_Nepokretan = 0,
        Pokretan_I_Ukljeste = 1,
        Ukljesten_Sa_Obe = 2,
        Ukljesten_Sa_Jedne = 3
    }

    public static class  OjleroviSlucajeviIzvijanja
    {
        public const double Pokretan_I_Nepokretan = 1;
        public const double Pokretan_I_Ukljesten = 0.707;
        public const double Ukljesten_Sa_Obe = 0.5;
        public const double Ukljesten_Sa_Jedne = 2;



        public static double GetK(Izvijanja? izvijanje)
        {
            switch (izvijanje)
            {
                case Izvijanja.Pokretan_I_Nepokretan:
                    return Pokretan_I_Nepokretan;
                case Izvijanja.Pokretan_I_Ukljeste:
                    return Pokretan_I_Ukljesten;
                case Izvijanja.Ukljesten_Sa_Jedne:
                    return Ukljesten_Sa_Jedne;
                case Izvijanja.Ukljesten_Sa_Obe:
                    return Ukljesten_Sa_Obe;
                case null:
                    return 0;
            }

            return 0;
        }
    
    }
}
