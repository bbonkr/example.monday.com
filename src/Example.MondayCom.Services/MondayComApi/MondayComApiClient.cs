
using System.Text;
using System.Text.Json;
using Example.MondayCom.Services.MondayComApi.Models;
using Microsoft.Extensions.Options;

namespace Example.MondayCom.Services.MondayComApi;

public class MondayComApiClient
{
    public const string BASE_URL = "https://api.monday.com/v2";

    private readonly MondayComOptions _mondayComOptions;
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public MondayComApiClient(IOptionsMonitor<MondayComOptions> mondayComOptionsAccessor)
    {
        _mondayComOptions = mondayComOptionsAccessor.CurrentValue;
    }

    public async Task<MondayComGetBoardColumnsModel> GetBoardColumnsAsync(long boardId, CancellationToken cancellationToken = default)
    {
        var query = @"query getBoardColumns($boardId: Int) {
    boards (ids: [$boardId]) {
        columns {
            id
            title
            type
        }       
    }
}";

        GetBoardItemVairables variables = new()
        {
            BoardId = boardId,
        };

        var client = CreateHttpClient();
        var request = CreateHttpRequestMessage(query, "getBoardColumns", variables);

        var response = await client.SendAsync(request, cancellationToken);

        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var result = JsonSerializer.Deserialize<MondayComGetBoardColumnsModel>(responseBody, _jsonSerializerOptions);

            if (result?.HasError ?? true)
            {
                throw new MondayComApiException(new() { Errors = result?.Errors });
            }

            return result;
        }
        else
        {
            var errors = JsonSerializer.Deserialize<MondayComErrorsModel>(responseBody, _jsonSerializerOptions);

            throw new MondayComApiException(errors);
        }

    }

    public async Task<MondayComGetBoardItemsModel?> GetBoardItemsAsync(long boardId, CancellationToken cancellationToken = default)
    {
        var query = @"query getBoard($boardId: Int) {
  boards (ids: [$boardId]) {
    name
    state
    board_folder_id
    items (limit:100) {
      id
      name
      column_values {
        id
        title
        text
      }
    }
  }
}";
        GetBoardItemVairables variables = new()
        {
            BoardId = boardId,
        };

        var client = CreateHttpClient();
        var request = CreateHttpRequestMessage(query, "getBoard", variables);

        var response = await client.SendAsync(request, cancellationToken);

        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var result = JsonSerializer.Deserialize<MondayComGetBoardItemsModel>(responseBody, _jsonSerializerOptions);

            if (result?.HasError ?? true)
            {
                throw new MondayComApiException(new() { Errors = result?.Errors });
            }

            return result;
        }
        else
        {
            var errors = JsonSerializer.Deserialize<MondayComErrorsModel>(responseBody, _jsonSerializerOptions);

            throw new MondayComApiException(errors);
        }
    }

    private HttpClient CreateHttpClient() => new();


    private HttpRequestMessage CreateHttpRequestMessage<TVariables>(string query, string? operationName, TVariables? variables = default)
        where TVariables : class, new()
    {
        HttpRequestMessage requestMessage = new(HttpMethod.Post, BASE_URL);

        requestMessage.Headers.Add("Authorization", _mondayComOptions.ApiKey);

        RequestModel<TVariables> requestBody = new()
        {
            Query = query,
        };

        if (variables != null)
        {
            if (variables != null && string.IsNullOrWhiteSpace(operationName))
            {
                throw new Exception($"${nameof(operationName)} is required to use the variable.");
            }

            requestBody.OperationName = operationName;
            requestBody.Variables = variables;
        }

        var json = JsonSerializer.Serialize(requestBody, _jsonSerializerOptions);
        requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");

        return requestMessage;
    }


}
