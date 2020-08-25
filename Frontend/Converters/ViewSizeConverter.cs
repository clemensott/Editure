using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;
using Editure.Backend;

namespace Editure.Frontend.Converters
{
    class ViewSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IntSize size = (IntSize)value;

            return size.Width + " x " + size.Height;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
