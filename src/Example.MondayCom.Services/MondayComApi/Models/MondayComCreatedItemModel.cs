using System.Text.Json.Serialization;

namespace Example.MondayCom.Services.MondayComApi.Models;

public class MondayComCreatedItemModel : ResponseModel<MondayComCreatedItemDataModel> { }

public class MondayComCreatedItemDataModel
{
    [JsonPropertyName("create_item")]
    public MondayComCreateItemModel? CreateItem { get; set; }
}

public class MondayComCreateItemModel
{
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
}

