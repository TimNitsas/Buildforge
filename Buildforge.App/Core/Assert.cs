using System.Runtime.CompilerServices;

namespace Buildforge.App.Core;

public static class Assert
{
    public static void EnsureUiThread([CallerMemberName] string? caller = null)
    {
        if (!Dispatcher.CurrentDispatcher.CheckAccess())
        {
            throw new InvalidOperationException($"Ui thread required while calling '{caller}' ({Thread.CurrentThread.Name}).");
        }
    }
}