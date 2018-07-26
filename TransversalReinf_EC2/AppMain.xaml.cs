using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TransversalReinf_EC2.View;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace TransversalReinf_EC2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppMain : Page
    {
        public AppMain()
        {
            this.InitializeComponent();
            frameT.Navigate(typeof(MainPage));
            HamburgerMenu.ItemClick += (s, e) => { OnItemSelect(s, e); };
            HamburgerMenu.OptionsItemClick += (s, e) => { OnItemSelect(s, e); };
        }

        private void HamburgerMenu_OptionsItemClick(object sender, ItemClickEventArgs e)
        {
            var MenuItemSelected = sender as HamburgerMenuItem;
            if (MenuItemSelected.Label == "Transverzalne sile")
                frameT.Navigate(typeof(MainPage));
            if (MenuItemSelected.Label == "About")
                frameT.Navigate(typeof(AboutView));
        }
        private void OnItemSelect(object sender, ItemClickEventArgs e) 
        {
            var MenuItemSelected = sender as HamburgerMenuItem;
            if (MenuItemSelected.Label == "Transverzalne sile")
                frameT.Navigate(typeof(MainPage));
            if (MenuItemSelected.Label == "About")
                frameT.Navigate(typeof(AboutView));
        }
    }
}
