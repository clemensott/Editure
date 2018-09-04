using MainProgram;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;

namespace Bilder_suchen.Converters
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
