namespace Example.MondayCom.Services.MondayComApi.Models;

public class UpdateBoardItemVariables
{
    public long BoardId { get; set; }

    public long ItemId { get; set; }

    public string ColumnValues { get; set; } = string.Empty;
}