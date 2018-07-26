using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TabeleEC2.Model;
using static TabeleEC2.Model.KofZaProracunPravougaonogPresekaModelEC;

namespace TabeleEC2
{

    public static class KofZaProracunPravougaonogPresekaEC
    {
        
        public static List<KofZaProracunPravougaonogPresekaModelEC> GetKofZaProracunPravougaonogPresekaList()
        {
            List<KofZaProracunPravougaonogPresekaModelEC> items;
            items = /*ListTacna();*/ JsonConvert.DeserializeObject<List<KofZaProracunPravougaonogPresekaModelEC>>(jsons);
            return items;
        }
        public static KofZaProracunPravougaonogPresekaModelEC GetLimitKofZaProracunPravougaonogPresekaEC(BetonModelEC beton)
        {
            if (beton.fck <= 35)
            {
                return new KofZaProracunPravougaonogPresekaModelEC() { εc = -3.5, εs1 = 4.278, ζ = 0.813, ξ = 0.45, μRd = 0.252, ω = 0/*, kd = 0*/ };
            }else return new KofZaProracunPravougaonogPresekaModelEC() { εc = -3.5, εs1 = 6.5, ζ = 0.854, ξ = 0.35, μRd = 0.206, ω = 0/*, kd = 0*/ };
        }

       
        private static double GetItem_ξ(double ξ) 
        {
            var list = GetKofZaProracunPravougaonogPresekaList().Select(n => n.ξ);

            double closest = list
                .Select(n => new { n, distance = Math.Abs(n - ξ) })
                .OrderBy(p => p.distance)
                .First(i => i.n >= ξ).n;

            return closest;
        }

        private static double GetItem_ω(double μSd)
        {
            var item = GetKofZaProracunPravougaonogPresekaList().Single(n =>n.μRd== GetItem_μRd(μSd));

            return item.ω;
        }
        private static double GetItem_μRd(double μSd)
        {
            var list = GetKofZaProracunPravougaonogPresekaList().Select(n => n.μRd);

            double closest = list
                .Select(n => new { n, distance = Math.Abs(n - μSd) })
                .OrderBy(p => p.distance)
                .First(i => i.n >= μSd).n;

            return closest;
        }
        public static KofZaProracunPravougaonogPresekaModelEC GetItem_Full(double μSd) 
        {
            var item = GetKofZaProracunPravougaonogPresekaList().Single(n => n.μRd == GetItem_μRd(μSd));

            return item;
        }
        private static KofZaProracunPravougaonogPresekaModelEC GetItem_Full(double ξ, double a=0)
        {
            var item = GetKofZaProracunPravougaonogPresekaList().Single(n => n.ξ == GetItem_ξ(ξ));

            return item;
        }
        private static KofZaProracunPravougaonogPresekaModelEC GetItem_Full()
        {
            var item = GetKofZaProracunPravougaonogPresekaList().Single(n => n.εc == -3.5 && n.εs1 == 20);

            return item;
        }


        /// <summary>
        /// Msd * 100 / (b * Math.Pow(d, 2) * fcd)
        /// </summary>
        /// <param name="Msd">in kN/m</param>
        /// <param name="b">in cm</param>
        /// <param name="d">in cm</param>
        /// <param name="fcd">in kN/cm</param> 
        /// <returns></returns>
        public static double GetμSd(double Msd, double b, double d, double fcd)
        {
            var result= Msd * 100 / (b * Math.Pow(d, 2) * fcd);
            var max = new KofZaProracunPravougaonogPresekaModelEC(-3.5,0.5).μRd;
            var min = new KofZaProracunPravougaonogPresekaModelEC(-0.1, 20).μRd;
            if (result > max)
                return max;
                //throw new Exception("Diletacija u armaturi i betonu prekoracema; \nPovecajte presek!");
            if (result < min)
                throw new Exception("Diletacija u armaturi i betonu prekoracema; \nPovecajte presek!");
            return result;
        }

        /// <summary>
        /// εc = -3.5% && εs1=20%
        /// </summary>
        /// <returns></returns>
        public static double GetμSd()
        {
            return GetItem_Full().μRd;
        }
        /// <summary>
        ///  Vraca 2350 listu 
        /// </summary>
        /// <returns></returns>
        public static List<KofZaProracunPravougaonogPresekaModelEC> ListTacna(double percision = 0.01)
        {
            double εc = 0.01; double εs1 = 0.01; 
            var result = new List<KofZaProracunPravougaonogPresekaModelEC>();

            for (double i = εc; i <= 3.5; i += percision)
            {
                var j = 20;
                var n = new KofZaProracunPravougaonogPresekaModelEC();
                n.SetByEcEs1(-i, j);
                result.Add(n);
            }
            for (double j = εs1; j < 20; j += percision)
            {
                var i = -3.5;
                var n = new KofZaProracunPravougaonogPresekaModelEC();
                n.SetByEcEs1(i, j);
                result.Add(n);
            }
            return result;

        }


