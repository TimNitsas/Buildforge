namespace Buildforge.App.Control;

public partial class BuildforgeProgressBar : UserControl
{
    public string ProgressText
    {
        get { return (string)GetValue(ProgressTextProperty); }
        set { SetValue(ProgressTextProperty, value); }
    }

    public static readonly DependencyProperty ProgressTextProperty = DependencyProperty.Register(nameof(ProgressText), typeof(string), typeof(BuildforgeProgressBar), new PropertyMetadata(string.Empty));

    public double ProgressValue
    {
        get { return (double)GetValue(ProgressValueProperty); }
        set { SetValue(ProgressValueProperty, value); }
    }

    public static readonly DependencyProperty ProgressValueProperty = DependencyProperty.Register(nameof(ProgressValue), typeof(double), typeof(BuildforgeProgressBar), new PropertyMetadata(0d));

    public BuildforgeProgressBar()
    {
        InitializeComponent();
    }
}