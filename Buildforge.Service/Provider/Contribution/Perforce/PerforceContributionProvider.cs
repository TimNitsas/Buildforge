using Perforce.P4;

namespace Buildforge.Service.Provider.Contribution.Perforce;

public sealed class PerforceContributionProvider : IContributionProvider
{
    private readonly int BatchSize = 10;

    public async IAsyncEnumerable<Contribution> GetContributions(object? startAtKey, [EnumeratorCancellation] CancellationToken ct)
    {
        await Task.Yield();

        int? index = ParseKey(startAtKey);

        Server pServer = new Server(new ServerAddress("localhost:1666"));

        global::Perforce.P4.Repository rep = new global::Perforce.P4.Repository(pServer);

        using Connection con = rep.Connection;

        con.UserName = "root";

        var result = con.Connect(null);

        ChangesCmdOptions opts = new
        (
            flags: ChangesCmdFlags.None,
            clientName: null,
            maxItems: BatchSize,
            status: ChangeListStatus.Submitted,
            userName: null
        )
        {
            ["-l"] = string.Empty,
            ["-t"] = string.Empty,
            ["-r"] = string.Empty
        };

        if (index.HasValue)
        {
            opts["-e"] = index.Value.ToString(); // Inclusive
        }

        IList<Changelist> changes = rep.GetChangelists(opts);

        if (changes is null)
        {
            yield break;
        }

        if (changes.Count <= 0)
        {
            yield break;
        }

        foreach (var change in changes)
        {
            if (ct.IsCancellationRequested)
            {
                yield break;
            }

            DescribeCmdOptions describe = new DescribeCmdOptions(
                DescribeChangelistCmdFlags.None,
                0, 0
            );

            Changelist desc = rep.GetChangelist(change.Id, describe);

            foreach (var file in desc.Files)
            {
                Console.WriteLine(file.DepotPath);
            }

            List<FileSpec> specs = [];

            foreach (var file in desc.Files)
            {
                var spec = new FileSpec
                (
                    new DepotPath(file.DepotPath.Path),
                    null,
                    null,
                    new Revision(file.HaveRev.ToString())
                );

                specs.Add(spec);
            }

            var fileMetaDataOption = new GetFileMetaDataCmdOptions
            (
                flags: GetFileMetadataCmdFlags.FileSize,
                filter: null,
                taggedFields: null,
                maxItems: -1,
                afterChangelist: null,
                byChangelist: null,
                attribPattern: null
            );

            var metadata = rep.GetFileMetaData(specs, fileMetaDataOption);

            var files = metadata.Select(m => new ContributionFile()
            {
                DepotPath = m.DepotPath.Path,
                Size = m.FileSize,
                Hash = m.Digest,
                ContributionAction = m.Action switch
                {
                    FileAction.Delete | FileAction.MoveDelete => ContributionAction.Deletion,
                    FileAction.Add | FileAction.Added => ContributionAction.Addition,
                    FileAction.Edit => ContributionAction.Edit,
                    _ => null
                }
            });

            yield return new Contribution()
            {
                Description = change.Description,
                Id = change.Id.ToString(),
                User = change.OwnerName,
                CommitDate = change.ModifiedDate,
                Files = [.. files]
            };
        }

        if (changes.Count < BatchSize)
        {
            yield break;
        }

        index = changes[^-1].Id + 1;

        await foreach (var item in GetContributions(index, ct))
        {
            yield return item;
        }
    }

    private static int? ParseKey(object? startAtKey)
    {
        if (startAtKey is int key)
        {
            return key;
        }

        if (startAtKey is string keyString)
        {
            if (int.TryParse(keyString, out var value))
            {
                return value;
            }
        }

        return null;
    }
}