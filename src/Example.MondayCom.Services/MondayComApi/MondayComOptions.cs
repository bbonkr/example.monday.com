namespace Example.MondayCom.Services.MondayComApi;

public class MondayComOptions
{
    public const string Name = "MondayCom";

    public string BaseUrl { get; set; } = "https://api.monday.com/v2";
    public string ApiKey { get; set; } = string.Empty;
}
