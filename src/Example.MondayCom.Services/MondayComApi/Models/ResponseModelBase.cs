using System.Text.Json.Serialization;

namespace Example.MondayCom.Services.MondayComApi.Models;

public abstract class ResponseModelBase
{
    [JsonPropertyName("account_id")]
    public long AccountId { get; set; }
}