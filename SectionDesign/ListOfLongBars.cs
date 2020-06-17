using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace SectionDesign
{
    public class ListOfLongBars : ObservableCollection<LongBar>
    {
        private bool IsManagedAdd;
        public ListOfLongBars(List <LongBar> initList ):base(initList)
        {}
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            var Xmax = this
                .Where(x => x.X== this.Max(c => c.X))
                .OrderBy(x=>x.Y)
                .ToList();
            var Xmin = this
                .Where(x => x.X == this.Min(c => c.X))
                .OrderBy(x => x.Y)
                .ToList();
            var Ymax = this
                .Where(y => y.Y == this.Max(c => c.Y))
                .OrderBy(x => x.X).
                ToList();
            var Ymin = this
                .Where(y => y.Y == this.Min(c => c.Y)) 
                .OrderBy(x => x.X).
                ToList();
            if (e.Action== NotifyCollectionChangedAction.Add)
            {
                if (IsManagedAdd) 
                    IsManagedAdd=false;
                else if (GetDistance(Xmax[0], Xmax[1]) >= GetDistance(Ymax[0], Ymax[1]))
                {
                    var dY = GetDistance(Xmax.First(), Xmax.Last()) / Xmax.Count;
                    for (int i = 1; i < Xmax.Count - 1; i++)
                    {
                        Xmax[i].Y = Xmax[i - 1].Y + dY;
                        Xmin[i].Y = Xmin[i - 1].Y + dY;
                    }

                    var newBar1 = e.NewItems[0] as LongBar;
                    newBar1.X = Xmax[0].X;
                    newBar1.Y = Xmax[Xmax.Count() - 2].Y + dY;

                    var newBar2 = new LongBar
                    {
                        X = Xmin[0].X,
                        Y = Xmin[Xmin.Count() - 2].Y + dY,
                        diametar = newBar1.diametar,
                    };
                    IsManagedAdd = true;
                    base.Add(newBar2);
                }
                else
                {
                    var dX = GetDistance(Ymax.First(), Ymax.Last()) / Ymax.Count;
                    for (int i = 1; i < Ymax.Count - 1; i++)
                    {
                        Ymax[i].Y = Ymax[i - 1].Y + dX;
                        Ymin[i].Y = Ymin[i - 1].Y + dX;
                    }

                    var newBar1 = e.NewItems[0] as LongBar;
                    newBar1.Y = Ymax[0].Y;
                    newBar1.X = Ymax[Ymax.Count() - 2].X + dX;

                    var newBar2 = new LongBar
                    {
                        Y = Ymin[0].Y,
                        X = Ymin[Ymin.Count() - 2].X + dX,
                        diametar = newBar1.diametar,
                    };
                    IsManagedAdd = true;
                    base.Add(newBar2);
                }
                
            }
            else if (e.Action== NotifyCollectionChangedAction.Remove)
            {
              

                if (!IsManagedAdd)
                {
                    var dX = GetDistance(Ymax.First(), Ymax.Last()) / (Ymax.Count - 1);
                    for (int i = 1; i < Ymax.Count - 1; i++)
                    {
                        Ymax[i].X = Ymax[i - 1].X + dX; 
                        Ymin[i].X= Ymin[i - 1].X + dX;
                    }

                    var dY = GetDistance(Xmax.First(), Xmax.Last()) / (Xmax.Count - 1);
                    for (int i = 1; i < Xmax.Count - 1; i++)
                    {
                        Xmax[i].Y = Xmax[i - 1].Y + dY;
                        Xmin[i].Y = Xmin[i - 1].Y + dY;
                    }
                }
            }
            
        }

        /// <summary>
        /// Sorting bars in Clockwise 
        /// </summary>
        public void SortClockwise() 
        {
            var sortableList = new List<LongBar>(Items.AsEnumerable());
            sortableList= sortableList.OrderBy(x => Math.Atan2(x.X, x.Y)).ToList();
            for (int i = 0; i < sortableList.Count; i++)
            {
                Move(IndexOf(sortableList[i]), i);
            }
        }

        /// <summary>
        /// Removes last two bars
        /// </summary>
        public void Remove()
        {
            IsManagedAdd = true;
            this.Remove(this.Last());
            IsManagedAdd = false;
            this.Remove(this.Last()); 
        }
        public void Add(int diametar)
        {
            this.Add(new LongBar 
            {diametar=diametar }
            );
        }
        public List<Tuple<int, double>> GetLinkedDistance()
        {
            var sortableList = this
                .Where(x=>x.IsLinked)
                .OrderBy(x => Math.Atan2(x.X, x.Y))
                .ToList();
            var result =new List<Tuple<int, double>>();
            var distances = new List<double>();
            for (int i = 0; i < sortableList.Count(); i++)
            {
                LongBar nextP;
                if (i == sortableList.Count()-1)
                    nextP = sortableList[0];
                else
                    nextP = sortableList[i + 1];

                var thisP = sortableList[i];
                var dist = GetDistance(thisP, nextP);
                if (result.Any(item => item.Item2 == dist))
                {
                    var item1 = result.First(x => x.Item2 == dist).Item1 + 1;
                    result.Remove(result.First(x => x.Item2 == dist));
                    result.Add(new Tuple<int, double>(item1, dist));
                }
                else
                {
                    result.Add(new Tuple<int, double>(1, dist));
                }
            }
            return result;
        }

        private static double GetDistance(LongBar longBar1, LongBar longBar2)
        {
            return Math.Sqrt(Math.Pow((longBar2.X - longBar1.X), 2) + Math.Pow((longBar2.Y - longBar1.Y), 2));
        }
    }
}
