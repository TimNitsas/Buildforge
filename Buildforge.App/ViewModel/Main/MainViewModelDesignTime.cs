using Bogus;
using Buildforge.App.Domain;

namespace Buildforge.App.ViewModel.Main;

public sealed class MainViewModelDesignTime : MainViewModel
{
    public MainViewModelDesignTime() : base(new TokenHandler())
    {
        Username = new Faker().Name.FullName();
    }
}