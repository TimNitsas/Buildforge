namespace Buildforge.Service.Repository.Core;

public class Serialization
{
    public static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        AllowOutOfOrderMetadataProperties = true
    };
}