        private static List<KofZaProracunPravougaonogPresekaModelEC> GetListTacna_εs1(double max_εs1, double min_εs1, double percision = 0.0001)
        {
            var result = new List<KofZaProracunPravougaonogPresekaModelEC>();
            var εc = -3.5;

            for (double j = min_εs1; j <= max_εs1; j += percision)
            {
                var i = εc;
                var n = new KofZaProracunPravougaonogPresekaModelEC();
                n.SetByEcEs1(i, j);
                result.Add(n);
            }
            return result;
        }
        private static List<KofZaProracunPravougaonogPresekaModelEC> GetListTacna_εc(double max_εc, double min_εc, double percision = 0.0001)
        {
            //if (Math.Abs(εc) > 3.5 || εs1 > 20 && εs1 < 0) 
            //    return null;
            var result = new List<KofZaProracunPravougaonogPresekaModelEC>();


            for (double i = min_εc ; i <=  max_εc; i += percision)
            { 
                var j = 20;
                var n = new KofZaProracunPravougaonogPresekaModelEC();
                n.SetByEcEs1(i, j);
                result.Add(n);
            }
            return result;
        }
        public static KofZaProracunPravougaonogPresekaModelEC Get_Kof_From_μ(double μSd)
        {
            var μ_lim = new KofZaProracunPravougaonogPresekaModelEC();
            μ_lim.SetByEcEs1(-3.5,20);

            if (μSd > μ_lim.μRd)
            {
                var kof_iz_tablice = GetItem_Full(μSd: μSd);
                var max_εs1 = kof_iz_tablice.εs1 + 0.5;
                var min_εs1 = kof_iz_tablice.εs1 - 0.5;
                var ListToSerachForKof = new List< KofZaProracunPravougaonogPresekaModelEC > ();
                ListToSerachForKof.AddRange(GetListTacna_εs1(max_εs1, min_εs1));

                return GetSearchForμRd(μSd,ListToSerachForKof);
            }
            if (μSd < μ_lim.μRd)
            {
                var kof_iz_tablice = GetItem_Full(μSd: μSd);
                var max_εc = kof_iz_tablice.εc + 0.5;
                var min_εc = kof_iz_tablice.εc - 0.5;
                var ListToSerachForKof = new List<KofZaProracunPravougaonogPresekaModelEC>();
                ListToSerachForKof.AddRange(GetListTacna_εc(max_εc, min_εc));

                return GetSearchForμRd(μSd, ListToSerachForKof);
            }
            return μ_lim;

        }
        private static KofZaProracunPravougaonogPresekaModelEC GetSearchForμRd(double μSd, List<KofZaProracunPravougaonogPresekaModelEC> listToSearch) 
        {
            var list = listToSearch;
            if (listToSearch.Count == 0)
            {
                var nul = new KofZaProracunPravougaonogPresekaModelEC();
                nul.SetByEcEs1(0, 20);
                return nul;
            }
                
            double closest = list.Select(k=>k.μRd)
                .Select(n => new { n, distance = Math.Abs(n - μSd) })
                .OrderBy(p => p.distance)
                .First(i => i.n >= μSd).n;

            return listToSearch.Single(k=>k.μRd == closest);
        }


        #region Jsons


