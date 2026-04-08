using System.Text.Json.Serialization;

namespace Buildforge.App.Domain.Token;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "version")]
[JsonDerivedType(typeof(V1), "v1")]
public abstract class Token
{
    public sealed class V1 : Token
    {
        public required string Payload { get; set; }

        public required string Username { get; set; }

        public required DateTime UtcExpiry { get; set; }
    }
}