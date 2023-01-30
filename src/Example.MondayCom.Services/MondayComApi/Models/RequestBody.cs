namespace Example.MondayCom.Services.MondayComApi.Models;

public class RequestModel<TVariables> where TVariables : class, new()
{
    public string? OperationName { get; set; }

    public string Query { get; set; } = string.Empty;

    public TVariables? Variables { get; set; }
}
