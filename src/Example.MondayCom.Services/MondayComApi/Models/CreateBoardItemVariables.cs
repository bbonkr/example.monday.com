namespace Example.MondayCom.Services.MondayComApi.Models;

public class CreateBoardItemVariables
{
    public long BoardId { get; set; }

    public string? GroupId { get; set; }

    public string ItemName { get; set; } = string.Empty;

    public string ColumnValues { get; set; } = string.Empty;
}
