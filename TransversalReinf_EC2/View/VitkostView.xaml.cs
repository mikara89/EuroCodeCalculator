using CalculatorEC2Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Windows.Input;
using TabeleEC2.Model;
using TransversalReinf_EC2.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace TransversalReinf_EC2.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VitkostView : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetValue<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value))
                return;
            backingField = value;
            OnPropertyChanged(propertyName);
        }


        private ImageSource _image;

        public ImageSource image
        {
            get { return _image; }
            set
            {
                if (_image != value)
                {
                    SetValue(ref _image, value);
                }
            }
        }
        private double _k;

        public double k
        {
            get { return _k; }
            set
            {
                if (_k != value)
                {
                    SetValue(ref _k, value);
                }
            }
        }
        public ICommand CalculateCommand => new CommandHandler(async p => await IzracunajAsync());
        public VitkostView()
        {
            this.InitializeComponent();
            var list = Enum.GetNames(typeof(Izvijanja));
            cmbIzvijanje.ItemsSource = list;
            cmbIzvijanje.SelectedValue = Enum.GetName(typeof(Izvijanja), Izvijanja.Ukljesten_Sa_Jedne);
            settingK(cmbIzvijanje);
            cmbBeton.ItemsSource = TabeleEC2.BetonClasses.GetBetonClassListEC();
            cmbBeton.SelectedIndex = 3;
            cmbArmatura.ItemsSource = TabeleEC2.ReinforcementType.GetArmatura();
            cmbArmatura.SelectedIndex = 0;
            txtN.Text = "" + 1620;
            txtMt.Text = "" + 38.5;
            txtMb.Text = "" + (-38.5);
            txtL.Text = "" + 375;
            txtb.Text = "" + 30;
            txth.Text = "" + 30;
            txtd1.Text = "" + 4;
            cmbIzvijanje.SelectionChanged += (s, e) => 
            {
                settingK(s as ComboBox);
            };
            //btnIzracunaj.Click +=
            //    async (s, e) =>
            //    {
            //        await IzracunajAsync();
            //    };
            //IzracunajAsync();
            Imagechange();


            cmbIzvijanje.SelectionChanged += (s, e) => { Imagechange(); };
        }
        private void settingK(ComboBox cmb) 
        {
            var selected = cmb.SelectedItem as string;
            k = OjleroviSlucajeviIzvijanja.GetK((Izvijanja)Enum.Parse(typeof(Izvijanja), cmbIzvijanje.SelectedValue.ToString()));
        }

        private async Task IzracunajAsync()
        {
            try
            {
                if (txtN.Text == "" || txtMb.Text == "" ||
                txtMt.Text == "" || txtL.Text == "" || txtb.Text == "" ||
                txth.Text == "" || txtd1.Text == "" ||
                Convert.ToDouble(txtL.Text) == 0 || Convert.ToDouble(txtL.Text) < 0 ||
                Convert.ToDouble(txtb.Text) == 0 || Convert.ToDouble(txtb.Text) < 0 ||
                Convert.ToDouble(txth.Text) == 0 || Convert.ToDouble(txth.Text) < 0 ||
                Convert.ToDouble(txtd1.Text) == 0 || Convert.ToDouble(txtd1.Text) < 0
                )
                {
                    var dialoga = new MessageDialog("Nisu svi paramtri popunjenji ili su manji od 0");
                    await dialoga.ShowAsync();
                    return;
                }

                //Izvijanja izvijanja = (Izvijanja)Enum.Parse(typeof(Izvijanja), cmbIzvijanje.SelectedValue.ToString());

                //await Task.Run(() =>
                // {
                var g = new ElementGeometySlenderness()
                {
                    b = Convert.ToDouble(txtb.Text),
                    d1 = Convert.ToDouble(txtd1.Text),
                    h = Convert.ToDouble(txth.Text),
                    k = this.k,
                    L = Convert.ToDouble(txtL.Text),
                };
                var f = new ForcesSlenderness(g.li, g.h)
                {
                    M_bottom = Convert.ToDouble(txtMb.Text),
                    M_top = Convert.ToDouble(txtMt.Text),
                    NEd = Convert.ToDouble(txtN.Text),
                };
                var m = new Material()
                {
                    armatura = cmbArmatura.SelectedItem as ReinforcementTypeModelEC,
                    beton = cmbBeton.SelectedItem as BetonModelEC,
                };
                     using (var se = new VitkostEC2_V2(g,f,m))
                     {
                         se.Calculate();
                         se.KontrolaCentPritPreseka();
                         se.ProracunArmature();
                         lblAs.Text = "Potrebna Armatura: " + Math.Round(se.As, 2) + " [cm2]";
                         lblMsd_II.Text = "MEd: " + Math.Round(se.MEd, 2) + " [kNm]";
                         lblIsAcOK.Text = "Da li je dovoljan presek stuba: " + se.IsAcOK;
                         ResulteText.Text = se.ToString();
                     }
                 //});
               
            }
            catch (Exception)
            {

                var dialoga = new MessageDialog("Pogresno uneti parametri");
                await dialoga.ShowAsync();
                return;
            }

        }
        private void Imagechange()
        {
            imgVitkost.Source = new BitmapImage(new Uri(this.BaseUri, "../Assets/Vitkost/" + cmbIzvijanje.SelectedValue + ".jpg"));
        }

    }
}