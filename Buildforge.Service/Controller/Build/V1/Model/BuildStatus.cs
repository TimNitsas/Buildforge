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

    public static BuildStatus FromDomain(Domain.Build.V1.Build item)
    {
        return item.Status switch
        {
            Domain.Build.V1.BuildStatusActive a => new V1.Model.BuildStatusActive(),
            Domain.Build.V1.BuildStatusFailed f => new V1.Model.BuildStatusFailed() { Reason = f.Reason },
            Domain.Build.V1.BuildStatusQueued q => new V1.Model.BuildStatusQueued(),
            Domain.Build.V1.BuildStatusSuccess s => new V1.Model.BuildStatusSuccess() { BuildTime = s.BuildTime, Bytes = s.Bytes },
            _ => throw new NotImplementedException()
        };
    }
}