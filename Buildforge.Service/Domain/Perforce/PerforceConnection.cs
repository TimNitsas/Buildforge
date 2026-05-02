using Perforce.P4;

namespace Buildforge.Service.Domain.Perforce;

public class PerforceConnection : IDisposable
{
    public global::Perforce.P4.Connection Connection { get; }

    public PerforceConnection(IOptions<PerforceServerOptions> options)
    {
        Server pServer = new global::Perforce.P4.Server(new ServerAddress(options.Value.Host));

        global::Perforce.P4.Repository rep = new(pServer);

        Connection = rep.Connection;

        Connection.UserName = options.Value.Username;

        Connection.Connect(null);
    }

    public void Dispose()
    {
        Connection.Dispose();
    }
}