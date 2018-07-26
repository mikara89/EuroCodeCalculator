using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace TransversalReinf_EC2.ViewModel.Convert
{
    public class ObsevCollOfStringsShortConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            
            var param = 20;
            if (value == null)
                return null;
            if (parameter != null)
                Int32.TryParse(parameter as string,out param);
            if (value.GetType() == typeof(ObservableCollection<string>))
            {
                ObservableCollection<string> text = new ObservableCollection<string>();
                foreach (var item in (ObservableCollection<string>)value)
                {
                    var i = item.Substring(0, param);
                    text.Add(i + "...");
                }
                return text;
            }
            if (value.GetType() == typeof(string))
            {
                var i = (value as string).Substring(0, param);
                return i;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }
}
