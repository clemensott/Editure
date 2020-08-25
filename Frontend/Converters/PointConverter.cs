using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;
using Editure.Backend;

namespace Editure.Frontend.Converters
{
    class PointXConverter : IValueConverter
    {
        private IntPoint intPoint;
        private string text;

        public int Test { get; set; }
        public PointXConverter()
        {
            intPoint = new IntPoint();
            text = intPoint.X.ToString();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            intPoint.Y = ((IntPoint)value).Y;

            if (intPoint == (IntPoint)value) return text;

            intPoint = (IntPoint)value;

            return text = intPoint.X.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int x;
            text = value.ToString();

            if (int.TryParse(text, out x)) intPoint.X = x;

            return intPoint;
        }
    }

    class PointYConverter : IValueConverter
    {
        private IntPoint IntPoint;
        private string text;

        public PointYConverter()
        {
            IntPoint = new IntPoint();
            text = IntPoint.Y.ToString();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IntPoint.X = ((IntPoint)value).X;

            if (IntPoint == (IntPoint)value) return text;

            IntPoint = (IntPoint)value;

            return text = IntPoint.Y.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int y;
            text = value.ToString();

            if (int.TryParse(text, out y)) IntPoint.Y = y;

            return IntPoint;
        }
    }
}
