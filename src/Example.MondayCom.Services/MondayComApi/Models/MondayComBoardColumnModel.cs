namespace Example.MondayCom.Services.MondayComApi.Models;

public class MondayComBoardColumnModel : IMondayComBoardColumn
{
    public string Id { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;
}
