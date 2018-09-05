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

            var x = new List<double>();
            var y = new List<double>();


            //r.ForEach(n => points.Add(new Point( n.Mbh, n.NbhPower2)));

            //var l= new List<List<Point>>();
            //l.Add(points);
            //list.ItemsSource = l;
            Grid g = new Grid();
            var l = new List<List<Point>>();
            for (int i = 0; i < sym.GetAllLines().Count; i++)
            {
                l.Add(new List<Point>());
                var r = sym.GetAllLines()[i];
                var toRemove = new SymmetricalReinfByClassicMethod.μSd_And_νSdCollection();
                r.ForEach(z => { if (z.μSd < 0 || z.νSd < 0) toRemove.Add(z); });
                toRemove.ForEach(z => r.Remove(z));

                r.ForEach(n => x.Add(n.μSd));
                r.ForEach(n => y.Add(n.νSd));
                r.ForEach(n => l[i].Add( new Point(n.μSd, n.νSd)));
                LineGraph lineGraph = new LineGraph();
                lineGraph.Plot(x, y);
                var a = r.First(t => t.νSd == r.Max(n=>n.νSd) );
                //g.Children.Add(lineGraph);
            }
            // Char.Content = g;
            //var c = new Chart();
            
            list.DataContext = l;
        }
    }
}
