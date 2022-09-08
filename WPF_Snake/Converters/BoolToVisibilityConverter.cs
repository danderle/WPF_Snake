using System;
using System.Globalization;
using System.Windows;

namespace WPF_Snake.Converters
{
    /// <summary>
    /// Converts a boolean to a <see cref="Visibility"/>
    /// </summary>
    public class BoolToVisibilityConverter : BaseValueConverter<BoolToVisibilityConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visible = (bool)value;

            if (!visible)
            {
                return Visibility.Collapsed;
            }

            return Visibility.Visible;
        }


        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
