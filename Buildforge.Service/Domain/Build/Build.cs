using System.Text.Json.Serialization;

namespace Buildforge.Service.Domain.Build;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "Version")]
[JsonDerivedType(typeof(V1.Build), "v1")]
public abstract class Build
{
    public required string Id { get; set; }

    public required DateTime ReadAt { get; set; }
}