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

     

        private Generate_ω_LineForDiagram minOf_ω;
        private Generate_ω_LineForDiagram maxOf_ω;
        private Generate_ω_LineForDiagram searchingOf_ω; 
         
        public SymmetricalReinfByClassicMethod(IMaterial material, IElementGeometry geometry) 
        {
            _material = material;
            _geometry = geometry;
            SetMinimumOf_ρ_and_Max();
        }

        private void SetMinimumOf_ρ_and_Max()
        {
            minOf_ω = new Generate_ω_LineForDiagram(_material, _geometry,0.05);
            maxOf_ω = new Generate_ω_LineForDiagram(_material, _geometry, 1);
            maxOf_ω = new Generate_ω_LineForDiagram(_material, _geometry, 1);
        }
        public double Get_ω(double μRd, double νRd) 
        {
            var ω = maxOf_ω.ω / 2;
            double addTo_ω = maxOf_ω.ω / 2;
            for (int i = 0; i < 20; i++)
            {
                
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
                addTo_ω = addTo_ω / 2;
            }
            if (ω > 1.0)
                throw new Exception("the percentage of reinforcement has been exceeded, make cross-section bigger");
            if (ω < 0.05) return 0.05;
            return ω;
        }

        private int CheckDiagram(Generate_ω_LineForDiagram toCheck, double μRd, double νRd)
        {
            var test = toCheck.ListOfDotsInLineOfDiagram;

            var νSdmax = test.Single(m => m.μSd == test.Max(n => n.μSd)).νSd;
            if (νRd >= νSdmax)
            {
                test.RemoveAll(n => n.νSd < νSdmax);
                var closestItemByM = test
                                 .Aggregate((x, y) => Math.Abs(x.μSd - μRd) < Math.Abs(y.μSd - μRd) ? x : y);

                if (
                    (μRd - 0.005 >= closestItemByM.μSd && closestItemByM.μSd <= μRd + 0.005)
                    &&
                    (νRd - 0.005 >= closestItemByM.νSd && closestItemByM.νSd >= νRd + 0.005))
                {
                    return 0;
                }
                else if (closestItemByM.νSd > νRd) return 1; 
                else return 2;

            }
            else
            {
                test.RemoveAll(n => n.νSd > νSdmax);
                var closestItemByM = test
                                 .Aggregate((x, y) => Math.Abs(x.μSd - μRd) < Math.Abs(y.μSd - μRd) ? x : y);

                if (
                    (μRd - 0.002 >= closestItemByM.μSd && closestItemByM.μSd <= μRd + 0.002)
                    &&
                    (νRd - 0.002 >= closestItemByM.νSd && closestItemByM.νSd >= νRd + 0.002))
                {
                    return 0;
                }
                else if (closestItemByM.νSd < νRd) return 1;
                else return 2;
            }

        }

        public class Generate_ω_LineForDiagram
        {
            private readonly IMaterial _material;
            private readonly IElementGeometry _geometry;
            public double ω { get; internal set; }

            public List<μSd_And_νSd> ListOfDotsInLineOfDiagram { get; set; } 
            /// <summary>
            /// 0.85 by defoult
            /// </summary>
            public double αcc { get; set; }

            public Generate_ω_LineForDiagram(IMaterial material, IElementGeometry geometry,double ω, double αcc=0.85) 
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
            public μSd_And_νSdCollection(IElementGeometry geometry, IMaterial material,double ω)
            {
                if (geometry == null)
                {
                    throw new ArgumentNullException(nameof(geometry));
                }

                if (material == null)
                {
                    throw new ArgumentNullException(nameof(material));
                }

                var item = new μSd_And_νSd( geometry,  material);
                for (double i = -3.5; i < 0; i+=0.1)
                {
                    Add(item.GetFromKof(new KofZaProracunPravougaonogPresekaModelEC(i, 20), ω));
                }
                for (double i = 19.9; i < -1.5; i -= 0.1)
                {
                    Add(item.GetFromKof(new KofZaProracunPravougaonogPresekaModelEC(-3.5, i), ω));
                }
            }
        }
        public class μSd_And_νSd
        {
            private readonly IElementGeometry geometry;
            private readonly IMaterial material; 

            public double μSd { get; internal set; }
            public double νSd { get; internal set; }
            public KofZaProracunPravougaonogPresekaModelEC kof { get; internal set; } 
            public double εs2 { get; internal set; } 
            public double σs1 { get; internal set; } 
            public double σs2 { get; internal set; }
            public double x { get; internal set; }
            public μSd_And_νSd(IElementGeometry geometry,IMaterial material)
            {
                this.geometry = geometry ?? throw new ArgumentNullException(nameof(geometry));
                this.material = material ?? throw new ArgumentNullException(nameof(material));
            }
            public μSd_And_νSd GetFromKof(KofZaProracunPravougaonogPresekaModelEC kof,double ω) 
            {
                this.kof = kof ?? throw new ArgumentNullException(nameof(kof));
                x = (geometry as ElementGeomety).d * kof.ξ;
                εs2 = kof.εc / x * (x - geometry.d1);
                var sigma = kof.εs1 * material.armatura.Es;
                σs1 = sigma > material.armatura.fyd ? material.armatura.fyd : sigma;
                sigma = Math.Abs(εs2) * material.armatura.Es;
                σs2 = sigma > material.armatura.fyd ? material.armatura.fyd : sigma;

                νSd = 0.85 * kof.αv * kof.ξ + ω * ((σs2 / material.armatura.fyd) - (σs1 / material.armatura.fyd));
                μSd = 0.85 * kof.αv * kof.ξ * (geometry.h / (geometry as ElementGeomety).d * 0.5 - kof.ka * kof.ξ) + ω * (geometry.h / (geometry as ElementGeomety).d * 0.5 - geometry.d1 / (geometry as ElementGeomety).d) * Math.Abs((σs2 / material.armatura.fyd) + (σs1 / material.armatura.fyd));

                return this;
            }
        }
    }
}