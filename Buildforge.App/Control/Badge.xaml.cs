using System.Windows.Media;

namespace Buildforge.App.Control;

public partial class Badge : UserControl
{
    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(Badge), new PropertyMetadata(string.Empty));

    public SolidColorBrush Color
    {
        get { return (SolidColorBrush)GetValue(ColorProperty); }
        set { SetValue(ColorProperty, value); }
    }

    public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(SolidColorBrush), typeof(Badge), new PropertyMetadata(new SolidColorBrush()));

    public Badge()
    {
        InitializeComponent();
    }
}