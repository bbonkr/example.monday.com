using System.Text.Json.Serialization;

namespace Example.MondayCom.Services.MondayComApi.Models;

public class GetBoardItemByColumnValueVariables
{
    public long BoardId { get; set; }

    public string ColumnId { get; set; } = string.Empty;

    public string ColumnValue { get; set; } = string.Empty;
}
