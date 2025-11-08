using System.Text.Json.Serialization;

namespace CleanArchitecture.Domain.Dtos;
public sealed class GetAccessTokenResponseDto
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = default!;
}
