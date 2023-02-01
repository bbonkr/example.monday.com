
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
        if (boardId < 1)
        {
            throw new MondayComApiException("Board id is invalid", null);
        }

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
                throw new MondayComApiException(result);
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
        if (boardId < 1)
        {
            throw new MondayComApiException("Board id is invalid", null);
        }

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
                throw new MondayComApiException(result);
            }

            return result;
        }
        else
        {
            var errors = JsonSerializer.Deserialize<MondayComErrorsModel>(responseBody, _jsonSerializerOptions);

            throw new MondayComApiException(errors);
        }
    }

    public async Task<MondayComItemsModel?> GetBoardItemByColumnValueAsync(long boardId, string columnId, string columnValue, CancellationToken cancellationToken = default)
    {
        if (boardId < 1)
        {
            throw new MondayComApiException("Board id is invalid", null);
        }

        if (string.IsNullOrWhiteSpace(columnId))
        {
            throw new MondayComApiException("Column id is required", null);
        }

        var query = @"query getBoardItemByColumnValue($boardId: Int!, $columnId: String!, $columnValue: String!){
    items: items_by_column_values(board_id: $boardId, column_id: $columnId, column_value: $columnValue) {
        id
        name
        column_values {
            id
            title
            text
        }
    }
}";
        GetBoardItemByColumnValueVariables variables = new()
        {
            BoardId = boardId,
            ColumnId = columnId,
            ColumnValue = columnValue,
        };

        var client = CreateHttpClient();
        var request = CreateHttpRequestMessage(query, "getBoardItemByColumnValue", variables);

        var response = await client.SendAsync(request, cancellationToken);

        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var result = JsonSerializer.Deserialize<MondayComItemsModel>(responseBody, _jsonSerializerOptions);

            if (result?.HasError ?? true)
            {
                throw new MondayComApiException(result);
            }

            return result;
        }
        else
        {
            var errors = JsonSerializer.Deserialize<MondayComErrorsModel>(responseBody, _jsonSerializerOptions);

            throw new MondayComApiException(errors);
        }
    }

    public async Task<MondayComCreatedItemModel?> CreateBoardItemAsync(long boardId, string? groupId, string itemName, IDictionary<string, string> columnValues, CancellationToken cancellationToken = default)
    {
        if (boardId < 1)
        {
            throw new MondayComApiException("Board id is invalid", null);
        }

        if (string.IsNullOrWhiteSpace(itemName))
        {
            throw new MondayComApiException("ItemName is required", null);
        }

        var query = @"mutation createBoardItem($boardId: Int!, $groupId: String, $itemName:String!, $columnValues: JSON){
  create_item(board_id: $boardId, group_id: $groupId, item_name: $itemName, column_values: $columnValues) {
    id
    name
  }
}";

        var columnValuesJson = JsonSerializer.Serialize(columnValues, _jsonSerializerOptions);

        CreateBoardItemVariables variables = new()
        {
            BoardId = boardId,
            GroupId = groupId,
            ItemName = itemName,
            ColumnValues = columnValuesJson,
        };

        var client = CreateHttpClient();
        var request = CreateHttpRequestMessage(query, "createBoardItem", variables);

        var response = await client.SendAsync(request, cancellationToken);

        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var result = JsonSerializer.Deserialize<MondayComCreatedItemModel>(responseBody, _jsonSerializerOptions);

            if (result?.HasError ?? true)
            {
                throw new MondayComApiException(result);
            }

            return result;
        }
        else
        {
            var errors = JsonSerializer.Deserialize<MondayComErrorsModel>(responseBody, _jsonSerializerOptions);

            throw new MondayComApiException(errors);
        }
    }

    public async Task<MondayComUpdatedItemModel?> UpdateColumnValuesAsync(long boardId, long itemId, string? itemName, IDictionary<string, string> columnValues, CancellationToken cancellationToken = default)
    {
        var query = @"mutation updateBoardItem($itemId: Int, $boardId: Int!, $columnValues: JSON!) {
    change_multiple_column_values(item_id:$itemId, board_id:$boardId, column_values: $columnValues) {
        id
	    name
    }
}
";
        if (boardId < 1)
        {
            throw new MondayComApiException("Board id is invalid", null);
        }

        if (itemId < 1)
        {
            throw new MondayComApiException("Item id is invalid", null);
        }

        if (!string.IsNullOrWhiteSpace(itemName))
        {
            columnValues.Add("Name", itemName);
        }

        if (!columnValues.Any())
        {
            throw new MondayComApiException("Column values are invalid", null);
        }

        var columnValuesJson = JsonSerializer.Serialize(columnValues, _jsonSerializerOptions);

        UpdateBoardItemVariables variables = new()
        {
            BoardId = boardId,
            ItemId = itemId,
            ColumnValues = columnValuesJson,
        };

        var client = CreateHttpClient();
        var request = CreateHttpRequestMessage(query, "updateBoardItem", variables);

        var response = await client.SendAsync(request, cancellationToken);

        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var result = JsonSerializer.Deserialize<MondayComUpdatedItemModel>(responseBody, _jsonSerializerOptions);

            if (result?.HasError ?? true)
            {
                throw new MondayComApiException(result);
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
