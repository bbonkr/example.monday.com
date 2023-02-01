namespace Example.MondayCom.Services.MondayComApi.Models;

public interface IResponseModel
{
    IEnumerable<MondayComErrorModel>? Errors { get; }

    bool HasError { get; }
}


public interface IResponseModel<TResult> : IResponseModel
{
    TResult? Data { get; }
}