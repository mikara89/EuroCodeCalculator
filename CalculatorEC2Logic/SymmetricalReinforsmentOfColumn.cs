using System;
using System.Collections.Generic;
using System.Linq;
using CalcModels;

namespace CalculatorEC2Logic
{
    public class SymmetricalReinfByMaxAndMinPercentageReinf
    {
        private IMaterial _material;

        /// <summary>
        /// 0.85 by defoult
        /// </summary>
        public double αcc { get; set; } = 0.85;

        /// <summary>
        /// 0.8 by defoult
        /// </summary>
        public double λ { get; set; } = 0.8;

        /// <summary>
        /// 1 by defoult
        /// </summary>
        public double η { get; set; } = 1;

        /// <summary>
        /// 0.95 by defoult
        /// </summary>
        public double ratio_d_and_h { get; set; } = 0.95;

        /// <summary>
        /// 0.95 by defoult
        /// </summary>
        public double ratio_d1_and_h
        {
            get { return 1 - ratio_d_and_h; }
        }
        /// <summary>
        /// 0.0035 by defoult
        /// </summary>
        public double εcu3 { get; set; } = 0.0035;

        private Generate_ρ_LineForDiagram minOf_ρ;
        private Generate_ρ_LineForDiagram maxOf_ρ;
        private Generate_ρ_LineForDiagram searchingOf_ρ;

        public SymmetricalReinfByMaxAndMinPercentageReinf(IMaterial material,IElementGeometry geomety)
        {
            ratio_d_and_h = (geomety as ElementGeometry).d / geomety.h;
            _material = material;
            SetMinimumOf_ρ_and_Max();
        }

        private void SetMinimumOf_ρ_and_Max()
        {
            minOf_ρ = new Generate_ρ_LineForDiagram(_material);
            minOf_ρ.αcc = αcc;
            minOf_ρ.λ = λ;
            minOf_ρ.η = η;
            minOf_ρ.ratio_d_and_h = ratio_d_and_h;
            minOf_ρ.ratio_d1_and_h = ratio_d1_and_h;
            minOf_ρ.ρ = 0.004;
            minOf_ρ.GetLineForDiagram();
            maxOf_ρ = new Generate_ρ_LineForDiagram(_material);
            maxOf_ρ.αcc = αcc;
            maxOf_ρ.λ = λ;
            maxOf_ρ.η = η;
            maxOf_ρ.ratio_d_and_h = ratio_d_and_h;
            maxOf_ρ.ratio_d1_and_h = ratio_d1_and_h;
            maxOf_ρ.ρ = 0.04;
            maxOf_ρ.GetLineForDiagram();
            searchingOf_ρ = new Generate_ρ_LineForDiagram(_material);
            searchingOf_ρ.αcc = αcc;
            searchingOf_ρ.λ = λ;
            searchingOf_ρ.η = η;
            searchingOf_ρ.ratio_d_and_h = ratio_d_and_h;
            searchingOf_ρ.ratio_d1_and_h = ratio_d1_and_h;
        }
        public double Get_ρ(double Mbh, double NbhPower2)
        {
            var ρ = maxOf_ρ.ρ / 2;
            var addTo_ρ = maxOf_ρ.ρ / 2;
            for (int i = 0; i < 20; i++)
            {
                addTo_ρ = addTo_ρ / 2;
                searchingOf_ρ.ρ = ρ;
                searchingOf_ρ.GetLineForDiagram();
                switch (CheckDiagram(searchingOf_ρ, Mbh, NbhPower2))
                {
                    case 0:
                        return ρ;
                    case 1:
                        ρ -= addTo_ρ;
                        break;
                    case 2:
                        ρ += addTo_ρ;
                        break;
                    default:
                        break;
                }
            }
            if (ρ > 0.04)
                throw new Exception("the percentage of reinforcement has been exceeded, make cross-section bigger");
            if (ρ < 0.004) return 0.004;
            return ρ;
        }
        public double Get_ρ(double M, double N, double b, double h)
        {
            var Mbh = M*1000000 / (b*10 * Math.Pow(h*10, 2));
            var NbhPower2 = N*1000 / (b*10 *h*10);

            return Get_ρ(Mbh: Mbh, NbhPower2: NbhPower2);
        }
        private int CheckDiagram(Generate_ρ_LineForDiagram toCheck, double Mbh, double NbhPower2, int percision = 3)
        {
            toCheck.GetLineForDiagram();
            var test = toCheck.ListOfDotsInLineOfDiagram;

            

            var NmaxOfM = test.Single(m => m.Mbh == test.Max(n => n.Mbh)).NbhPower2;
            if (NbhPower2>= NmaxOfM)
            {
                test.RemoveAll(n => n.NbhPower2 < NmaxOfM);
                var closestItemByM = test
                                 .Aggregate((x, y) => Math.Abs(x.Mbh - Mbh) < Math.Abs(y.Mbh - Mbh) ? x : y);

                if (
                    (Math.Round( Mbh, percision) >= Math.Round(closestItemByM.Mbh, percision) && Math.Round(closestItemByM.Mbh, percision) <= Math.Round(Mbh, percision))
                    &&
                    (Math.Round(NbhPower2, percision) >= Math.Round(closestItemByM.NbhPower2, percision) && Math.Round(closestItemByM.NbhPower2, percision) >= Math.Round(NbhPower2, percision)))
                {
                    return 0;
                }
                else if (closestItemByM.NbhPower2 > NbhPower2) return 1;
                else return 2;

            }
            else
            {
                test.RemoveAll(n => n.NbhPower2 > NmaxOfM);
                var closestItemByM = test
                                 .Aggregate((x, y) => Math.Abs(x.Mbh - Mbh) < Math.Abs(y.Mbh - Mbh) ? x : y);

                if (
                    (Math.Round(Mbh, percision) >= Math.Round(closestItemByM.Mbh, percision) && Math.Round(closestItemByM.Mbh, percision) <= Math.Round(Mbh, percision))
                    &&
                    (Math.Round(NbhPower2, percision) >= Math.Round(closestItemByM.NbhPower2, percision) && Math.Round(closestItemByM.NbhPower2, percision) >= Math.Round(NbhPower2 , percision)))
                {
                    return 0;
                }
                else if (closestItemByM.NbhPower2 < NbhPower2) return 1;
                else return 2;
            }

        }

