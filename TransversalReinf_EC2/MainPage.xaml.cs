using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TabeleEC2;
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
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TransversalReinf_EC2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            ViewModel = new TransversalReinf_EC2ViewModel(65, 40, 25, 40, 6, ReinforcementType.GetArmatura().Where(n=>n.name=="B500B").First(), BetonClasses.GetBetonClassListEC().Find(n => n.name == "C30/37"), new ReinforcementModelEC(ReinforcementType.GetAramturaList().Single(n => n.diameter == 16), 6));
            this.InitializeComponent();
            
        }
        public TransversalReinf_EC2ViewModel ViewModel {
            get { return DataContext as TransversalReinf_EC2ViewModel; }
            set { DataContext = value; }
        }

        private void cmbLongArmDiametar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var selectedLongArmDiametar = cmbLongArmDiametar.SelectedItem as ReinforcementTabelModel;
            var selectedLongArmNum = (int)cmbLongArmNum.SelectedItem;
            if (selectedLongArmDiametar != ViewModel.Longitud_As1.armatura || selectedLongArmNum != ViewModel.Longitud_As1.Number)
            {
                if (selectedLongArmDiametar != null)
                    ViewModel.Longitud_As1 = new ReinforcementModelEC(selectedLongArmDiametar, selectedLongArmNum);
            }


            if(cmbAddArmNum.SelectedItem==null)return;

            var selectedAddArmDiametar = cmbAddArmDiametar.SelectedItem as ReinforcementTabelModel;
            var selectedAddArmNum = (int)cmbAddArmNum.SelectedItem;
            if (selectedAddArmDiametar != ViewModel.A_add_usv.armatura || selectedAddArmNum != ViewModel.A_add_usv.Number)
            {
                if (selectedAddArmDiametar != null)
                    ViewModel.A_add_usv = new ReinforcementModelEC(selectedAddArmDiametar, selectedAddArmNum);
            }

        }

        private void cmbTransArmDiametar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel.Asw_min == 0)
            {
                var selectedTransArmDiametar = cmbTransArmDiametar.SelectedItem as ReinforcementTabelModel;
                if (selectedTransArmDiametar != ViewModel.Transversal_Asw.armatura)
                {
                    if (selectedTransArmDiametar != null)
                        ViewModel.Transversal_Asw = new ReinforcementModelEC(selectedTransArmDiametar, 1);
                }
            }
              
            //if (ViewModel.Asw_min != 0)
            //{
            //    cmbTransArmDiametar.SelectedIndex = 0;
            //    var selectedTransArmDiametar = cmbTransArmDiametar.SelectedItem as ReinforcementTabelModel;
            //    if (selectedTransArmDiametar != ViewModel.Transversal_Asw_min.armatura)
            //    {
            //        if (selectedTransArmDiametar != null)
            //            ViewModel.Transversal_Asw_min = new ReinforcementModel(selectedTransArmDiametar, 1);
            //    }
            //}

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            var Item = (sender as ListView).SelectedItem as string;
            if (Item == null) return;
            var dialoga = new MessageDialog("Greška: " + Item);
            dialoga.ShowAsync();
            return;
        }
    }
}
