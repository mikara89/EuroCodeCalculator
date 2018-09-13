using CalculatorEC2Logic;
using InteractiveDataDisplay.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TabeleEC2;
using static CalculatorEC2Logic.SymmetricalReinfByMaxAndMinPercentageReinf;

namespace TestingCharts
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            
            InitializeComponent();
            testqq();

        }

        private void testqq()
        {
            var material = new Material()
            {
                beton = TabeleEC2.BetonClasses.GetBetonClassListEC().First(n => n.name == "C25/30"),
                armatura = ReinforcementType.GetArmatura().First(n => n.name == "B500B"),
            };
            var geometry = new ElementGeomety()
            {
                b = 20,
                d1 = 4,
                h = 40,
            };
            var sym = new SymmetricalReinfByClassicMethod(material, geometry);
            var random = new Random();
            var l = new List<PointCollection>();
            for (int i = 0; i < sym.GetAllLines().Count; i++)
            {
                l.Add(new PointCollection());
                var r = sym.GetAllLines()[i];
                var x = new List<double>();
                var y = new List<double>();
                r.ForEach(n => { x.Add(n.μSd); y.Add(n.νSd); });


                var lg = new LineGraph();
                lines.Children.Add(lg);
                lg.Stroke = new SolidColorBrush(Color.FromArgb(255, GetRandumByte(random), GetRandumByte(random), GetRandumByte(random)));
                lg.Description = String.Format("w= {0}", i==0?0.05:Convert.ToDouble(i)/10);
                lg.StrokeThickness = 2;
                lg.Plot(x, y);

            }
        }
        private byte GetRandumByte(Random random)
        {
            return Convert.ToByte(random.Next(0, 256));
        }

        public class DisplayRange
        {
            public double Start { get; set; }
            public double End { get; set; }

            public DisplayRange(double start, double end)
            {
                Start = start;
                End = end;
            }
        }

       
    }
}
