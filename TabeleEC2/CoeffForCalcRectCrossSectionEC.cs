using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using TabeleEC2.Model;

namespace TabeleEC2
{

    public static class CoeffForCalcRectCrossSectionEC
    {
        
        public static CoeffForCalcRectCrossSectionModelEC GetLimitCoeff(BetonModelEC beton,double ni=1)
        {
            if (beton.fck <= 50)
            {
                return new CoeffForCalcRectCrossSectionModelEC(-3.5, 4.278,ni) ;
            }else return new CoeffForCalcRectCrossSectionModelEC(-3.5, 6.5, ni);
        }

        public static List<CoeffForCalcRectCrossSectionModelEC> GetList(double ni=1)
        {
            List<CoeffForCalcRectCrossSectionModelEC> result= new List<CoeffForCalcRectCrossSectionModelEC>();
            for (double i = -0.1; i >= -3.5; i+=-0.1)
            {
                result.Add(new CoeffForCalcRectCrossSectionModelEC(i, 20, ni));
            }
            for (double i = 0.5; i < 20; i+=0.5)
            {
                result.Add(new CoeffForCalcRectCrossSectionModelEC(-3.5, i, ni));
            }
            result.Add(new CoeffForCalcRectCrossSectionModelEC(-3.5, 20, ni));
            return result.OrderBy(n=>n.μRd).ToList();
        }
        /// <summary>
        /// Msd * 100 / (b * Math.Pow(d, 2) * fcd)
        /// </summary>
        /// <param name="Msd">in kN/m</param>
        /// <param name="b">in cm</param>
        /// <param name="d">in cm</param>
        /// <param name="fcd">in kN/cm</param> 
        /// <returns></returns>
        public static double GetμSd(double Msd, double b, double d, double fcd, double ni=1)
        {
            var result= Msd * 100 / (b * Math.Pow(d, 2) * fcd);
            var max = new CoeffForCalcRectCrossSectionModelEC(-3.5,0.5,ni).μRd;
            var min = new CoeffForCalcRectCrossSectionModelEC(-0.1, 20,ni).μRd;
            if (result > max)
                return max;
                //throw new Exception("Diletacija u armaturi i betonu prekoracema; \nPovecajte presek!");
            if (result < min)
                throw new Exception("Diletacija u armaturi i betonu prekoracema; \nPovecajte presek!");
            return result;
        }

        public static CoeffForCalcRectCrossSectionModelEC Get_Kof_From_μ(double μSd,double ni=1, int percision=4)
        {
            var μ_lim = new CoeffForCalcRectCrossSectionModelEC(ni);
            μ_lim.SetByEcEs1(-3.5, 20);
            CoeffForCalcRectCrossSectionModelEC kofResult =new CoeffForCalcRectCrossSectionModelEC(ni);

            if (μSd > μ_lim.μRd)
            {
                var max_εs1 = 20.0;
                var min_εs1 = 0.0;

                var test_εs1 = (max_εs1 + min_εs1) / 2; 
                var adder_εs1 = test_εs1;


                while (μSd.Round(percision) != kofResult.μRd.Round(percision))
                {

                    adder_εs1 = adder_εs1 / 2;

                    kofResult = new CoeffForCalcRectCrossSectionModelEC(-3.5, test_εs1,ni);
                    //if (μSd.Round(percision) == kofResult.μRd.Round(percision)) break;
                    if (kofResult.μRd > μSd) test_εs1 += adder_εs1;
                    else test_εs1 -= adder_εs1;
                }
            }
            else if (μSd < μ_lim.μRd)
            {
                var max_εc = 0.0;
                var min_εc = -3.5;

                var test_εc = (max_εc + min_εc) / 2;
                var adder_εc = test_εc;

                while (μSd.Round(percision) != kofResult.μRd.Round(percision))
                {
                    adder_εc = adder_εc / 2;
                    kofResult = new CoeffForCalcRectCrossSectionModelEC( test_εc, 20,ni);
                    //if (μSd.Round(percision) == kofResult.μRd.Round(percision)) break;
                    if (kofResult.μRd > μSd) test_εc -= adder_εc;
                    else test_εc += adder_εc;
                }
            }
            else return μ_lim;

            return kofResult;
        } 
    }
}
