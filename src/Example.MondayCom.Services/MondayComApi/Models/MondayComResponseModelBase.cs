using System.Text.Json.Serialization;

namespace Example.MondayCom.Services.MondayComApi.Models;

public abstract class MondayComResponseModelBase
{
    [JsonPropertyName("account_id")]
    public long AccountId { get; set; }
}