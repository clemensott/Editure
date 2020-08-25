using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;
using Editure.Backend;

namespace Editure.Frontend.Converters
{
    class SizeWidthConverter : IValueConverter
    {
        private IntSize intSize;
        private string text;

        public SizeWidthConverter()
        {
            intSize = new IntSize();
            text = intSize.Width.ToString();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            intSize.Height = ((IntSize)value).Height;

            if (intSize == (IntSize)value) return text;

            intSize = (IntSize)value;

            return text = intSize.Width.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int width;
            text = value.ToString();

            if (int.TryParse(text, out width) && width >= 0) intSize.Width = width;

            return intSize;
        }
    }

    class SizeHeightConverter : IValueConverter
    {
        private IntSize IntSize;
        private string text;

        public SizeHeightConverter()
        {
            IntSize = new IntSize();
            text = IntSize.Height.ToString();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IntSize.Width = ((IntSize)value).Width;

            if (IntSize == (IntSize)value) return text;

            IntSize = (IntSize)value;

            return text = IntSize.Height.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int height;
            text = value.ToString();

            if (int.TryParse(text, out height) && height >= 0) IntSize.Height = height;

            return IntSize;
        }
    }
}
