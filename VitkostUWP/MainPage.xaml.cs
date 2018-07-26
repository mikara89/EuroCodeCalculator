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
using UINGN_VitkostEC2;
using Windows.UI.Xaml.Media.Imaging;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Popups;
using TabeleEC2.Model;

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
            cmbIzvijanje.SelectedValue = "Pokretan_I_Ukljeste";
            cmbBeton.ItemsSource = TabeleEC2.BetonClasses.GetBetonClassListEC();
            cmbBeton.SelectedIndex = 3;           
            cmbArmatura.ItemsSource = TabeleEC2.ReinforcementType.GetArmatura();
            cmbArmatura.SelectedIndex = 0;
            txtNg.Text = "" + 43.5;
            txtNq.Text = "" + 18.0;
            txtL.Text = "" + 400;
            txtb.Text = "" + 25;
            txth.Text = "" + 25;
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
                if (txtNg.Text == "" ||
                txtNq.Text == "" || txtL.Text == "" || txtb.Text == "" ||
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
                using (var se = new VitkostEC2(
                    izvijanja,
                    Convert.ToDouble(txtNg.Text),
                    Convert.ToDouble(txtNq.Text),
                    Convert.ToDouble(txtL.Text),
                    Convert.ToDouble(txtb.Text),
                    Convert.ToDouble(txth.Text),
                    (cmbBeton.SelectedItem as BetonModelEC),
                    (cmbArmatura.SelectedItem as ReinforcementTypeModelEC),
                    Convert.ToDouble(txtd1.Text)
                    ))
                {
                    lblAs.Text = "Potrebna Armatura: " + Math.Round(se.usv_As, 2) + " [cm2]";
                    lblMsd_II.Text = se.Msd_I == 0 ? "Msd_II: " + Math.Round(se.Msd_II, 2) + " [kNm]" : "Msd_I: " + Math.Round(se.Msd_I, 2) + " [kNm]";
                    lblIsAcOK.Text = "Da li je dovoljan presek stuba: " + se.IsAcOK;
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
