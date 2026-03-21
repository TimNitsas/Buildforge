using DotMake.CommandLine;
using System.Windows.Threading;

namespace Buildforge.App.Cli;

[CliCommand(Name = "sayhello", Description = "Says hello", Parent = typeof(RootCli))]
public class SayHelloCli
{
    public void Run()
    {
        Dispatcher.CurrentDispatcher.Invoke(() =>
        {
            MessageBox.Show("Hello!");
        });
    }
}