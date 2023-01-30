namespace Example.MondayCom.Services.MondayComApi.Models;

public class MondayComGetBoardItemsModel : ResponseModel<MondayComBoardsDataModel>
{

}

public class MondayComBoardsDataModel
{
    public IEnumerable<MondayComBoardModel>? Boards { get; set; } = Enumerable.Empty<MondayComBoardModel>();
}
