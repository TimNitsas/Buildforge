using System.Text.Json.Serialization;

namespace Buildforge.Service.Repository.Contribution;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "Version")]
[JsonDerivedType(typeof(V1.ContributionRecord), "v1")]
public abstract class ContributionRecord
{
}