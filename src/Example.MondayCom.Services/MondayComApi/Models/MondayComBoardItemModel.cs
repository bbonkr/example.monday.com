using System.Text.Json.Serialization;

namespace Example.MondayCom.Services.MondayComApi.Models;

public class MondayComBoardItemModel
{
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; }= string.Empty;

    [JsonPropertyName("column_values")]
    public IEnumerable<MondayComBoardItemColumnValueModel> ColumnValues { get; set; } = Enumerable.Empty<MondayComBoardItemColumnValueModel>();
}

