using System;
using Newtonsoft.Json;
using System.Reflection;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace LanguageServerProtocol
{
    static class JsonStuff
    {
        internal sealed class ResourceOperationKindContractResolver : DefaultContractResolver
        {
            private readonly ResourceOperationKindConverter rokConverter = new ResourceOperationKindConverter();

            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                var property = base.CreateProperty(member, memberSerialization);
                if (property.PropertyType == typeof(ResourceOperationKind[]))
                {
                    property.Converter = this.rokConverter;
                }

                return property;
            }
        }

        internal class ResourceOperationKindConverter : JsonConverter<ResourceOperationKind[]>
        {
            public override ResourceOperationKind[] ReadJson(JsonReader reader, Type objectType, ResourceOperationKind[]? existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                if (reader.TokenType != JsonToken.StartArray)
                {
                    throw new JsonSerializationException($"Expected array start, got {reader.TokenType}.");
                }

                var values = new List<ResourceOperationKind>();
                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.EndArray)
                    {
                        break;
                    }

                    values.Add(reader.Value switch
                    {
                        "create" => ResourceOperationKind.Create,
                        "delete" => ResourceOperationKind.Delete,
                        "rename" => ResourceOperationKind.Delete,
                        var badValue => throw new JsonSerializationException($"Could not deserialize {badValue} as ResourceOperationKind."),
                    });
                }

                return values.ToArray();
            }

            public override void WriteJson(JsonWriter writer, ResourceOperationKind[]? value, JsonSerializer serializer)
            {
                writer.WriteStartArray();
                foreach (var element in value ?? Array.Empty<ResourceOperationKind>())
                {
                    writer.WriteValue(element switch
                    {
                        ResourceOperationKind.Create => "create",
                        ResourceOperationKind.Delete => "delete",
                        ResourceOperationKind.Rename => "rename",
                        _ => throw new JsonSerializationException($"Could not serialize {value} as ResourceOperationKind."),
                    });
                }

                writer.WriteEndArray();
            }
        }
    }
}
