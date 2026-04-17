using System.Text.Json.Serialization;

namespace Buildforge.Service.Repository.Contribution;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "Version")]
[JsonDerivedType(typeof(V1.Contribution), "v1")]
public abstract class Contribution
{
    public required string Id { get; init; }

    public required DateTime ReadAt { get; set; }
}
