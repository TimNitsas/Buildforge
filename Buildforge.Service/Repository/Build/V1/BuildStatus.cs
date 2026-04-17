using System.Text.Json.Serialization;

namespace Buildforge.Service.Repository.Build.V1;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "Type")]
[JsonDerivedType(typeof(BuildStatusActive), "active")]
[JsonDerivedType(typeof(BuildStatusFailed), "failed")]
[JsonDerivedType(typeof(BuildStatusQueued), "queued")]
[JsonDerivedType(typeof(BuildStatusSuccess), "success")]
public abstract class BuildStatus
{
    public DateTime StartTime { get; set; }
}