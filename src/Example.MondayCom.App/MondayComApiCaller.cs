using System.Text.Json;
using Example.MondayCom.Services.MondayComApi;
using Example.MondayCom.Services.MondayComApi.Models;
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
            var result = await _client.GetBoardItemsAsync(boardId, cancellationToken);

            if (result == null)
            {
                _logger.LogInformation("Result is null");
            }

            var board = result?.Data?.Boards?.FirstOrDefault();

            if (board == null)
            {
                _logger.LogInformation("Board not found");

                if (result?.HasError ?? false)
                {
                    throw new MondayComApiException(result);
                }
                else
                {
                    throw new Exception("Unknown error occurred");
                }
            }

            _logger.LogInformation("Board name: {name}", board.Name);
            _logger.LogInformation("Raw data: {json}", JsonSerializer.Serialize(board));
            _logger.LogInformation("✅ Items found");
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

    public async Task GetBoardItemByColumnValueAsync(CancellationToken cancellationToken = default)
    {
        long boardId = 3867392294;
        var columnValue = "6aa28ba3-9713-4d3c-a637-75e654137dad";
        var uidColumnText = "uid";

        try
        {
            var getBoardColumnsResult = await _client.GetBoardColumnsAsync(boardId, cancellationToken);
            var columns = getBoardColumnsResult.Data?.Boards.FirstOrDefault()?.Columns;

            if (columns == null || !columns.Any())
            {
                throw new Exception("Columns not found");
            }

            var emailColumn = columns
                .Where(item => !item.Id.Equals(uidColumnText, StringComparison.OrdinalIgnoreCase) && item.Title.Equals(uidColumnText, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();

            if (emailColumn == null)
            {
                throw new Exception("Email column not found");
            }

            var result = await _client.GetBoardItemByColumnValueAsync(boardId, emailColumn.Id, columnValue, cancellationToken);

            if (result == null)
            {
                _logger.LogInformation("Result is null");
            }

            var item = result?.Data?.Items?.FirstOrDefault();

            if (item == null)
            {
                _logger.LogInformation("Item not found");
                if (result?.HasError ?? false)
                {
                    throw new MondayComApiException(result);
                }
                else
                {
                    throw new Exception("Unknown error occurred");
                }
            }

            _logger.LogInformation("Item name: {name}", item.Name);
            _logger.LogInformation("Raw data: {json}", JsonSerializer.Serialize(item));
            _logger.LogInformation("✅ Item found");
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

    public async Task CreateBoardItemAsync(int identifier, CancellationToken cancellationToken = default)
    {
        long boardId = 3867392294;
        var itemName = "Create Board Item test";
        string? groupId = null;

        try
        {
            var getBoardColumnsResult = await _client.GetBoardColumnsAsync(boardId, cancellationToken);

            var columns = getBoardColumnsResult.Data?.Boards.FirstOrDefault()?.Columns;
            var columnValues = CreateItemColumnValues(columns, identifier);

            var result = await _client.CreateBoardItemAsync(boardId, groupId, itemName, columnValues, cancellationToken);

            if (result == null)
            {
                _logger.LogInformation("Result is null");
            }

            var item = result?.Data?.CreateItem;

            if (item == null)
            {
                _logger.LogInformation("Item not found");
                if (result?.HasError ?? false)
                {
                    throw new MondayComApiException(result);
                }
                else
                {
                    throw new Exception("Unknown error occurred");
                }
            }

            _logger.LogInformation("Item id: {id}", item.Id);
            _logger.LogInformation("Raw data: {json}", JsonSerializer.Serialize(item));
            _logger.LogInformation("✅ Item created");
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

    public async Task UpdateBoardItemAsync(int identifier, CancellationToken cancellationToken = default)
    {
        long boardId = 3867392294;
        // long itemId = 3899889309;
        // var uidColumnTitle = "uid";
        var emailColumnTitle = "email";
        var targetEmail = "bbon+test1@bbon.me";

        try
        {
            var getBoardColumnsResult = await _client.GetBoardColumnsAsync(boardId, cancellationToken);

            var columns = getBoardColumnsResult.Data?.Boards.FirstOrDefault()?.Columns;

            if (columns == null || !columns.Any())
            {
                throw new Exception("Columns not found");
            }

            var emailColumn = columns
                .Where(column => column.Title.Equals(emailColumnTitle, StringComparison.OrdinalIgnoreCase))
                .Where(column => !column.Id.Equals(emailColumnTitle, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();

            if (emailColumn == null)
            {
                throw new Exception($"{nameof(emailColumn)} not found");
            }

            // TODO: find item by column value
            var getBoardItemByColumnValueResult = await _client.GetBoardItemByColumnValueAsync(boardId, emailColumn.Id, targetEmail, cancellationToken);

            var foundItem = getBoardItemByColumnValueResult?.Data?.Items.FirstOrDefault();
            if (foundItem == null)
            {
                throw new Exception("Item not found");
            }
            // TODO: map update model
            var columnValues = MapColumnValues(foundItem.ColumnValues);

            if (columnValues == null)
            {
                throw new Exception("Can not map column values");
            }

            // TODO: update item
            columnValues.FirstName = $"{columnValues.FirstName} Updated";
            columnValues.LastName = $"{columnValues.LastName} Updated";

            if (!long.TryParse(foundItem.Id, out long itemId))
            {
                throw new Exception("Item id is invalid");
            }

            var columnValueDictionary = CreateItemColumnValues(foundItem.ColumnValues, identifier, columnValues);

            var result = await _client.UpdateColumnValuesAsync(boardId, itemId, null, columnValueDictionary, cancellationToken);

            if (result == null)
            {
                _logger.LogInformation("Result is null");
            }

            var item = result?.Data?.UpdatedItem;

            if (item == null)
            {
                _logger.LogInformation("Item not found");
                if (result?.HasError ?? false)
                {
                    throw new MondayComApiException(result);
                }
                else
                {
                    throw new Exception("Unknown error occurred");
                }
            }

            _logger.LogInformation("Item id: {id}", item.Id);
            _logger.LogInformation("Raw data: {json}", JsonSerializer.Serialize(item));
            _logger.LogInformation("✅ Item updated");
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

    private IDictionary<string, string> CreateItemColumnValues(IEnumerable<IMondayComBoardColumn>? columnDefinitions, int identifier, ColumnValuesModel? current = null)
    {
        if (columnDefinitions == null || !columnDefinitions.Any())
        {
            throw new ArgumentException("Column definitions are reqired", nameof(columnDefinitions));
        }

        var firstNameColumnText = "First name";
        var lastNameColumnText = "Last name";
        var emailColumnText = "email";
        var languageColumnText = "language";
        var uidColumnText = "uid";

        Dictionary<string, string> values = new();

        foreach (var column in columnDefinitions)
        {
            if (column.Id.Equals("email", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (column.Title.Equals(firstNameColumnText, StringComparison.OrdinalIgnoreCase))
            {

                values.Add(column.Id, current == null ? $"PonCheolTest {identifier}" : current.FirstName);
            }

            if (column.Title.Equals(lastNameColumnText, StringComparison.OrdinalIgnoreCase))
            {

                values.Add(column.Id, current == null ? $"Ku" : current.LastName);
            }

            if (column.Title.Equals(emailColumnText, StringComparison.OrdinalIgnoreCase))
            {
                values.Add(column.Id, current == null ? $"bbon+test{identifier}@bbon.me" : current.Email);
            }

            if (column.Title.Equals(languageColumnText, StringComparison.OrdinalIgnoreCase))
            {

                values.Add(column.Id, current == null ? "ko" : current.Language);
            }

            if (column.Title.Equals(uidColumnText, StringComparison.OrdinalIgnoreCase))
            {
                values.Add(column.Id, current == null ? Guid.NewGuid().ToString() : current.Uid);
            }
        }

        return values;
    }

    private ColumnValuesModel? MapColumnValues(IEnumerable<MondayComBoardItemColumnValueModel> columnValues)
    {
        var firstNameColumnText = "First name";
        var lastNameColumnText = "Last name";
        var emailColumnText = "email";
        var languageColumnText = "language";
        var uidColumnText = "uid";

        if (columnValues == null || !columnValues.Any())
        {
            return null;
        }


        ColumnValuesModel model = new();
        foreach (var columnValue in columnValues)
        {
            if (columnValue.Title.Equals(firstNameColumnText, StringComparison.OrdinalIgnoreCase))
            {
                model.FirstName = columnValue.Text;
            }
            if (columnValue.Title.Equals(lastNameColumnText, StringComparison.OrdinalIgnoreCase))
            {
                model.LastName = columnValue.Text;
            }
            if (columnValue.Title.Equals(emailColumnText, StringComparison.OrdinalIgnoreCase))
            {
                model.Email = columnValue.Text;
            }
            if (columnValue.Title.Equals(languageColumnText, StringComparison.OrdinalIgnoreCase))
            {
                model.Language = columnValue.Text;
            }
            if (columnValue.Title.Equals(uidColumnText, StringComparison.OrdinalIgnoreCase))
            {
                model.Uid = columnValue.Text;
            }
        }

        return model;
    }
}

public class ColumnValuesModel
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Uid { get; set; } = string.Empty;

    public string Language { get; set; } = string.Empty;
}