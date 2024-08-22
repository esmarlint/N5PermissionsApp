using MediatR;
using Microsoft.EntityFrameworkCore;
using N5PermissionsAPI.Core.Common;
using N5PermissionsAPI.Core.DTOs;
using N5PermissionsAPI.Core.Interfaces;
using N5PermissionsAPI.Infrastructure.Services;

namespace N5PermissionsAPI.Application.Features.GetPermissions;

public class GetPermissionsHandler :
    IRequestHandler<GetPermissionsQuery, Result<PaginatedResult<PermissionDto>>>,
    IRequestHandler<GetPermissionByIdQuery, Result<PermissionDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetPermissionsHandler> _logger;
    private readonly KafkaService _kafkaService;

    public GetPermissionsHandler(IUnitOfWork unitOfWork, ILogger<GetPermissionsHandler> logger, KafkaService kafkaService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _kafkaService = kafkaService;
    }

    public async Task<Result<PaginatedResult<PermissionDto>>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _unitOfWork.Permissions.GetAllAsQueryable()
                .Include(p => p.PermissionType)
                .Select(p => new PermissionDto
                {
                    Id = p.Id,
                    EmployeeName = p.EmployeeName,
                    EmployeeLastName = p.EmployeeLastName,
                    PermissionTypeId = p.PermissionTypeId,
                    PermissionDate = p.PermissionDate,
                    PermissionTypeName = p.PermissionType.Description
                });

            var paginatedResult = await PaginatedResult<PermissionDto>.CreateAsync(query, request.Params.PageNumber, request.Params.PageSize);

            await _kafkaService.ProduceMessageAsync(Constans.Kafka.GetOperation, new
            {
                Operation = "GetPermissions",
                request.Params.PageNumber,
                request.Params.PageSize,
                paginatedResult.TotalCount
            });

            return Result<PaginatedResult<PermissionDto>>.Success(paginatedResult);
        }
        catch (Exception ex)
        {
            return HandleException<PaginatedResult<PermissionDto>>(ex, "retrieving permissions");
        }
    }

    public async Task<Result<PermissionDto>> Handle(GetPermissionByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var permission = await _unitOfWork.Permissions
                .GetAllAsQueryable()
                .Include(p => p.PermissionType)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (permission == null)
            {
                _logger.LogWarning("Permission with ID {Id} not found", request.Id);
                return Result<PermissionDto>.Fail("Permission not found");
            }

            var permissionDto = new PermissionDto
            {
                Id = permission.Id,
                EmployeeName = permission.EmployeeName,
                EmployeeLastName = permission.EmployeeLastName,
                PermissionTypeId = permission.PermissionTypeId,
                PermissionDate = permission.PermissionDate,
                PermissionTypeName = permission.PermissionType!.Description
            };

            return Result<PermissionDto>.Success(permissionDto);
        }
        catch (Exception ex)
        {
            return HandleException<PermissionDto>(ex, $"retrieving permission with ID {request.Id}");
        }
    }

    private Result<T> HandleException<T>(Exception ex, string operation)
    {
        switch (ex)
        {
            case DbUpdateException dbEx:
                _logger.LogError(dbEx, "Database error while {Operation}", operation);
                return Result<T>.Fail("A database error occurred. Please try again later.");
            case HttpRequestException httpEx:
                _logger.LogError(httpEx, "HTTP error while {Operation}", operation);
                return Result<T>.Fail("A network error occurred. Please check your connection and try again.");
            default:
                _logger.LogError(ex, "Unexpected error while {Operation}", operation);
                return Result<T>.Fail("An unexpected error occurred. Please try again later.");
        }
    }
}
