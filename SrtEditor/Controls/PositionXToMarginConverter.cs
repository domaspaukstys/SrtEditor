using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SrtEditor.Controls
{
    public class PositionXToMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Thickness((double) value, 0, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object result = 0;
            var val = value as Thickness?;
            if (val != null)
            {
                result = val.Value.Left;
            }
            return result;
        }
    }
}