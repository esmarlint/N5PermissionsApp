using MediatR;
using N5PermissionsAPI.Core.Common;
using N5PermissionsAPI.Core.Interfaces;
using N5PermissionsAPI.Core.Models;
using N5PermissionsAPI.Infrastructure.Services;

namespace N5PermissionsAPI.CQRS.RequestPermission
{
    public class RequestPermissionHandler : IRequestHandler<RequestPermissionCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ElasticsearchService _elasticsearchService;
        private readonly KafkaService _kafkaService;
        private readonly ILogger<RequestPermissionHandler> _logger;

        public RequestPermissionHandler(IUnitOfWork unitOfWork, ElasticsearchService elasticsearchService, KafkaService kafkaService, ILogger<RequestPermissionHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _elasticsearchService = elasticsearchService;
            _kafkaService = kafkaService;
            _logger = logger;
        }

        public async Task<Result<int>> Handle(RequestPermissionCommand request, CancellationToken cancellationToken)
        {
            var permission = CreatePermission(request);

            try
            {
                await SaveToDatabase(permission);
                await SaveToElasticsearch(permission);
                await ProduceMessageToKafka(permission);

                return Result<int>.Success(permission.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process permission request");
                return Result<int>.Fail("An error occurred while processing the permission request.");
            }
        }

        private Permission CreatePermission(RequestPermissionCommand request)
        {
            return new Permission
            {
                EmployeeName = request.EmployeeName,
                EmployeeLastName = request.EmployeeLastName,
                PermissionTypeId = request.PermissionTypeId,
                PermissionDate = DateTime.UtcNow
            };
        }

        private async Task SaveToDatabase(Permission permission)
        {
            try
            {
                await _unitOfWork.Permissions.AddAsync(permission);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save to database.");
            }
        }

        private async Task SaveToElasticsearch(Permission permission)
        {
            try
            {
                await _elasticsearchService.IndexPermissionAsync(permission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save to Elasticsearch.");
            }
        }

        private async Task ProduceMessageToKafka(Permission permission)
        {
            try
            {
                await _kafkaService.ProduceMessageAsync(Constans.Kafka.RequestOperation, permission);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to produce message to Kafka.");
            }
        }
    }
}