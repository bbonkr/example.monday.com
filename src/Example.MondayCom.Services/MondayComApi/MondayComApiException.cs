using Example.MondayCom.Services.MondayComApi.Models;

namespace Example.MondayCom.Services.MondayComApi;

public class MondayComApiException : Exception
{
    public MondayComApiException(string message, MondayComErrorsModel? model) : base(message)
    {
        ErrorDetail = model;
    }

    public MondayComApiException(MondayComErrorsModel? model)
        : this(model?.Errors?.FirstOrDefault()?.Message ?? "An unknown error has occurred.", model)
    {

    }

    public MondayComErrorsModel? ErrorDetail { get; private set; }
}