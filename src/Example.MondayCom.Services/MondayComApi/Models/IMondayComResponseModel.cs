namespace Example.MondayCom.Services.MondayComApi.Models;

public interface IMondayComResponseModel
{
    IEnumerable<MondayComErrorModel>? Errors { get; }

    bool HasError { get; }
}


public interface IMondayComResponseModel<TResult> : IMondayComResponseModel
{
    TResult? Data { get; }
}