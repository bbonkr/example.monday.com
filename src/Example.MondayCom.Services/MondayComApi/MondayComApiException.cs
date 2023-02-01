using Example.MondayCom.Services.MondayComApi.Models;

namespace Example.MondayCom.Services.MondayComApi;

public class MondayComApiException : Exception
{
    public MondayComApiException(string message, IResponseModel? model) : base(message)
    {
        ErrorDetail = model;
    }

    public MondayComApiException(IResponseModel? model)
        : this(model?.Errors?.FirstOrDefault()?.Message ?? "An unknown error has occurred.", model)
    {

    }

    public IResponseModel? ErrorDetail { get; private set; }
}