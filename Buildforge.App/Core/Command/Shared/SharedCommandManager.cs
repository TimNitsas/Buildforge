namespace Buildforge.App.Core.Command.Shared;

public class SharedCommandManager
{
    public static readonly SharedCommandManager Instance = new();

    private readonly List<string> Keys = [];

    public void Queue(SharedCommand sharedCommand)
    {
        Assert.EnsureUiThread();

        if (Keys.Contains(sharedCommand.Key))
        {
            return;
        }

        Keys.Add(sharedCommand.Key);

        CommandManager.InvalidateRequerySuggested();

        Task.Run(async () =>
        {
            try
            {
                await sharedCommand.ExecuteImpl();
            }
            finally
            {
                Dispatcher.CurrentDispatcher.Invoke(() =>
                {
                    Keys.Remove(sharedCommand.Key);

                    CommandManager.InvalidateRequerySuggested();
                });
            }
        });
    }

    public bool CanExecute(SharedCommand sharedCommand)
    {
        return !Keys.Contains(sharedCommand.Key);
    }
}