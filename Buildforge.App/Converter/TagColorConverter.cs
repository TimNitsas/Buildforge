using System.Windows.Media;

namespace Buildforge.App.Converter;

public sealed class TagColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string tag)
        {
            return new SolidColorBrush(Colors.DimGray);
        }

        var color = tag.ToLower() switch
        {
            "qa approved" => Color.FromRgb(84, 168, 0),
            "approved" => Color.FromRgb(84, 168, 0),
            "buildfix" => Color.FromRgb(87, 168, 0),
            "hotfix" => Color.FromRgb(168, 157, 0),
            "custombuild" => Color.FromRgb(0, 84, 168),
            "rejected" => Color.FromRgb(192, 80, 80),
            "lab" => Color.FromRgb(80, 120, 192),
            _ => Colors.DimGray
        };

        return new SolidColorBrush(color);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}