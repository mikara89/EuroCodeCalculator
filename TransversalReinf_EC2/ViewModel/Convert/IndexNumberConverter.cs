using System;
using System.Collections.ObjectModel;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace TransversalReinf_EC2.ViewModel.Convert
{
    public class IndexNumberConverter : IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return null;
            if (parameter == null)
                return null;
            var param= value as string;
            if (parameter.GetType() == typeof(ObservableCollection<string>))
            {
                var v = parameter as ObservableCollection<string>;
                var i = v.IndexOf(param);
                return i;
            }
          
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    
    }
    public class RedGreenTextConverter : IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return null;
            var v = (double)value;
            if (v<=100)
            {
                return new SolidColorBrush(Colors.Green);
            }else return new SolidColorBrush(Colors.Red);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }

    }
}
