using System.Collections.Generic;

namespace SectionDesign
{
    public class SecDesingModel
    {
        private int def_daimetar = 14;
        private ListOfLongBars longBars; 
        private ListOfLikeBars linkBars;
        public double c_nom { get; internal set; } = 2.5;
        public double bc { get; internal set; }
        public double hc { get; internal set; }
        public double b0 { get; internal set; }
        public double h0 { get; internal set; }
        /// <summary>
        /// Returns volume of link bars
        /// </summary>
        public double Vh => linkBars.GetSumVh();
        public SecDesingModel(double bc, double hc)
        {
            this.bc = bc;
            this.hc = hc;
            Init();
        }

        /// <summary>
        /// Adding initial bars and links
        /// </summary>
        private void Init()
        {
            var diametar = def_daimetar;
            var X1 = (-bc + diametar / 10 + c_nom) / 2;
            var Y1 = (hc - diametar / 10 + c_nom) / 2;

            var longBars1= new List<LongBar>(
                new List<LongBar>
                {
                    new LongBar
                    {
                        diametar=diametar,
                        X=X1,
                        Y=Y1,
                    },
                    new LongBar
                    {
                        diametar=diametar,
                        X=-X1,
                        Y=Y1,
                    },
                    new LongBar
                    {
                        diametar=diametar,
                        X=-X1,
                        Y=-Y1,
                    },
                    new LongBar
                    {
                        diametar=diametar,
                        X=X1,
                        Y=-Y1,
                    },
                }
            );
            linkBars = new ListOfLikeBars();
            longBars = new ListOfLongBars(longBars1);
            AddLinkBar(8, longBars1);
            b0 = bc - 2 * c_nom - linkBars[0].diametar;
            h0 = hc - 2 * c_nom - linkBars[0].diametar;
            
           
        }
        /// <summary>
        /// Adding two bars by given diametar
        /// </summary>
        /// <param name="diametar">diametar of bar in [mm]</param>
        public void AddLonitudinalBar(int diametar)
        {
            longBars.Add(new LongBar { diametar=diametar });
        }
        /// <summary>
        /// Adding link bar by given diametar and list of bar to be linked
        /// </summary>
        /// <param name="diametar">diametar of bar in [mm]</param>
        /// <param name="longBarsToLink">List of bars to be linked</param>
        public void AddLinkBar(int diametar, List<LongBar> longBarsToLink)
        {
            longBarsToLink.ForEach(x =>
            {
                x.IsLinked = true;
            });
            linkBars.Add(new LinkBar
            {
                diametar = diametar,
                linkedBars = longBarsToLink
            });
        }

        /// <summary>
        /// Remove last two bars
        /// </summary>
        public void RemoveLonitudinalBar()
        {
            for (int i = 0; i < 2; i++)
            {
                if (longBars.Count <= 4) return;
                longBars.RemoveAt(longBars.Count - 1);
            }
        }

    }
}
