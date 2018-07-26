using System;
using Windows.UI.Xaml.Data;

namespace TransversalReinf_EC2.ViewModel.Convert
{
    public class BoolConvert : IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value.GetType() == typeof(bool))
                return !(bool)value;
            if (value.GetType() == typeof(double))
            {
                return (double)value > 0 ? true : false;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
