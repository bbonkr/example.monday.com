namespace Example.MondayCom.Services.MondayComApi.Models;

public class MondayComBoardItemColumnValueModel : IMondayComBoardColumn
{
    public string Id { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;
}

