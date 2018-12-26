using System;
using System.Collections.Generic;
using System.Linq;
using TabeleEC2.Model;

namespace CalculatorEC2Logic
{
    public class SymmetricalReinfByClassicMethod
    {
        private IMaterial _material;
        private IElementGeometry _geometry;

        /// <summary>
        /// 0.85 by defoult
        /// </summary>
        public double αcc { get; set; } = 0.85;

        private int iterations;
        private μSd_And_νSd find;
        private Generate_ω_LineForDiagram minOf_ω;
        private Generate_ω_LineForDiagram maxOf_ω;
        public Generate_ω_LineForDiagram searchingOf_ω { get; internal set; }

        public SymmetricalReinfByClassicMethod(IMaterial material, IElementGeometry geometry)
        {
            _material = material;
            _geometry = geometry;
            SetMinimumOf_ω_and_Max();
        }

        private void SetMinimumOf_ω_and_Max()
        {
            minOf_ω = new Generate_ω_LineForDiagram(_material, _geometry, 0.05);
            maxOf_ω = new Generate_ω_LineForDiagram(_material, _geometry, 1);
        }

        public double Get_ω2(double MRd, double NRd)
        {
            var geo = _geometry;

            var mi = MRd * 1000000 / (Math.Pow(geo.d * 10, 2) * geo.b * 10 * _material.beton.fcd);
            var ni = NRd * 1000 / (geo.d * 10 * geo.b * 10 * _material.beton.fcd);
            return Get_ω(mi, ni);
        }

        public double Get_ω(double μRd, double νRd)
        {
            var ω = maxOf_ω.ω / 2;
            double addTo_ω = maxOf_ω.ω / 2;
            for (int i = 0; i < 50; i++)
            {
                addTo_ω = addTo_ω / 2;
                if (ω < 0.05) break;
                searchingOf_ω = new Generate_ω_LineForDiagram(_material, _geometry, ω);
                switch (CheckDiagram(searchingOf_ω, μRd, νRd))
                {
                    case 0:
                        return ω;
                    case 1:
                        ω -= addTo_ω;
                        break;
                    case 2:
                        ω += addTo_ω;
                        break;
                    default:
                        break;
                }

                iterations = i;
            }

            if (ω > 1.0)
                throw new Exception("the percentage of reinforcement has been exceeded, make cross-section bigger");
            if (ω < 0.05)
            {
                searchingOf_ω = minOf_ω;
                return 0.05;
            };
            return ω;
        }

        public double Get_As(double MRd, double NRd)
        {
            for (double i = -3.5; i <= -0.1; i += 0.001)
            {
                var item = new μSd_And_νSd(_geometry, _material).GetFromKof(new CoeffForCalcRectCrossSectionModelEC(i, 20));
                var As1 = (NRd - 0.85 * _material.beton.fcd * item.kof.αv * item.kof.ξ * _geometry.d * _geometry.b) / (item.σs2 - item.σs1);
                var As2 = Math.Abs(MRd * 100 - 0.85 * _material.beton.fcd * item.kof.αv * item.kof.ξ * _geometry.d * _geometry.b * (_geometry.h / 2 - item.kof.ka * item.x)) / ((_geometry.h / 2 + _geometry.d1) * Math.Abs((item.σs2 + item.σs1)));


                if (Math.Round(As1, 2) == Math.Round(As2, 2)) return As1 * 2;

            }
            for (double i = 19.9; i > -1.5; i -= 0.001)
            {
                var item = new μSd_And_νSd(_geometry, _material).GetFromKof(new CoeffForCalcRectCrossSectionModelEC(-3.5, i));

                var As1 = (NRd - 0.85 * _material.beton.fcd * item.kof.αv * item.kof.ξ * _geometry.d * _geometry.b) / (item.σs2 - item.σs1);
                var As2 = Math.Abs(MRd * 100 - 0.85 * _material.beton.fcd * item.kof.αv * item.kof.ξ * _geometry.d * _geometry.b * (_geometry.h / 2 - item.kof.ka * item.x)) / ((_geometry.h / 2 + _geometry.d1) * Math.Abs((item.σs2 + item.σs1)));

                if (Math.Round(As1, 2) == Math.Round(As2, 2)) return As1 * 2;
            }
            return 0;
        }
        
