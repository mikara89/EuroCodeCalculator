using System;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace TransversalReinf_EC2.ViewModel.Convert
{
    public class ComboBoxItemReinforcementModelConvert : IValueConverter  
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value as TabeleEC2.Model.ReinforcementModelEC;
        }
    }
}
