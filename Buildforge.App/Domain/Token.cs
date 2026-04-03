using System.Text.Json.Serialization;

namespace Buildforge.App.Domain;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "version")]
[JsonDerivedType(typeof(V1), "v1")]
public abstract class Token
{
    public sealed class V1 : Token
    {
        public required string Payload { get; set; }

        public DateTime UtcExpiry { get; set; }
    }
}
