using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using RinhaBackend2023Q3.Attributes;

namespace RinhaBackend2023Q3.Entities;

public class Person
{
    [JsonPropertyName("id")] public string Id { get; init; }

    [Required]
    [StringLength(32)]
    [JsonPropertyName("apelido")]
    public required string Nickname { get; init; }

    [Required]
    [StringLength(100)]
    [JsonPropertyName("nome")]
    public required string Name { get; init; }

    [Required]
    [DataType(DataType.Date)]
    [JsonPropertyName("nascimento")]
    public DateOnly BirthDate { get; init; }

    [StringArrayRequired(32)]
    public required string[]? Stack { get; init; }
}