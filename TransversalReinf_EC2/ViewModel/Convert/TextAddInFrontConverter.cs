using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Data;

namespace TransversalReinf_EC2.ViewModel.Convert
{
    public class TextAddInFrontConverter : IValueConverter  
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string text="";
            if (value == null) 
                return null;
            if (value.GetType() == typeof(double))            
                text=(Math.Round(((double)value), 2)).ToString();
            if (value.GetType() == typeof(string))
                text = value as string;

            if (parameter == null)
                return text;
            var pretext = parameter as string;
            
            return pretext+" "+text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }

  
}
