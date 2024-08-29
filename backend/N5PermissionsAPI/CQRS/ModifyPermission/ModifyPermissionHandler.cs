using MediatR;
using N5PermissionsAPI.Core.Common;
using N5PermissionsAPI.Core.Interfaces;
using N5PermissionsAPI.Infrastructure.Services;
using System.Threading;
using System.Threading.Tasks;

namespace N5PermissionsAPI.Features.ModifyPermission
{
    public class ModifyPermissionHandler : IRequestHandler<ModifyPermissionCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ElasticsearchService _elasticsearchService;
        private readonly KafkaService _kafkaService;
        private readonly ILogger<ModifyPermissionHandler> _logger;

        public ModifyPermissionHandler(IUnitOfWork unitOfWork, 
            ElasticsearchService elasticsearchService,
            KafkaService kafkaService,
            ILogger<ModifyPermissionHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _elasticsearchService = elasticsearchService;
            _kafkaService = kafkaService;
            _logger = logger;
        }

        public async Task<Result<bool>> Handle(ModifyPermissionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var permission = await _unitOfWork.Permissions.GetByIdAsync(request.Id);
                if (permission == null)
                {
                    _logger.LogWarning("Permission with ID {Id} not found", request.Id);
                    return Result<bool>.Fail("Permission not found");
                }

                permission.EmployeeName = request.EmployeeName;
                permission.EmployeeLastName = request.EmployeeLastName;
                permission.PermissionTypeId = request.PermissionTypeId;

                _unitOfWork.Permissions.Update(permission);
                await _unitOfWork.SaveChangesAsync();

                await _elasticsearchService.UpdatePermissionAsync(permission);

                await _kafkaService.ProduceMessageAsync(Constans.Kafka.ModifyOperation,permission);

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error modifying permission with ID {Id}", request.Id);
                return Result<bool>.Fail("An unexpected error occurred while processing your request.");
            }
        }
    }
}
