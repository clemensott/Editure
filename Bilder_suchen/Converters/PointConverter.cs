using MainProgram;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;

namespace Bilder_suchen.Converters
{
    class PointXConverter : IValueConverter
    {
        private IntPoint IntPoint;
        private string text;

        public int Test { get; set; }
        public PointXConverter()
        {
            IntPoint = new IntPoint();
            text = IntPoint.X.ToString();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IntPoint.Y = ((IntPoint)value).Y;

            if (IntPoint == (IntPoint)value) return text;

            IntPoint = (IntPoint)value;

            return text = IntPoint.X.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int x;
            text = value.ToString();

            if (int.TryParse(text, out x)) IntPoint.X = x;

            return IntPoint;
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
