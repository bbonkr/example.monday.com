using System.Text.Json;
using Example.MondayCom.Services.MondayComApi;
using Microsoft.Extensions.Logging;

namespace Example.MondayCom.App;

public class MondayComApiCaller
{
    private readonly MondayComApiClient _client;
    private readonly ILogger _logger;

    public MondayComApiCaller(MondayComApiClient client, ILogger<MondayComApiCaller> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task GetBoardItemsAsync(CancellationToken cancellationToken = default)
    {
        long boardId = 3867392294;
        try
        {
            var result = await _client.GetBoardItemsAsync(boardId);

            if (result == null)
            {
                _logger.LogInformation("Result is null");
            }

            var board = result?.Data?.Boards?.FirstOrDefault();

            if (board == null)
            {
                _logger.LogInformation("Board not found");
            }

            _logger.LogInformation("Board name: {name}", board.Name);
            _logger.LogInformation("Raw data: {json}", JsonSerializer.Serialize(board));
        }
        catch (MondayComApiException ex)
        {
            _logger.LogError(ex, "message: {message}", ex.Message);
            _logger.LogError("Error: {error}", JsonSerializer.Serialize(ex.ErrorDetail));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "message: {message}", ex.Message);
        }
    }
}