        private static string jsons = @"[
 {
   'εc': -0.1,
   'εs1': 20,
   'ξ': 0.005,
   'ζ': 0.998,
   'ω': 0,
   'μRd': 0,
   'kd': 0
 },
 {
   'εc': -0.2,
   'εs1': 20,
   'ξ': 0.01,
   'ζ': 0.997,
   'ω': 0.001,
   'μRd': 0.001,
   'kd': 31.623
 },
 {
   'εc': -0.3,
   'εs1': 20,
   'ξ': 0.015,
   'ζ': 0.995,
   'ω': 0.002,
   'μRd': 0.002,
   'kd': 22.361
 },
 {
   'εc': -0.4,
   'εs1': 20,
   'ξ': 0.02,
   'ζ': 0.993,
   'ω': 0.003,
   'μRd': 0.003,
   'kd': 18.257
 },
 {
   'εc': -0.5,
   'εs1': 20,
   'ξ': 0.024,
   'ζ': 0.992,
   'ω': 0.005,
   'μRd': 0.005,
   'kd': 14.142
 },
 {
   'εc': -0.6,
   'εs1': 20,
   'ξ': 0.029,
   'ζ': 0.99,
   'ω': 0.007,
   'μRd': 0.007,
   'kd': 11.952
 },
 {
   'εc': -0.7,
   'εs1': 20,
   'ξ': 0.034,
   'ζ': 0.988,
   'ω': 0.009,
   'μRd': 0.009,
   'kd': 10.541
 },
 {
   'εc': -0.8,
   'εs1': 20,
   'ξ': 0.038,
   'ζ': 0.987,
   'ω': 0.011,
   'μRd': 0.011,
   'kd': 9.535
 },
 {
   'εc': -0.9,
   'εs1': 20,
   'ξ': 0.043,
   'ζ': 0.985,
   'ω': 0.014,
   'μRd': 0.014,
   'kd': 8.452
 },
 {
   'εc': -1,
   'εs1': 20,
   'ξ': 0.048,
   'ζ': 0.983,
   'ω': 0.017,
   'μRd': 0.017,
   'kd': 7.67
 },
 {
   'εc': -1.1,
   'εs1': 20,
   'ξ': 0.052,
   'ζ': 0.982,
   'ω': 0.02,
   'μRd': 0.019,
   'kd': 7.255
 },
 {
   'εc': -1.2,
   'εs1': 20,
   'ξ': 0.057,
   'ζ': 0.98,
   'ω': 0.023,
   'μRd': 0.023,
   'kd': 6.594
 },
 {
   'εc': -1.3,
   'εs1': 20,
   'ξ': 0.061,
   'ζ': 0.978,
   'ω': 0.026,
   'μRd': 0.026,
   'kd': 6.202
 },
 {
   'εc': -1.4,
   'εs1': 20,
   'ξ': 0.065,
   'ζ': 0.977,
   'ω': 0.03,
   'μRd': 0.029,
   'kd': 5.872
 },
 {
   'εc': -1.5,
   'εs1': 20,
   'ξ': 0.07,
   'ζ': 0.975,
   'ω': 0.033,
   'μRd': 0.033,
   'kd': 5.505
 },
 {
   'εc': -1.6,
   'εs1': 20,
   'ξ': 0.074,
   'ζ': 0.973,
   'ω': 0.037,
   'μRd': 0.036,
   'kd': 5.27
 },
 {
   'εc': -1.7,
   'εs1': 20,
   'ξ': 0.078,
   'ζ': 0.971,
   'ω': 0.04,
   'μRd': 0.039,
   'kd': 5.064
 },
 {
   'εc': -1.8,
   'εs1': 20,
   'ξ': 0.083,
   'ζ': 0.969,
   'ω': 0.044,
   'μRd': 0.043,
   'kd': 4.822
 },
 {
   'εc': -1.9,
   'εs1': 20,
   'ξ': 0.087,
   'ζ': 0.968,
   'ω': 0.048,
   'μRd': 0.046,
   'kd': 4.663
 },
 {
   'εc': -2,
   'εs1': 20,
   'ξ': 0.091,
   'ζ': 0.966,
   'ω': 0.052,
   'μRd': 0.05,
   'kd': 4.472
 },
 {
   'εc': -2.1,
   'εs1': 20,
   'ξ': 0.095,
   'ζ': 0.964,
   'ω': 0.055,
   'μRd': 0.053,
   'kd': 4.344
 },
 {
   'εc': -2.2,
   'εs1': 20,
   'ξ': 0.099,
   'ζ': 0.962,
   'ω': 0.059,
   'μRd': 0.056,
   'kd': 4.226
 },
 {
   'εc': -2.3,
   'εs1': 20,
   'ξ': 0.103,
   'ζ': 0.96,
   'ω': 0.062,
   'μRd': 0.06,
   'kd': 4.082
 },
 {
   'εc': -2.4,
   'εs1': 20,
   'ξ': 0.107,
   'ζ': 0.959,
   'ω': 0.066,
   'μRd': 0.063,
   'kd': 3.984
 },
 {
   'εc': -2.5,
   'εs1': 20,
   'ξ': 0.111,
   'ζ': 0.957,
   'ω': 0.069,
   'μRd': 0.066,
   'kd': 3.892
 },
 {
   'εc': -2.6,
   'εs1': 20,
   'ξ': 0.115,
   'ζ': 0.955,
   'ω': 0.073,
   'μRd': 0.069,
   'kd': 3.807
 },
 {
   'εc': -2.7,
   'εs1': 20,
   'ξ': 0.119,
   'ζ': 0.953,
   'ω': 0.076,
   'μRd': 0.073,
   'kd': 3.701
 },
 {
   'εc': -2.8,
   'εs1': 20,
   'ξ': 0.123,
   'ζ': 0.951,
   'ω': 0.08,
   'μRd': 0.076,
   'kd': 3.627
 },
 {
   'εc': -2.9,
   'εs1': 20,
   'ξ': 0.127,
   'ζ': 0.949,
   'ω': 0.083,
   'μRd': 0.079,
   'kd': 3.558
 },
 {
   'εc': -3,
   'εs1': 20,
   'ξ': 0.13,
   'ζ': 0.947,
   'ω': 0.086,
   'μRd': 0.081,
   'kd': 3.514
 },
 {
   'εc': -3.1,
   'εs1': 20,
   'ξ': 0.134,
   'ζ': 0.945,
   'ω': 0.089,
   'μRd': 0.084,
   'kd': 3.45
 },
 {
   'εc': -3.2,
   'εs1': 20,
   'ξ': 0.138,
   'ζ': 0.943,
   'ω': 0.093,
   'μRd': 0.088,
   'kd': 3.371
 },
 {
   'εc': -3.3,
   'εs1': 20,
   'ξ': 0.142,
   'ζ': 0.942,
   'ω': 0.096,
   'μRd': 0.091,
   'kd': 3.315
 },
 {
   'εc': -3.4,
   'εs1': 20,
   'ξ': 0.145,
   'ζ': 0.94,
   'ω': 0.099,
   'μRd': 0.093,
   'kd': 3.279
 },
 {
   'εc': -3.5,
   'εs1': 20,
   'ξ': 0.149,
   'ζ': 0.938,
   'ω': 0.103,
   'μRd': 0.096,
   'kd': 3.227
 },
 {
   'εc': -3.5,
   'εs1': 19.5,
   'ξ': 0.152,
   'ζ': 0.937,
   'ω': 0.105,
   'μRd': 0.098,
   'kd': 3.194
 },
 {
   'εc': -3.5,
   'εs1': 19,
   'ξ': 0.156,
   'ζ': 0.935,
   'ω': 0.107,
   'μRd': 0.1,
   'kd': 3.162
 },
 {
   'εc': -3.5,
   'εs1': 18.5,
   'ξ': 0.159,
   'ζ': 0.934,
   'ω': 0.109,
   'μRd': 0.102,
   'kd': 3.131
 },
 {
   'εc': -3.5,
   'εs1': 18,
   'ξ': 0.163,
   'ζ': 0.932,
   'ω': 0.112,
   'μRd': 0.105,
   'kd': 3.086
 },
 {
   'εc': -3.5,
   'εs1': 17.5,
   'ξ': 0.167,
   'ζ': 0.931,
   'ω': 0.115,
   'μRd': 0.107,
   'kd': 3.057
 },
 {
   'εc': -3.5,
   'εs1': 17,
   'ξ': 0.171,
   'ζ': 0.929,
   'ω': 0.118,
   'μRd': 0.109,
   'kd': 3.029
 },
 {
   'εc': -3.5,
   'εs1': 16.5,
   'ξ': 0.175,
   'ζ': 0.927,
   'ω': 0.12,
   'μRd': 0.112,
   'kd': 2.988
 },
 {
   'εc': -3.5,
   'εs1': 16,
   'ξ': 0.179,
   'ζ': 0.926,
   'ω': 0.123,
   'μRd': 0.114,
   'kd': 2.962
 },
 {
   'εc': -3.5,
   'εs1': 15.5,
   'ξ': 0.184,
   'ζ': 0.923,
   'ω': 0.127,
   'μRd': 0.117,
   'kd': 2.924
 },
 {
   'εc': -3.5,
   'εs1': 15,
   'ξ': 0.189,
   'ζ': 0.921,
   'ω': 0.13,
   'μRd': 0.12,
   'kd': 2.887
 },
 {
   'εc': -3.5,
   'εs1': 14.5,
   'ξ': 0.194,
   'ζ': 0.919,
   'ω': 0.133,
   'μRd': 0.123,
   'kd': 2.851
 },
 {
   'εc': -3.5,
   'εs1': 14,
   'ξ': 0.2,
   'ζ': 0.917,
   'ω': 0.138,
   'μRd': 0.126,
   'kd': 2.817
 },
 {
   'εc': -3.5,
   'εs1': 13.5,
   'ξ': 0.206,
   'ζ': 0.914,
   'ω': 0.142,
   'μRd': 0.13,
   'kd': 2.774
 },
 {
   'εc': -3.5,
   'εs1': 13,
   'ξ': 0.212,
   'ζ': 0.912,
   'ω': 0.146,
   'μRd': 0.133,
   'kd': 2.742
 },
 {
   'εc': -3.5,
   'εs1': 12.5,
   'ξ': 0.219,
   'ζ': 0.909,
   'ω': 0.151,
   'μRd': 0.137,
   'kd': 2.702
 },
 {
   'εc': -3.5,
   'εs1': 12,
   'ξ': 0.226,
   'ζ': 0.906,
   'ω': 0.156,
   'μRd': 0.141,
   'kd': 2.663
 },
 {
   'εc': -3.5,
   'εs1': 11.5,
   'ξ': 0.233,
   'ζ': 0.903,
   'ω': 0.16,
   'μRd': 0.145,
   'kd': 2.626
 },
 {
   'εc': -3.5,
   'εs1': 11,
   'ξ': 0.241,
   'ζ': 0.9,
   'ω': 0.166,
   'μRd': 0.149,
   'kd': 2.591
 },
 {
   'εc': -3.5,
   'εs1': 10.5,
   'ξ': 0.25,
   'ζ': 0.896,
   'ω': 0.172,
   'μRd': 0.154,
   'kd': 2.548
 },
 {
   'εc': -3.5,
   'εs1': 10,
   'ξ': 0.259,
   'ζ': 0.892,
   'ω': 0.178,
   'μRd': 0.159,
   'kd': 2.508
 },
 {
   'εc': -3.5,
   'εs1': 9.5,
   'ξ': 0.269,
   'ζ': 0.888,
   'ω': 0.185,
   'μRd': 0.164,
   'kd': 2.469
 },
 {
   'εc': -3.5,
   'εs1': 9,
   'ξ': 0.28,
   'ζ': 0.884,
   'ω': 0.193,
   'μRd': 0.17,
   'kd': 2.425
 },
 {
   'εc': -3.5,
   'εs1': 8.5,
   'ξ': 0.292,
   'ζ': 0.879,
   'ω': 0.201,
   'μRd': 0.177,
   'kd': 2.377
 },
 {
   'εc': -3.5,
   'εs1': 8,
   'ξ': 0.304,
   'ζ': 0.874,
   'ω': 0.209,
   'μRd': 0.183,
   'kd': 2.338
 },
 {
   'εc': -3.5,
   'εs1': 7.5,
   'ξ': 0.318,
   'ζ': 0.868,
   'ω': 0.219,
   'μRd': 0.19,
   'kd': 2.294
 },
 {
   'εc': -3.5,
   'εs1': 7,
   'ξ': 0.333,
   'ζ': 0.861,
   'ω': 0.229,
   'μRd': 0.197,
   'kd': 2.253
 },
 {
   'εc': -3.5,
   'εs1': 6.5,
   'ξ': 0.35,
   'ζ': 0.854,
   'ω': 0.241,
   'μRd': 0.206,
   'kd': 2.203
 },
 {
   'εc': -3.5,
   'εs1': 6,
   'ξ': 0.368,
   'ζ': 0.847,
   'ω': 0.253,
   'μRd': 0.214,
   'kd': 2.162
 },
 {
   'εc': -3.5,
   'εs1': 5.5,
   'ξ': 0.389,
   'ζ': 0.838,
   'ω': 0.268,
   'μRd': 0.224,
   'kd': 2.113
 },
 {
   'εc': -3.5,
   'εs1': 5,
   'ξ': 0.412,
   'ζ': 8.329,
   'ω': 0.283,
   'μRd': 0.235,
   'kd': 2.063
 },
 {
   'εc': -3.5,
   'εs1': 4.5,
   'ξ': 0.438,
   'ζ': 0.818,
   'ω': 0.301,
   'μRd': 0.247,
   'kd': 2.012
 },
 {
   'εc': -3.5,
   'εs1': 4,
   'ξ': 0.467,
   'ζ': 0.806,
   'ω': 0.321,
   'μRd': 0.259,
   'kd': 1.965
 },
 {
   'εc': -3.5,
   'εs1': 3.5,
   'ξ': 0.5,
   'ζ': 0.792,
   'ω': 0.344,
   'μRd': 0.272,
   'kd': 1.917
 },
 {
   'εc': -3.5,
   'εs1': 3,
   'ξ': 0.538,
   'ζ': 0.776,
   'ω': 0.37,
   'μRd': 0.287,
   'kd': 1.867
 },
 {
   'εc': -3.5,
   'εs1': 2.5,
   'ξ': 0.583,
   'ζ': 0.757,
   'ω': 0.401,
   'μRd': 0.304,
   'kd': 1.814
 },
 {
   'εc': -3.5,
   'εs1': 2,
   'ξ': 0.636,
   'ζ': 0.735,
   'ω': 0.438,
   'μRd': 0.322,
   'kd': 1.762
 },
 {
   'εc': -3.5,
   'εs1': 1.5,
   'ξ': 0.7,
   'ζ': 0.709,
   'ω': 0.482,
   'μRd': 0.342,
   'kd': 1.71
 },
 {
   'εc': -3.5,
   'εs1': 1,
   'ξ': 0.778,
   'ζ': 0.676,
   'ω': 0.535,
   'μRd': 0.362,
   'kd': 1.662
 },
 {
   'εc': -3.5,
   'εs1': 0.5,
   'ξ': 0.875,
   'ζ': 0.636,
   'ω': 0.602,
   'μRd': 0.383,
   'kd': 1.616
 }
]";
        #endregion
        #region Jsons_old
        private static string jsons_ = @"[ 
 {
   'εc': -0.1,
   'εs1': 20,
   'ξ': 0.005,
   'ζ': 0.998,
   'μRd': 0,
   'ω': 0,
   'αv': 0.049,
   'ka': 0.335
 },
 {
   'εc': -0.2,
   'εs1': 20,
   'ξ': 0.01,
   'ζ': 0.997,
   'μRd': 0.001,
   'ω': 0.001,
   'αv': 0.097,
   'ka': 0.336
 },
 {
   'εc': -0.3,
   'εs1': 20,
   'ξ': 0.015,
   'ζ': 0.995,
   'μRd': 0.002,
   'ω': 0.002,
   'αv': 0.143,
   'ka': 0.338
 },
 {
   'εc': -0.4,
   'εs1': 20,
   'ξ': 0.02,
   'ζ': 0.993,
   'μRd': 0.004,
   'ω': 0.004,
   'αv': 0.187,
   'ka': 0.339
 },
 {
   'εc': -0.5,
   'εs1': 20,
   'ξ': 0.024,
   'ζ': 0.992,
   'μRd': 0.006,
   'ω': 0.006,
   'αv': 0.229,
   'ka': 0.341
 },
 {
   'εc': -0.6,
   'εs1': 20,
   'ξ': 0.029,
   'ζ': 0.99,
   'μRd': 0.008,
   'ω': 0.008,
   'αv': 0.27,
   'ka': 0.343
 },
 {
   'εc': -0.7,
   'εs1': 20,
   'ξ': 0.034,
   'ζ': 0.988,
   'μRd': 0.01,
   'ω': 0.01,
   'αv': 0.309,
   'ka': 0.344
 },
 {
   'εc': -0.8,
   'εs1': 20,
   'ξ': 0.038,
   'ζ': 0.987,
   'μRd': 0.013,
   'ω': 0.013,
   'αv': 0.347,
   'ka': 0.346
 },
 {
   'εc': -0.9,
   'εs1': 20,
   'ξ': 0.043,
   'ζ': 0.985,
   'μRd': 0.016,
   'ω': 0.016,
   'αv': 0.383,
   'ka': 0.348
 },
 {
   'εc': -1,
   'εs1': 20,
   'ξ': 0.048,
   'ζ': 0.983,
   'μRd': 0.02,
   'ω': 0.02,
   'αv': 0.417,
   'ka': 0.35
 },
 {
   'εc': -1.1,
   'εs1': 20,
   'ξ': 0.052,
   'ζ': 0.982,
   'μRd': 0.023,
   'ω': 0.023,
   'αv': 0.449,
   'ka': 0.352
 },
 {
   'εc': -1.2,
   'εs1': 20,
   'ξ': 0.057,
   'ζ': 0.98,
   'μRd': 0.027,
   'ω': 0.027,
   'αv': 0.48,
   'ka': 0.354
 },
 {
   'εc': -1.3,
   'εs1': 20,
   'ξ': 0.061,
   'ζ': 0.978,
   'μRd': 0.03,
   'ω': 0.031,
   'αv': 0.509,
   'ka': 0.356
 },
 {
   'εc': -1.4,
   'εs1': 20,
   'ξ': 0.065,
   'ζ': 0.977,
   'μRd': 0.034,
   'ω': 0.035,
   'αv': 0.537,
   'ka': 0.359
 },
 {
   'εc': -1.5,
   'εs1': 20,
   'ξ': 0.07,
   'ζ': 0.975,
   'μRd': 0.038,
   'ω': 0.039,
   'αv': 0.563,
   'ka': 0.361
 },
 {
   'εc': -1.6,
   'εs1': 20,
   'ξ': 0.074,
   'ζ': 0.973,
   'μRd': 0.042,
   'ω': 0.043,
   'αv': 0.587,
   'ka': 0.364
 },
 {
   'εc': -1.7,
   'εs1': 20,
   'ξ': 0.078,
   'ζ': 0.971,
   'μRd': 0.046,
   'ω': 0.048,
   'αv': 0.609,
   'ka': 0.366
 },
 {
   'εc': -1.8,
   'εs1': 20,
   'ξ': 0.083,
   'ζ': 0.97,
   'μRd': 0.05,
   'ω': 0.052,
   'αv': 0.63,
   'ka': 0.369
 },
 {
   'εc': -1.9,
   'εs1': 20,
   'ξ': 0.087,
   'ζ': 0.968,
   'μRd': 0.055,
   'ω': 0.056,
   'αv': 0.649,
   'ka': 0.372
 },
 {
   'εc': -2,
   'εs1': 20,
   'ξ': 0.091,
   'ζ': 0.966,
   'μRd': 0.059,
   'ω': 0.061,
   'αv': 0.667,
   'ka': 0.375
 },
 {
   'εc': -2.1,
   'εs1': 20,
   'ξ': 0.095,
   'ζ': 0.964,
   'μRd': 0.063,
   'ω': 0.065,
   'αv': 0.683,
   'ka': 0.378
 },
 {
   'εc': -2.2,
   'εs1': 20,
   'ξ': 0.099,
   'ζ': 0.962,
   'μRd': 0.066,
   'ω': 0.069,
   'αv': 0.697,
   'ka': 0.381
 },
 {
   'εc': -2.3,
   'εs1': 20,
   'ξ': 0.103,
   'ζ': 0.96,
   'μRd': 0.07,
   'ω': 0.073,
   'αv': 0.71,
   'ka': 0.385
 },
 {
   'εc': -2.4,
   'εs1': 20,
   'ξ': 0.107,
   'ζ': 0.958,
   'μRd': 0.074,
   'ω': 0.077,
   'αv': 0.722,
   'ka': 0.388
 },
 {
   'εc': -2.5,
   'εs1': 20,
   'ξ': 0.111,
   'ζ': 0.957,
   'μRd': 0.078,
   'ω': 0.081,
   'αv': 0.733,
   'ka': 0.391
 },
 {
   'εc': -2.6,
   'εs1': 20,
   'ξ': 0.115,
   'ζ': 0.955,
   'μRd': 0.082,
   'ω': 0.086,
   'αv': 0.744,
   'ka': 0.394
 },
 {
   'εc': -2.7,
   'εs1': 20,
   'ξ': 0.119,
   'ζ': 0.953,
   'μRd': 0.085,
   'ω': 0.09,
   'αv': 0.753,
   'ka': 0.397
 },
 {
   'εc': -2.8,
   'εs1': 20,
   'ξ': 0.123,
   'ζ': 0.951,
   'μRd': 0.089,
   'ω': 0.094,
   'αv': 0.762,
   'ka': 0.4
 },
 {
   'εc': -2.9,
   'εs1': 20,
   'ξ': 0.127,
   'ζ': 0.949,
   'μRd': 0.093,
   'ω': 0.098,
   'αv': 0.77,
   'ka': 0.402
 },
 {
   'εc': -3,
   'εs1': 20,
   'ξ': 0.13,
   'ζ': 0.947,
   'μRd': 0.096,
   'ω': 0.101,
   'αv': 0.778,
   'ka': 0.405
 },
 {
   'εc': -3.1,
   'εs1': 20,
   'ξ': 0.134,
   'ζ': 0.945,
   'μRd': 0.1,
   'ω': 0.105,
   'αv': 0.785,
   'ka': 0.407
 },
 {
   'εc': -3.2,
   'εs1': 20,
   'ξ': 0.138,
   'ζ': 0.944,
   'μRd': 0.103,
   'ω': 0.109,
   'αv': 0.792,
   'ka': 0.41
 },
 {
   'εc': -3.3,
   'εs1': 20,
   'ξ': 0.142,
   'ζ': 0.942,
   'μRd': 0.106,
   'ω': 0.113,
   'αv': 0.798,
   'ka': 0.412
 },
 {
   'εc': -3.4,
   'εs1': 20,
   'ξ': 0.145,
   'ζ': 0.94,
   'μRd': 0.11,
   'ω': 0.117,
   'αv': 0.804,
   'ka': 0.414
 },
 {
   'εc': -3.5,
   'εs1': 20,
   'ξ': 0.149,
   'ζ': 0.938,
   'μRd': 0.113,
   'ω': 0.121,
   'αv': 0.81,
   'ka': 0.416
 },
 {
   'εc': -3.5,
   'εs1': 19.5,
   'ξ': 0.152,
   'ζ': 0.937,
   'μRd': 0.115,
   'ω': 0.123,
   'αv': 0.81,
   'ka': 0.416
 },
 {
   'εc': -3.5,
   'εs1': 19,
   'ξ': 0.156,
   'ζ': 0.935,
   'μRd': 0.118,
   'ω': 0.126,
   'αv': 0.81,
   'ka': 0.416
 },
 {
   'εc': -3.5,
   'εs1': 18.5,
   'ξ': 0.159,
   'ζ': 0.934,
   'μRd': 0.12,
   'ω': 0.129,
   'αv': 0.81,
   'ka': 0.416
 },
 {
   'εc': -3.5,
   'εs1': 18,
   'ξ': 0.163,
   'ζ': 0.932,
   'μRd': 0.123,
   'ω': 0.132,
   'αv': 0.81,
   'ka': 0.416
 },
 {
   'εc': -3.5,
   'εs1': 17.5,
   'ξ': 0.167,
   'ζ': 0.931,
   'μRd': 0.126,
   'ω': 0.135,
   'αv': 0.81,
   'ka': 0.416
 },
 {
   'εc': -3.5,
   'εs1': 17,
   'ξ': 0.171,
   'ζ': 0.929,
   'μRd': 0.128,
   'ω': 0.138,
   'αv': 0.81,
   'ka': 0.416
 },
 {
   'εc': -3.5,
   'εs1': 16.5,
   'ξ': 0.175,
   'ζ': 0.927,
   'μRd': 0.131,
   'ω': 0.142,
   'αv': 0.81,
   'ka': 0.416
 },
 {
   'εc': -3.5,
   'εs1': 16,
   'ξ': 0.179,
   'ζ': 0.925,
   'μRd': 0.134,
   'ω': 0.145,
   'αv': 0.81,
   'ka': 0.416
 },
 {
   'εc': -3.5,
   'εs1': 15.5,
   'ξ': 0.184,
   'ζ': 0.923,
   'μRd': 0.138,
   'ω': 0.149,
   'αv': 0.81,
   'ka': 0.416
 },
 {
   'εc': -3.5,
   'εs1': 15,
   'ξ': 0.189,
   'ζ': 0.921,
   'μRd': 0.141,
   'ω': 0.153,
   'αv': 0.81,
   'ka': 0.416
 },
 {
   'εc': -3.5,
   'εs1': 14.5,
   'ξ': 0.194,
   'ζ': 0.919,
   'μRd': 0.145,
   'ω': 0.157,
   'αv': 0.81,
   'ka': 0.416
 },
 {
   'εc': -3.5,
   'εs1': 14,
   'ξ': 0.2,
   'ζ': 0.917,
   'μRd': 0.148,
   'ω': 0.162,
   'αv': 0.81,
   'ka': 0.416
 },
 {
   'εc': -3.5,
   'εs1': 13.5,
   'ξ': 0.206,
   'ζ': 0.914,
   'μRd': 0.152,
   'ω': 0.167,
   'αv': 0.81,
   'ka': 0.416
 },
 {
   'εc': -3.5,
   'εs1': 13,
   'ξ': 0.212,
   'ζ': 0.912,
   'μRd': 0.157,
   'ω': 0.172,
   'αv': 0.81,
   'ka': 0.416
 },
 {
   'εc': -3.5,
   'εs1': 12.5,
   'ξ': 0.219,
   'ζ': 0.909,
   'μRd': 0.161,
   'ω': 0.177,
   'αv': 0.81,
   'ka': 0.416
 },
 {
   'εc': -3.5,
   'εs1': 12,
   'ξ': 0.226,
   'ζ': 0.906,
   'μRd': 0.166,
   'ω': 0.183,
   'αv': 0.81,
   'ka': 0.416
 },
 {
   'εc': -3.5,
   'εs1': 11.5,
   'ξ': 0.233,
   'ζ': 0.903,
   'μRd': 0.171,
   'ω': 0.189,
   'αv': 0.81,
   'ka': 0.416
 },
 {
   'εc': -3.5,
   'εs1': 11,
   'ξ': 0.241,
   'ζ': 0.9,
   'μRd': 0.176,
   'ω': 0.195,
   'αv': 0.81,
   'ka': 0.416
 },
 {
   'εc': -3.5,
   'εs1': 10.5,
   'ξ': 0.25,
   'ζ': 0.896,
   'μRd': 0.181,
   'ω': 0.202,
   'αv': 0.81,
   'ka': 0.416
 },
 {
   'εc': -3.5,
   'εs1': 10,
   'ξ': 0.259,
   'ζ': 0.892,
   'μRd': 0.187,
   'ω': 0.21,
   'αv': 0.81,
   'ka': 0.416
 },
 {
   'εc': -3.5,
   'εs1': 9.5,
   'ξ': 0.269,
   'ζ': 0.888,
   'μRd': 0.194,
   'ω': 0.218,
   'αv': 0.81,
   'ka': 0.416
 },
 {
   'εc': -3.5,
   'εs1': 9,
   'ξ': 0.28,
   'ζ': 0.884,
   'μRd': 0.2,
   'ω': 0.227,
   'αv': 0.81,
   'ka': 0.416
 },
 {
   'εc': -3.5,
   'εs1': 8.5,
   'ξ': 0.292,
   'ζ': 0.879,
   'μRd': 0.207,
   'ω': 0.236,
   'αv': 0.81,
   'ka': 0.416
 },
 {
   'εc': -3.5,
   'εs1': 8,
   'ξ': 0.304,
   'ζ': 0.873,
   'μRd': 0.215,
   'ω': 0.246,
   'αv': 0.81,
   'ka': 0.416
 },
 {
   'εc': -3.5,
   'εs1': 7.5,
   'ξ': 0.318,
   'ζ': 0.868,
   'μRd': 0.223,
   'ω': 0.258,
   'αv': 0.81,
   'ka': 0.416
 },
 {
   'εc': -3.5,
   'εs1': 7,
   'ξ': 0.333,
   'ζ': 0.861,
   'μRd': 0.232,
   'ω': 0.27,
   'αv': 0.81,
   'ka': 0.416
 },
 {
   'εc': -3.5,
   'εs1': 6.5,
   'ξ': 0.35,
   'ζ': 0.854,
   'μRd': 0.242,
   'ω': 0.283,
   'αv': 0.81,
   'ka': 0.416
 },
 {
   'εc': -3.5,
   'εs1': 6,
   'ξ': 0.368,
   'ζ': 0.847,
   'μRd': 0.253,
   'ω': 0.298,
   'αv': 0.81,
   'ka': 0.416
 },
 {
   'εc': -3.5,
   'εs1': 5.5,
   'ξ': 0.389,
   'ζ': 0.838,
   'μRd': 0.264,
   'ω': 0.315,
   'αv': 0.81,
   'ka': 0.416
 },
 {
   'εc': -3.5,
   'εs1': 5,
   'ξ': 0.412,
   'ζ': 0.829,
   'μRd': 0.276,
   'ω': 0.333,
   'αv': 0.81,
   'ka': 0.416
 },
 {
   'εc': -3.5,
   'εs1': 4.5,
   'ξ': 0.438,
   'ζ': 0.818,
   'μRd': 0.29,
   'ω': 0.354,
   'αv': 0.81,
   'ka': 0.416
 },
 {
   'εc': -3.5,
   'εs1': 4,
   'ξ': 0.467,
   'ζ': 0.806,
   'μRd': 0.304,
   'ω': 0.378,
   'αv': 0.81,
   'ka': 0.416
 }
]";
        #endregion
    }
    public static class KofZaProracunPravougaonogPresekaPBAB 
    {
        public static List<KofZaProracunPravougaonogPresekaModelPBAB> GetListOfKoficientsPBAB(double percision = 0.05)
        {
            double εc = 0.05; double εs1 = -0.50;
            var result = new List<KofZaProracunPravougaonogPresekaModelPBAB>();

            for (double i = εc; i <= 3.5; i += percision)
            {
                var j = 10;
                var n = new KofZaProracunPravougaonogPresekaModelPBAB(-i, j);
                result.Add(n);
            }
            for (double j = εs1; j <= 10; j += percision)
            {
                var i = -3.5;
                var n = new KofZaProracunPravougaonogPresekaModelPBAB(i, j);
                result.Add(n);
            }
            return result;

        }
        public static KofZaProracunPravougaonogPresekaModelPBAB GetLimitKofZaProracunPravougaonogPresekaPBAB()
        {
            return new KofZaProracunPravougaonogPresekaModelPBAB(-3.5, 6.5);
        }

        private static List<KofZaProracunPravougaonogPresekaModelPBAB> GetListTacna_εb(double max_εa1, double min_εa1, double percision = 0.0001)
        {
            var result = new List<KofZaProracunPravougaonogPresekaModelPBAB>(); 
            var εc = Math.Abs(-3.5);

            for (double j = min_εa1; j <= max_εa1; j += percision)
            {
                var i = 3.5;
                var n = new KofZaProracunPravougaonogPresekaModelPBAB(-i, j);
                result.Add(n);
            }
            return result;
        }
        private static List<KofZaProracunPravougaonogPresekaModelPBAB> GetListTacna_εa1(double max_εb, double min_εb, double percision = 0.0001)
        {
            var result = new List<KofZaProracunPravougaonogPresekaModelPBAB>();

            for (double i = Math.Abs(min_εb); i <= Math.Abs(max_εb); i += percision)
            {
                var j = 10;
                var n = new KofZaProracunPravougaonogPresekaModelPBAB(-i, j);
                result.Add(n);
            }
            return result;
        }
        public static KofZaProracunPravougaonogPresekaModelPBAB Get_Kof_From_k(double k)
        { 
            var k_lim = new KofZaProracunPravougaonogPresekaModelPBAB(-3.5, 10);

            if (k > k_lim.k)
            {
                var kof_iz_tablice = GetItem_Full(k);
                var max_εa1 = kof_iz_tablice.εa1 + 0.5;
                var min_εa1 = kof_iz_tablice.εa1 - 0.5;
                var ListToSerachForKof = new List<KofZaProracunPravougaonogPresekaModelPBAB>();
                ListToSerachForKof.AddRange(GetListTacna_εa1(max_εa1, min_εa1));

                return GetSearchFork(k, ListToSerachForKof);
            }
            if (k < k_lim.k)
            {
                var kof_iz_tablice = GetItem_Full(k: k);
                var max_εb = kof_iz_tablice.εb + 0.5;
                var min_εb = kof_iz_tablice.εb - 0.5;
                var ListToSerachForKof = new List<KofZaProracunPravougaonogPresekaModelPBAB>();
                ListToSerachForKof.AddRange(GetListTacna_εb(max_εb, min_εb));

                return GetSearchFork(k, ListToSerachForKof);
            }
            return k_lim;

        }
        private static KofZaProracunPravougaonogPresekaModelPBAB GetSearchFork(double k, List<KofZaProracunPravougaonogPresekaModelPBAB> listToSearch)
        {
            var list = listToSearch;
            if (listToSearch.Count == 0)
            {
                var nul = new KofZaProracunPravougaonogPresekaModelPBAB(0, 20);
                return nul;
            }

            double closest = list.Select(x => x.k)
                .Select(n => new { n, distance = Math.Abs(n - k) })
                .OrderBy(p => p.distance)
                .First(i => i.n >= k).n;

            return listToSearch.Single(x => x.k == closest);
        }


        public static KofZaProracunPravougaonogPresekaModelPBAB GetItem_Full(double k)
        {
        var list = GetListOfKoficientsPBAB();
            var item = list.Single(n => n.k == GetItem_k(k, list));

            return item;
        }
        private static double GetItem_k(double k, List<KofZaProracunPravougaonogPresekaModelPBAB> listToSearch)
        {
            var list = listToSearch.Select(n => n.k);

            double closest = list
                .Select(n => new { n, distance = Math.Abs(n - k) })
                .OrderBy(p => p.distance)
                .First(i => i.n >= k).n;

            return closest;
        }
    }
}
