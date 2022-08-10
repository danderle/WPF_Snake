using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace WPF_Snake.Converters
{
    /// <summary>
    /// Base value converter to reduce rewriting a bunch of base code
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseValueConverter<T> : MarkupExtension, IValueConverter
        where T : class, new()
    {
        private static T _converter = null;

        /// <summary>
        /// Makes the converter available to use in xaml
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new T());
        }

        /// <summary>
        /// The convert method to override
        /// </summary>
        /// <param name="value">the value to convert</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
    }
}
