using System.Text.Json.Serialization;

namespace Example.MondayCom.Services.MondayComApi.Models;

public class MondayComBoardModel
{
    public string Name { get; set; } = string.Empty;

    public string State { get; set; } = string.Empty;

    [JsonPropertyName("board_folder_id")]
    public long? FolderId { get; set; }

    public IEnumerable<MondayComBoardItemModel> Items { get; set; } = Enumerable.Empty<MondayComBoardItemModel>();
}

