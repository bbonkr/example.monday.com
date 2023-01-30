namespace Example.MondayCom.Services.MondayComApi.Models;

public abstract class ResponseModel<TResult> : ResponseModelBase
{
    public TResult? Data { get; set; } = default;

    public IEnumerable<MondayComErrorModel>? Errors { get; set; }

    public bool HasError => Errors != null;
}

