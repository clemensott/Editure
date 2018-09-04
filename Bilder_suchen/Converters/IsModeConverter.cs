using MainProgram;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Bilder_suchen.Converters
{
    class IsModeConverterParent
    {
        public EditMode CurrentValue { get; set; }
    }

    class IsModeConverter : DependencyObject, IValueConverter
    {
        private static EditMode currentValue;

        public static readonly DependencyProperty ParentProperty = DependencyProperty.Register("Parent",
            typeof(IsModeConverterParent), typeof(IsModeConverter), new PropertyMetadata(null));

        public static readonly DependencyProperty DefaultValueProperty = DependencyProperty.Register("DefaultValue",
            typeof(EditMode), typeof(IsModeConverter), new PropertyMetadata(EditMode.Crop));

        public IsModeConverterParent Parent
        {
            get { return (IsModeConverterParent)GetValue(ParentProperty); }
            set { SetValue(ParentProperty, value); }
        }

        public EditMode DefaultValue
        {
            get { return (EditMode)GetValue(DefaultValueProperty); }
            set { SetValue(DefaultValueProperty, value); }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Parent != null) Parent.CurrentValue = (EditMode)value;
            else currentValue = (EditMode)value;

            return (EditMode)value == DefaultValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Parent != null)
            {
                if ((bool)value) return Parent.CurrentValue = DefaultValue;

                return Parent.CurrentValue;
            }

            if ((bool)value) return currentValue = DefaultValue;

            return currentValue;
        }
    }
}