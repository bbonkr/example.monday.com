namespace Example.MondayCom.Services.MondayComApi.Models;

public class MondayComItemsDataModel
{
    public IEnumerable<MondayComBoardItemModel> Items { get; set; } = Enumerable.Empty<MondayComBoardItemModel>();
}