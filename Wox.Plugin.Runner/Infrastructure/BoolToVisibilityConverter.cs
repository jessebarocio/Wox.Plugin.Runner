using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Wox.Plugin.Runner.Infrastructure
{
    class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            if ( ( (bool)value ) )
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }
}
