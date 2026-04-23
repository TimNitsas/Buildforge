namespace Buildforge.App.Core.Command.Shared;

public abstract class SharedCommand : ICommand
{
    public string Key { get; }

    public CancellationTokenSource Cts { get; }

    protected SharedCommand(string key)
    {
        Key = key;

        Cts = new CancellationTokenSource();
    }

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object? parameter)
    {
        if (!SharedCommandManager.Instance.CanExecute(this))
        {
            return false;
        }

        return CanExecuteImpl();
    }

    public void Execute(object? parameter)
    {
        SharedCommandManager.Instance.Queue(this);
    }

    protected abstract bool CanExecuteImpl();

    public abstract Task ExecuteImpl();
}
