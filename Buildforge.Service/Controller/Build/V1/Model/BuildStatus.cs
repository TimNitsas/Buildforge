using System.Text.Json.Serialization;

namespace Buildforge.Service.Controller.Build.V1.Model;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "discriminator")]
[JsonDerivedType(typeof(BuildStatusFailed), typeDiscriminator: nameof(BuildStatusFailed))]
[JsonDerivedType(typeof(BuildStatusSuccess), typeDiscriminator: nameof(BuildStatusSuccess))]
[JsonDerivedType(typeof(BuildStatusQueued), typeDiscriminator: nameof(BuildStatusQueued))]
[JsonDerivedType(typeof(BuildStatusActive), typeDiscriminator: nameof(BuildStatusActive))]
public abstract class BuildStatus
{
    public DateTime StartTime { get; set; }
}