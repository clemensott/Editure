using MainProgram;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Bilder_suchen.Converters
{
    class IsReferenceConverterParent
    {
        public EditReferencePositionType CurrentValue { get; set; }
    }

    class IsReferenceConverter : DependencyObject, IValueConverter
    {
        private static EditReferencePositionType currentValue;

        public static readonly DependencyProperty ParentProperty = DependencyProperty.Register("Parent",
            typeof(IsReferenceConverterParent), typeof(IsReferenceConverter), new PropertyMetadata(null));

        public static readonly DependencyProperty DefaultValueProperty = DependencyProperty.Register("DefaultValue",
            typeof(EditReferencePositionType), typeof(IsReferenceConverter),
                new PropertyMetadata(EditReferencePositionType.TopLeft));

        public IsReferenceConverterParent Parent
        {
            get { return (IsReferenceConverterParent)GetValue(ParentProperty); }
            set { SetValue(ParentProperty, value); }
        }

        public EditReferencePositionType DefaultValue
        {
            get { return (EditReferencePositionType)GetValue(DefaultValueProperty); }
            set { SetValue(DefaultValueProperty, value); }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Parent != null) Parent.CurrentValue = (EditReferencePositionType)value;
            else currentValue = (EditReferencePositionType)value;

            return (EditReferencePositionType)value == DefaultValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Parent != null)
            {
                if ((bool)value) return Parent.CurrentValue= DefaultValue;

                return Parent.CurrentValue;
            }

            if ((bool)value) return currentValue = DefaultValue;

            return currentValue;
        }
    }
}