namespace Example.MondayCom.Services.MondayComApi.Models;

public class MondayComBoardsDataModel
{
    public IEnumerable<MondayComBoardModel>? Boards { get; set; } = Enumerable.Empty<MondayComBoardModel>();
}
