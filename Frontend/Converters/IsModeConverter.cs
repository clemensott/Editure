using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Editure.Backend.Editing.EditMode;

namespace Editure.Frontend.Converters
{
    class IsModeConverterParent
    {
        public EditModeType CurrentValue { get; set; }
    }

    class IsModeConverter : DependencyObject, IValueConverter
    {
        private static EditModeType currentValue;

        public static readonly DependencyProperty ParentProperty = DependencyProperty.Register("Parent",
            typeof(IsModeConverterParent), typeof(IsModeConverter), new PropertyMetadata(null));

        public static readonly DependencyProperty DefaultValueProperty = DependencyProperty.Register("DefaultValue",
            typeof(EditModeType), typeof(IsModeConverter), new PropertyMetadata(EditModeType.Crop));

        public IsModeConverterParent Parent
        {
            get => (IsModeConverterParent)GetValue(ParentProperty);
            set => SetValue(ParentProperty, value);
        }

        public EditModeType DefaultValue
        {
            get => (EditModeType)GetValue(DefaultValueProperty);
            set => SetValue(DefaultValueProperty, value);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Parent != null) Parent.CurrentValue = (EditModeType)value;
            else currentValue = (EditModeType)value;

            return (EditModeType)value == DefaultValue;
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