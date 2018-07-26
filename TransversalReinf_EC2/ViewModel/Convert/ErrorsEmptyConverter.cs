using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Data;

namespace TransversalReinf_EC2.ViewModel.Convert
{
    public class ErrorsEmptyConverter : IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var list =value as ObservableCollection<string>;
            if (value == null)
                return false;
            if (list.Count() != 0)
                return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
