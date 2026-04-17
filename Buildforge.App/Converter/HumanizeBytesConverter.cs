using Humanizer;

namespace Buildforge.App.Converter;

public sealed class HumanizeBytesConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is long dt)
        {
            return dt.Bytes().Humanize();
        }
        else if (value is int @int)
        {
            return @int.Bytes().Humanize();
        }

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}