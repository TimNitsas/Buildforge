using Material.Icons;
using System.Windows.Input;

namespace Buildforge.App.Control;

public partial class NavigationIcon : UserControl
{
    public MaterialIconKind IconKind
    {
        get => (MaterialIconKind)GetValue(IconKindProperty);
        set => SetValue(IconKindProperty, value);
    }

    public static readonly DependencyProperty IconKindProperty = DependencyProperty.Register(nameof(IconKind), typeof(MaterialIconKind), typeof(NavigationIcon), new PropertyMetadata(MaterialIconKind.Null));

    public string Tooltip
    {
        get => (string)GetValue(TooltipProperty);
        set => SetValue(TooltipProperty, value);
    }

    public static readonly DependencyProperty TooltipProperty = DependencyProperty.Register(nameof(Tooltip), typeof(string), typeof(NavigationIcon), new PropertyMetadata(string.Empty));

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(NavigationIcon), new PropertyMetadata(null));

    public NavigationIcon()
    {
        InitializeComponent();
    }
}