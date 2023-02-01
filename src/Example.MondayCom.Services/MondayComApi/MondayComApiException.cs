using Example.MondayCom.Services.MondayComApi.Models;

namespace Example.MondayCom.Services.MondayComApi;

public class MondayComApiException : Exception
{
    public MondayComApiException(string message, IMondayComResponseModel? model) : base(message)
    {
        ErrorDetail = model;
    }

    public MondayComApiException(IMondayComResponseModel? model)
        : this(model?.Errors?.FirstOrDefault()?.Message ?? "An unknown error has occurred.", model)
    {

    }

    public IMondayComResponseModel? ErrorDetail { get; private set; }
}