namespace Example.MondayCom.Services.MondayComApi.Models;

public class MondayComErrorModel
{
    public string Message { get; set; }

    public IEnumerable<MondayComErrorLocationModel> Locations { get; set; } = Enumerable.Empty<MondayComErrorLocationModel>();

    public IEnumerable<string> Fields { get; set; } = Enumerable.Empty<string>();
}

