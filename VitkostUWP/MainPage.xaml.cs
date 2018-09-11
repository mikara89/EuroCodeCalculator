using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Popups;
using TabeleEC2.Model;
using CalculatorEC2Logic;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace VitkostUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
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

        public MainPage()
        {
            this.InitializeComponent();
            var list = Enum.GetNames(typeof(Izvijanja));
            cmbIzvijanje.ItemsSource = list;
            cmbIzvijanje.SelectedValue = Enum.GetName(typeof(Izvijanja),Izvijanja.Ukljesten_Sa_Jedne);
            cmbBeton.ItemsSource = TabeleEC2.BetonClasses.GetBetonClassListEC();
            cmbBeton.SelectedIndex = 3;           
            cmbArmatura.ItemsSource = TabeleEC2.ReinforcementType.GetArmatura();
            cmbArmatura.SelectedIndex = 0;
            txtN.Text = "" + 569.7;
            txtMt.Text = "" + 2037.79;
            txtMb.Text = "" + (0);
            txtL.Text = "" + 850;
            txtb.Text = "" + 60;
            txth.Text = "" + 60;
            txtd1.Text = "" + 4;

            btnIzracunaj.Click +=
                async(s, e) =>
            {
                await IzracunajAsync();
            };
            IzracunajAsync();
            Imagechange();


            cmbIzvijanje.SelectionChanged += (s,e) => { Imagechange(); };
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

                Izvijanja izvijanja = (Izvijanja)Enum.Parse(typeof(Izvijanja), cmbIzvijanje.SelectedValue.ToString());
                //using (var se = new VitkostEC2(
                //    izvijanja,
                //    Convert.ToDouble(txtNg.Text),
                //    Convert.ToDouble(txtNq.Text),
                //    Convert.ToDouble(txtL.Text),
                //    Convert.ToDouble(txtb.Text),
                //    Convert.ToDouble(txth.Text),
                //    (cmbBeton.SelectedItem as BetonModelEC),
                //    (cmbArmatura.SelectedItem as ReinforcementTypeModelEC),
                //    Convert.ToDouble(txtd1.Text)
                //    ))
                using (var se = new VitkostEC2_V2(
                    new ElementGeometySlenderness()
                    {
                        b = Convert.ToDouble(txtb.Text) ,
                        d1 = Convert.ToDouble(txtd1.Text) ,
                        h = Convert.ToDouble(txth.Text),
                        izvijanje= izvijanja,
                        L= Convert.ToDouble(txtL.Text),
                    },
                    new ForcesSlenderness()
                    {
                         M_bottom= Convert.ToDouble(txtMb.Text),
                         M_top= Convert.ToDouble(txtMt.Text),
                         N= Convert.ToDouble(txtN.Text),
                    },
                    new Material()
                    {
                         armatura= cmbArmatura.SelectedItem as ReinforcementTypeModelEC,
                         beton= cmbBeton.SelectedItem as BetonModelEC,
                    }
                    ))
                {
                    se.Calculate();
                    se.KontrolaCentPritPreseka();
                    se.ProracunArmature();
                    lblAs.Text = "Potrebna Armatura: " + Math.Round(se.As, 2) + " [cm2]";
                    lblMsd_II.Text = "MEd: " + Math.Round(se.MEd, 2) + " [kNm]";
                    lblIsAcOK.Text = "Da li je dovoljan presek stuba: " + se.IsAcOK;
                    ResulteText.Text = se.ToString();
                }
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
            imgVitkost.Source = new BitmapImage(new Uri(this.BaseUri, "Assets/Vitkost/" + cmbIzvijanje.SelectedValue + ".jpg"));
        }
      
    }
}
