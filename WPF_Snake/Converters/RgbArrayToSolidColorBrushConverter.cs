using System;
using System.Globalization;
using System.Windows.Media;

namespace WPF_Snake.Converters
{
    public class RgbArrayToSolidColorBrushConverter : BaseValueConverter<RgbArrayToSolidColorBrushConverter>
    {

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var rgb = (int[])value;

            var color = Color.FromRgb(System.Convert.ToByte(rgb[0]), System.Convert.ToByte(rgb[1]), System.Convert.ToByte(rgb[2]));
            return new SolidColorBrush(color);
        }


        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