    private int CheckDiagram(Generate_ω_LineForDiagram toCheck, double μRd, double νRd, int percision = 3)
        {
            var test = new List<μSd_And_νSd>(toCheck.ListOfDotsInLineOfDiagram);
            var νSdmax = test.Single(m => m.μSd == test.Max(n => n.μSd)).νSd;
            if (νRd >= νSdmax)
            {
                test.RemoveAll(n => n.νSd < νSdmax);
                var closestItemByM = test
                                 .Aggregate((x, y) => Math.Abs(x.μSd - μRd) < Math.Abs(y.μSd - μRd) ? x : y);

                if (
                    (Math.Round(μRd, percision) == Math.Round(closestItemByM.μSd, percision))
                    &&
                    (Math.Round(νRd, percision) == Math.Round(closestItemByM.νSd, percision)))
                {
                    find = closestItemByM;
                    return 0;
                }
                else if (closestItemByM.νSd > νRd) { find = closestItemByM; return 1; }
                else { find = closestItemByM; return 2; };

            }
            else
            {
                test.RemoveAll(n => n.νSd > νSdmax);
                var closestItemByM = test
                                 .Aggregate((x, y) => Math.Abs(x.μSd - μRd) < Math.Abs(y.μSd - μRd) ? x : y);

                if (
                    (Math.Round(μRd, percision) == Math.Round(closestItemByM.μSd, percision))
                    &&
                    (Math.Round(νRd, percision) == Math.Round(closestItemByM.νSd, percision)))
                {
                    find = closestItemByM;
                    return 0;
                }
                else if (closestItemByM.νSd < νRd) { find = closestItemByM; return 1; }
                else { find = closestItemByM; return 2; };
            }

        }

        public string TextResult()
        {
            var geo = _geometry as ElementGeometry;
            if (find == null) return "No result!";
            return $@"///////Result////////{Environment.NewLine }" +
                $"Material:{Environment.NewLine }" +
                $"  beton: {_material.beton.ToString()}{Environment.NewLine }" +
                $"  armatura: {_material.armatura.ToString()}{Environment.NewLine }" +
                $"Geometrija:{Environment.NewLine }" +
                $"  h: {geo.h}{geo.GetUnits()}{Environment.NewLine }" +
                $"  b: {geo.b}{geo.GetUnits()}{Environment.NewLine }" +
                $"  d1: {geo.d1}{geo.GetUnits()}{Environment.NewLine }" +
                $"Sile{Environment.NewLine }" +
                $"  μRd: {Math.Round(find.μSd, 2)}{Environment.NewLine }" +
                $"  MRd: {Math.Round(find.μSd/100 * Math.Pow(geo.d, 2) * geo.b * _material.beton.fcd/10, 2)} kNm{Environment.NewLine }" +
                $"  νRd: {Math.Round(find.νSd, 2)}{Environment.NewLine }" +
                $"  NRd: {Math.Round(find.νSd * geo.d  * geo.b  * _material.beton.fcd/10, 2)} kN{Environment.NewLine }" +
                $"  Mehanički koficijent armiranja: {Math.Round(searchingOf_ω.ω, 3)}{Environment.NewLine }" +
                $"  Iteracije: {iterations}";

        }

        public class Generate_ω_LineForDiagram
        {
            private readonly IMaterial _material;
            private readonly IElementGeometry _geometry;
            public double ω { get; internal set; }

            public μSd_And_νSdCollection ListOfDotsInLineOfDiagram { get; set; }
            /// <summary>
            /// 0.85 by defoult
            /// </summary>
            public double αcc { get; set; }

            public Generate_ω_LineForDiagram(IMaterial material, IElementGeometry geometry, double ω, double αcc = 0.85)
            {
                _material = material;
                _geometry = geometry;
                this.ω = ω;
                this.αcc = αcc;
                ListOfDotsInLineOfDiagram = new μSd_And_νSdCollection(_geometry, _material, ω);
            }
        }

