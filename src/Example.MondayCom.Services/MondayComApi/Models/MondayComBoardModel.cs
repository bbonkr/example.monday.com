using System.Text.Json.Serialization;

namespace Example.MondayCom.Services.MondayComApi.Models;

public class MondayComBoardModel
{
    public string Name { get; set; }

    public string State { get; set; }

    [JsonPropertyName("board_folder_id")]
    public long? FolderId { get; set; }

    public IEnumerable<MondayComBoardItemModel> Items { get; set; } = Enumerable.Empty<MondayComBoardItemModel>();
}

