using System.Text.Json.Serialization;

namespace Buildforge.Service.Controller.Build.V1.Model;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "discriminator")]
[JsonDerivedType(typeof(BuildStatusFailed))]
[JsonDerivedType(typeof(BuildStateSuccess))]
public abstract class BuildStatus
{
}
