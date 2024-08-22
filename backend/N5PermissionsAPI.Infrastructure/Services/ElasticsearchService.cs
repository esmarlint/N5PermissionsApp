using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nest;
using N5PermissionsAPI.Core.Models;
using N5PermissionsAPI.Infrastructure.Configuration;

namespace N5PermissionsAPI.Infrastructure.Services;

public class ElasticsearchService
{
    private readonly ElasticClient _client;
    private readonly string _indexName;
    private readonly ILogger<ElasticsearchService> _logger;

    public ElasticsearchService(
        IOptions<ElasticsearchOptions> elasticsearchOptions,
        ILogger<ElasticsearchService> logger)
    {
        var options = elasticsearchOptions.Value;
        _indexName = options.IndexName;

        var settings = new ConnectionSettings(new Uri(options.Url))
            .DefaultIndex(_indexName)
            .BasicAuthentication(options.Username, options.Password);

        _client = new ElasticClient(settings);
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        try
        {
            var indexExists = await _client.Indices.ExistsAsync(_indexName);
            if (!indexExists.Exists)
            {
                await _client.Indices.CreateAsync(_indexName, c => c
                    .Map<Permission>(m => m
                        .Properties(ps => ps
                            .Keyword(k => k.Name(n => n.Id))
                            .Text(t => t.Name(n => n.EmployeeName))
                            .Text(t => t.Name(n => n.EmployeeLastName))
                            .Number(n => n.Name(n => n.PermissionTypeId))
                            .Date(d => d.Name(n => n.PermissionDate))
                        )
                    )
                );
                _logger.LogInformation("Index {IndexName} created successfully in Elasticsearch.", _indexName);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize Elasticsearch index {IndexName}.", _indexName);
        }
    }

    public async Task IndexPermissionAsync(Permission permission)
    {
        try
        {
            await _client.IndexDocumentAsync(permission);
            _logger.LogInformation("Permission indexed successfully in Elasticsearch.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to index permission in Elasticsearch.");
        }
    }

    public async Task UpdatePermissionAsync(Permission permission)
    {
        try
        {
            await _client.UpdateAsync<Permission>(permission.Id, u => u
                .Index(_indexName)
                .Doc(permission)
                .DocAsUpsert());
            _logger.LogInformation("Permission with ID {PermissionId} updated successfully in Elasticsearch.", permission.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update permission in Elasticsearch.");
        }
    }

    public async Task<Permission> GetPermissionAsync(int id)
    {
        try
        {
            var response = await _client.GetAsync<Permission>(id, g => g.Index(_indexName));
            if (response.Found)
            {
                _logger.LogInformation("Permission with ID {PermissionId} retrieved successfully from Elasticsearch.", id);
                return response.Source;
            }
            else
            {
                _logger.LogWarning("Permission with ID {PermissionId} not found in Elasticsearch.", id);
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve permission from Elasticsearch.");
            return null;
        }
    }
}
