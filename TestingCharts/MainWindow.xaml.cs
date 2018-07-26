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
using static CalculatorEC2Logic.SymmetricalReinforsmentOfColumn;

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
            var m = new Material()
            {
                beton = TabeleEC2.BetonClasses.GetBetonClassListEC().First(n => n.name == "C30/37"),
                armatura = ReinforcementType.GetArmatura().First(n => n.name == "B500B"),
            };
            var s = new Generate_ρ_LineForDiagram(m);
            s.GetLineForDiagram();
            var r = s.ListOfDotsInLineOfDiagram;
            var points = r.Select(n =>new Point( n.Mbh, n.NbhPower2)).AsEnumerable();

            var l= new List<IEnumerable<Point>>();
            l.Add(points);
            list.ItemsSource = l;

         
        }
    }
}
