using ChartJs.Blazor.ChartJS.Common;
using ChartJs.Blazor.ChartJS.Common.Axes;
using ChartJs.Blazor.ChartJS.Common.Axes.Ticks;
using ChartJs.Blazor.ChartJS.Common.Enums;
using ChartJs.Blazor.ChartJS.Common.Properties;
using ChartJs.Blazor.ChartJS.LineChart;
using ChartJs.Blazor.ChartJS.MixedChart;
using ChartJs.Blazor.Charts;
using ChartJs.Blazor.Util;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CalcBlazor.Components
{
    partial class ChartInterComponent
    {
        private int Selecteditem=-1;
        private int widthChart = 500;
        private int heightChart = 500; 
        private int borderWidth = 2;
        private int pointRadius = 5;
        public ObservableCollection<IEnumerable<Point>> DiagramDatas { get; set; }
        public ObservableCollection<Point> ForcesDatas { get; set; } 



        private LineConfig _config;
        private ChartJsLineChart _lineChartJs;
        
        protected override void OnInitialized()
        {
            _config = new LineConfig
            {
                Options = new LineOptions
                {
                    Title = new OptionsTitle
                    {
                        Display = true, 
                        Text = "Interaction chart"
                    },
                    Responsive = true,
                    Scales = new Scales
                    {
                        xAxes = new List<CartesianAxis>()
                        {
                            new LinearCartesianAxis {
                                Display = AxisDisplay.True,
                                Position = Position.Bottom,
                                ScaleLabel = new ScaleLabel { Display = true, LabelString = "MRd" }
                            }
                        },
                        yAxes = new List<CartesianAxis>()
            {
                        new LinearCartesianAxis {

                            Display = AxisDisplay.True,
                            Position = Position.Bottom,
                            ScaleLabel = new ScaleLabel { Display = true, LabelString = "NRd" },
                            Ticks= new LinearCartesianTicks
                            {
                                 Reverse=true,
                                BeginAtZero=false, 
                            }
                        }
                    },
                    }
                }
            };
            DiagramDatas = intCalcService.ResultsAsPoint;
            ForcesDatas = intCalcService.ForcesPoints;
            SetDiagramDatasActions();
            SetForcesDatasActions();
        }

        private void resetLabelText()
        {
            int i=0;
            _config.Data.Datasets.ToList().ForEach(x =>
            {
                if((x as LineDataset<Point>).Label.Contains("Sec"))
                {
                    i++;
                    (x as LineDataset<Point>).Label = "Sec " + i;
                }
            });
            i = 0;
            _config.Data.Datasets.ToList().ForEach(x =>
            {
                if ((x as LineDataset<Point>).Label.Contains("Point"))
                {
                    i++;
                    (x as LineDataset<Point>).Label = "Point " + i;
                }
            });
        }
        private void SetDiagramDatasActions()
        {
           DiagramDatas.CollectionChanged += async(s, e) =>
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    var newEle = e.NewItems;
                    for (int i = 0; i < newEle.Count; i++)
                    {
                        var color = ColorUtil.RandomColorString();
                        var ele = newEle[i] as IEnumerable<Point>;
                        var lineSet = new LineDataset<Point>(newEle[i] as IEnumerable<Point>)
                        {
                            Label = "Sec " + (DiagramDatas.Count()),
                            BackgroundColor = color,
                            BorderWidth = borderWidth,
                            BorderColor = color,
                            PointHitRadius = 5,
                            PointHoverRadius = 5,
                            
                        };
                        _config.Data.Datasets.Add(lineSet);

                    }
                }
                else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
                {
                    var newEle = e.OldItems;
                    for (int i = 0; i < newEle.Count; i++)
                    {
                        var ele = _config.Data.Datasets.ToList().Find((x) =>
                            (x as LineDataset<Point>).Label == "Sec " + (e.OldStartingIndex + 1));

                        _config.Data.Datasets.Remove(ele);
                    }
                    resetLabelText();
                    OnChangedSecetion(-1);
                }
                else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Replace)
                {
                    var oldEle = e.OldItems;
                    var newEle = e.NewItems;
                    for (int i = 0; i < oldEle.Count; i++)
                    {
                        var ele = _config.Data.Datasets.ToList().Find((x) =>
                            (x as LineDataset<Point>).Label == "Sec " + (e.OldStartingIndex + 1)) as LineDataset<Point>;

                        ele.RemoveAll(x => true);
                        ele.AddRange(newEle[i] as IEnumerable<Point>);

                    }
                    OnChangedSecetion(-1);
                }
                await InvokeAsync(this.StateHasChanged);
            };
            
        }
        private void SetForcesDatasActions()
        {
            ForcesDatas.CollectionChanged += (s, e) =>
            {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    var newEle = e.NewItems;
                    for (int i = 0; i < newEle.Count; i++)
                    {
                        var color = ColorUtil.RandomColorString();
                        var ele = new List<Point> { newEle[i] as Point };
                        var lineSet = new LineDataset<Point>(ele)
                        {
                            Label = "Point " + (ForcesDatas.Count()),
                            PointBackgroundColor = color,
                            PointBorderWidth=borderWidth,
                            PointHitRadius=pointRadius,
                            PointHoverBorderWidth= borderWidth,
                            PointRadius= pointRadius,
                            PointHoverRadius= pointRadius*12/10,
                        };
                        _config.Data.Datasets.Add(lineSet);

                    }
                }
                else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
                {
                    var newEle = e.OldItems;
                    for (int i = 0; i < newEle.Count; i++)
                    {
                        var ele = _config.Data.Datasets.ToList().Find((x) =>
                            (x as LineDataset<Point>).Label == "Point " + (e.OldStartingIndex + 1));

                        _config.Data.Datasets.Remove(ele);
                    }
                    resetLabelText();
                }
                else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Replace)
                {
                    var oldEle = e.OldItems;
                    var newEle = e.NewItems;
                    for (int i = 0; i < oldEle.Count; i++)
                    {
                        var ele = _config.Data.Datasets.ToList().Find((x) =>
                            (x as LineDataset<Point>).Label == "Point " + (e.OldStartingIndex + 1)) as LineDataset<Point>;

                        ele.RemoveAll(x => true);
                        ele.AddRange(newEle[i] as IEnumerable<Point>);

                    }
                }
                InvokeAsync(this.StateHasChanged);
            };

        }
        private void OnChangedSecetion(int i)
        {
            var IsCheck = i==Selecteditem;
            Selecteditem = IsCheck ? -1 : i;
            intCalcService.SelectedItem(Selecteditem);
            highlightLine(Selecteditem);
        }
        private void OnDeleteItem(int index)
        {
            DiagramDatas.RemoveAt(index);
            var a = index;
        }
        private void ResetColor(int index) 
        {
            var newColor= ColorUtil.RandomColorString();
            _config.Data.Datasets.ToList().ForEach(x =>
            {
                var l = (x as LineDataset<Point>);
                if (l.Label== "Sec " + (index+1))
                {
                    l.BackgroundColor = newColor;
                    l.BorderColor = newColor;
                }
            });
        }
        private void highlightLine(int index) 
        {
            _config.Data.Datasets.ToList().ForEach(x =>
            {
                var l = (x as LineDataset<Point>);
                if (l.Label== "Sec " + (index + 1))
                {
                    l.Fill = true;
                }
                else
                {
                    l.Fill = false;
                }
            });
        }
    }
}
