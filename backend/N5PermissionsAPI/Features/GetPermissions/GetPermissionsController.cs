using MediatR;
using Microsoft.AspNetCore.Mvc;
using N5PermissionsAPI.Core.Common;
using N5PermissionsAPI.Core.DTOs;

namespace N5PermissionsAPI.Application.Features.GetPermissions;

[ApiController]
[Route("api/permissions")]
public class GetPermissionsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<GetPermissionsController> _logger;

    public GetPermissionsController(IMediator mediator, ILogger<GetPermissionsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResult<PermissionDto>>> GetAllPermissions([FromQuery] PaginationParams @params)
    {
        _logger.LogInformation("Received GetAllPermissions query with pagination params: PageNumber = {PageNumber}, PageSize = {PageSize}", @params.PageNumber, @params.PageSize);

        var query = new GetPermissionsQuery { Params = @params };
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("GetAllPermissions query failed: {ErrorMessage}", result.ErrorMessage);
            return StatusCode(500, result.ErrorMessage);
        }

        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PermissionDto>> GetPermissionById(int id)
    {
        var result = await _mediator.Send(new GetPermissionByIdQuery { Id = id });

        if (!result.IsSuccess)
        {
            _logger.LogWarning("GetPermissionById query failed: {ErrorMessage}", result.ErrorMessage);
            return NotFound(result.ErrorMessage);
        }

        return Ok(result);
    }
}