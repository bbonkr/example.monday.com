using System.Text.Json.Serialization;

namespace Example.MondayCom.Services.MondayComApi.Models;

public class MondayComBoardItemModel
{
    public string Id { get; set; }

    public string Name { get; set; }

    [JsonPropertyName("column_values")]
    public IEnumerable<MondayComBoardItemColumnValueModel> ColumnValues { get; set; } = Enumerable.Empty<MondayComBoardItemColumnValueModel>();
}

