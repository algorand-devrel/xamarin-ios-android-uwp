using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace algorandapp
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string)
            {
                try
                {
                    return ImageSource.FromResource("algorandapp.Assets." + value.ToString(), typeof(App));
                }
                catch (Exception ex)
                {
                    Debug.Write("error on image converter ... " + ex.Message);
                    return null;
                }
            }
            return value;
        }



        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            throw new NotImplementedException();

        }
    }
}
