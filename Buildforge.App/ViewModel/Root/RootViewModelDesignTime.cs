using Buildforge.App.Domain.Token;

namespace Buildforge.App.ViewModel.Root;

public partial class RootViewModelDesignTime : RootViewModel
{
    public RootViewModelDesignTime() : base(new TokenHandler())
    {
    }
}
