using System;
using System.Collections.Generic;
using System.Linq;
using TabeleEC2.Model;

namespace TabeleEC2
{
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
