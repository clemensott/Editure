using MainProgram;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Bilder_suchen.Converters
{
    class IsEncoderConverterParent
    {
        public EditEncoderType CurrentValue { get; set; }
    }

    class IsEncoderConverter : DependencyObject, IValueConverter
    {
        private static EditEncoderType currentValue;

        public static readonly DependencyProperty ParentProperty =
            DependencyProperty.Register("Parent", typeof(IsEncoderConverterParent), typeof(IsEncoderConverter),
                new PropertyMetadata(null, new PropertyChangedCallback(OnParentPropertyChanged)));

        private static void OnParentPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var s = (IsEncoderConverter)sender;
            var value = (IsEncoderConverterParent)e.NewValue;
        }

        public static readonly DependencyProperty DefaultValueProperty =
            DependencyProperty.Register("DefaultValue", typeof(EditEncoderType), typeof(IsEncoderConverter),
                new PropertyMetadata(EditEncoderType.Auto, new PropertyChangedCallback(OnDefaultValuePropertyChanged)));

        private static void OnDefaultValuePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var s = (IsEncoderConverter)sender;
            var value = (EditEncoderType)e.NewValue;
        }

        public IsEncoderConverterParent Parent
        {
            get { return (IsEncoderConverterParent)GetValue(ParentProperty); }
            set { SetValue(ParentProperty, value); }
        }

        public EditEncoderType DefaultValue
        {
            get { return (EditEncoderType)GetValue(DefaultValueProperty); }
            set { SetValue(DefaultValueProperty, value); }
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