        public class μSd_And_νSdCollection : List<μSd_And_νSd>
        {
            public μSd_And_νSdCollection(IElementGeometry geometry, IMaterial material, double ω)
            {
                if (geometry == null)
                {
                    throw new ArgumentNullException(nameof(geometry));
                }

                if (material == null)
                {
                    throw new ArgumentNullException(nameof(material));
                }


                for (double i = -3.5; i <= -0.1; i += 0.1)
                {
                    var item = new μSd_And_νSd(geometry, material);
                    Add(item.GetFromKof(new CoeffForCalcRectCrossSectionModelEC(i, 20), ω));
                }
                for (double i = 19.9; i > -1.5; i -= 0.1)
                {
                    var item = new μSd_And_νSd(geometry, material);
                    Add(item.GetFromKof(new CoeffForCalcRectCrossSectionModelEC(-3.5, i), ω));
                }

                var t = this;
            }
            public μSd_And_νSdCollection()
            {

            }
        }
        public class μSd_And_νSd
        {
            private readonly IElementGeometry geometry;
            private readonly IMaterial material;

            public double μSd { get; internal set; }
            public double νSd { get; internal set; }
            public CoeffForCalcRectCrossSectionModelEC kof { get; internal set; }
            public double εs2 { get; internal set; }
            public double σs1 { get; internal set; }
            public double σs2 { get; internal set; }
            public double x { get; internal set; }
            public μSd_And_νSd(IElementGeometry geometry, IMaterial material)
            {
                this.geometry = geometry ?? throw new ArgumentNullException(nameof(geometry));
                this.material = material ?? throw new ArgumentNullException(nameof(material));
            }
            public μSd_And_νSd GetFromKof(CoeffForCalcRectCrossSectionModelEC kof, double ω)
            {
                this.kof = kof ?? throw new ArgumentNullException(nameof(kof));
                var fyd = material.armatura.fyd * 10;

                x = geometry.d * kof.ξ;

                εs2 = kof.εc / x * (x - geometry.d1);

                var sigma = kof.εs1 * material.armatura.Es;
                σs1 = sigma > fyd ? fyd : sigma;

                sigma = Math.Abs(εs2) * material.armatura.Es;
                σs2 = sigma > fyd ? fyd : sigma;

                νSd = 0.85 * kof.αv * kof.ξ + ω * ((σs2 / fyd) - (σs1 / fyd));
                μSd = 0.85 * kof.αv * kof.ξ * (geometry.h /geometry.d * 0.5 - kof.ka * kof.ξ) + ω * (geometry.h / geometry.d * 0.5 - geometry.d1 /geometry.d) * Math.Abs((σs2 / fyd) + (σs1 / fyd));

                return this;
            }
            public μSd_And_νSd GetFromKof(CoeffForCalcRectCrossSectionModelEC kof)
            {
                this.kof = kof ?? throw new ArgumentNullException(nameof(kof));
                var fyd = material.armatura.fyd * 10;

                x = geometry.d * kof.ξ;

                εs2 = kof.εc / x * (x - geometry.d1);

                var sign = kof.εs1 / Math.Abs(kof.εs1);
                var sigma = kof.εs1 * material.armatura.Es;
                σs1 = sigma > fyd ? sign*fyd : sign*sigma;

                sign = εs2 / Math.Abs(εs2);
                sigma = Math.Abs(εs2) * material.armatura.Es;
                σs2 = sigma > fyd ? sign * fyd : sign * sigma;

                return this;
            }
        }


        public List<μSd_And_νSdCollection> GetAllLines()
        {
            var result = new List<μSd_And_νSdCollection>();
            var r = new Generate_ω_LineForDiagram(_material, _geometry, 0.05);
            result.Add(r.ListOfDotsInLineOfDiagram);
            for (double i = 0.1; i <= 1; i += 0.1)
            {
                r = new Generate_ω_LineForDiagram(_material, _geometry, i);
                result.Add(r.ListOfDotsInLineOfDiagram);
            }

            return result;
        }
    }
}