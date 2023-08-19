using System.Text.Json.Serialization;
using UuidExtensions;

namespace RinhaBackend2023Q3.Entities;

public class CreatePerson : Person
{
    [JsonIgnore] public new string Id { get; } = Uuid7.String();
}