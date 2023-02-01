using System.Text.Json.Serialization;

namespace Example.MondayCom.Services.MondayComApi.Models;

public class MondayComUpdatedItemModel : MondayComResponseModel<MondayComUpdatedItemDataModel> { }

public class MondayComUpdatedItemDataModel
{
    [JsonPropertyName("change_multiple_column_values")]
    public MondayComUpdateItemModel? UpdatedItem { get; set; }
}

public class MondayComUpdateItemModel
{
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
}