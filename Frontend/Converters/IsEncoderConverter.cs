using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Editure.Backend.Editing.EditEncoders;

namespace Editure.Frontend.Converters
{
    class IsEncoderConverterParent
    {
        public EditEncoderType CurrentValue { get; set; }
    }

    class IsEncoderConverter : DependencyObject, IValueConverter
    {
        private static EditEncoderType currentValue;

        public static readonly DependencyProperty ParentProperty = DependencyProperty.Register("Parent", 
            typeof(IsEncoderConverterParent), typeof(IsEncoderConverter),
            new PropertyMetadata(null));

        public static readonly DependencyProperty DefaultValueProperty = DependencyProperty.Register("DefaultValue", 
            typeof(EditEncoderType), typeof(IsEncoderConverter),
                new PropertyMetadata(EditEncoderType.Auto));

        public IsEncoderConverterParent Parent
        {
            get => (IsEncoderConverterParent)GetValue(ParentProperty);
            set => SetValue(ParentProperty, value);
        }

        public EditEncoderType DefaultValue
        {
            get => (EditEncoderType)GetValue(DefaultValueProperty);
            set => SetValue(DefaultValueProperty, value);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Parent != null) Parent.CurrentValue = (EditEncoderType)value;
            else currentValue = (EditEncoderType)value;

            return (EditEncoderType)value == DefaultValue;
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