        public class Generate_ρ_LineForDiagram
        {
            private readonly IMaterial _material; 
            public List<Mbh_NbhPower2> ListOfDotsInLineOfDiagram { get; set; }
            /// <summary>
            /// 0.85 by defoult
            /// </summary>
            public double αcc { get; set; }

            /// <summary>
            /// 0.8 by defoult
            /// </summary>
            public double λ { get; set; }

            /// <summary>
            /// 1 by defoult
            /// </summary>
            public double η { get; set; }


            /// <summary>
            /// 0.004 by defoult
            /// </summary>
            public double ρ { get; set; }

            /// <summary>
            /// 0.95 by defoult
            /// </summary>
            public double ratio_d_and_h { get; set; }

            /// <summary>
            /// 0.95 by defoult
            /// </summary>
            public double ratio_d1_and_h
            {
                get;set;
            }
            /// <summary>
            /// 0.0035 by defoult
            /// </summary>
            public double εcu3 { get; set; } = 0.0035;
            public Generate_ρ_LineForDiagram(IMaterial material)
            {
                _material = material;
            }

            private Mbh_NbhPower2 GetMbh_NbhPower2(double ratio_x_and_h)
            {
                var result = new Mbh_NbhPower2();

                result.εsc = εcu3 * (1 - ratio_d1_and_h * 1 / ratio_x_and_h);
                result.εst = εcu3 * (((1 / ratio_x_and_h) * ratio_d_and_h ) - 1);

                result.Ratio_x_and_h = ratio_x_and_h;

                result.fsc = result.εsc * _material.armatura.Es * 1000 >= _material.armatura.fyd * 10 ? _material.armatura.fyd * 10 : result.εsc * _material.armatura.Es * 1000;
                result.fst = result.εst * _material.armatura.Es * 1000 >= _material.armatura.fyd * 10 ? _material.armatura.fyd * 10 : result.εst * _material.armatura.Es * 1000;

                //Cc/(bh)
                result.Cc_divideWithBandH = ratio_x_and_h <= 1.25 ? αcc * λ * η * _material.beton.fcd / αcc * ratio_x_and_h : αcc * η * _material.beton.fcd / αcc;

                //Mc/(bh^2)
                result.Mc_divideWithBandHPower2 = ratio_x_and_h <= 1.25 ? 0.5 * αcc * λ * η * _material.beton.fcd / αcc * ratio_x_and_h * (1 - λ * ratio_x_and_h) : 0;

                result.NbhPower2 = result.Cc_divideWithBandH + ρ / 2 * (result.fsc - result.fst);
                result.Mbh = result.Mc_divideWithBandHPower2 + ρ / 2 * 0.45 * (result.fsc + result.fst);

                return result;
            }

            public void GetLineForDiagram(double Max_ratio_x_and_h = 4, double Min_ratio_x_and_h = 0.03)
            {
                ListOfDotsInLineOfDiagram = new List<Mbh_NbhPower2>();
                for (double i = Min_ratio_x_and_h; i < Max_ratio_x_and_h; i += 0.01)
                {
                    ListOfDotsInLineOfDiagram.Add(GetMbh_NbhPower2(i));
                }
            }
        }
        public class Mbh_NbhPower2
        {
            public double Mbh { get; set; }
            public double NbhPower2 { get; set; }
            public double εsc { get; set; } 
            public double εst { get; set; }
            public double fsc { get; set; }
            public double fst { get; set; }
            public double Cc_divideWithBandH { get; set; }
            public double Mc_divideWithBandHPower2 { get; set; }
            public double Ratio_x_and_h { get; set; } 
        }
    }
}