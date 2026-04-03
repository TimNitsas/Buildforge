namespace Buildforge.App.Control;

public partial class Person : UserControl
{
    public string PersonName
    {
        get { return (string)GetValue(PersonNameProperty); }
        set { SetValue(PersonNameProperty, value); }
    }

    public static readonly DependencyProperty PersonNameProperty = DependencyProperty.Register(nameof(PersonName), typeof(string), typeof(Person), new PropertyMetadata(string.Empty));

    public Person()
    {
        InitializeComponent();
    }
}