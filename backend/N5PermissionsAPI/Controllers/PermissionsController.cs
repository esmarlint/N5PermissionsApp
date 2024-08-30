using MediatR;
using Microsoft.AspNetCore.Mvc;
using N5PermissionsAPI.Application.Features.GetPermissions;
using N5PermissionsAPI.Core.Common;
using N5PermissionsAPI.Core.DTOs;
using N5PermissionsAPI.CQRS.ModifyPermission;
using N5PermissionsAPI.CQRS.RequestPermission;

namespace N5PermissionsAPI.Application.Controllers;

[ApiController]
[Route("api/permissions")]
public class PermissionsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<PermissionsController> _logger;

    public PermissionsController(IMediator mediator, ILogger<PermissionsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> RequestPermission([FromBody] RequestPermissionCommand command)
    {
        _logger.LogInformation("Received RequestPermission command for {EmployeeName} {EmployeeLastName}", command.EmployeeName, command.EmployeeLastName);

        var validator = new RequestPermissionValidator();
        var validationResult = await validator.ValidateAsync(command);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for RequestPermission command: {Errors}", validationResult.Errors);
            return BadRequest(validationResult.Errors);
        }

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("RequestPermission failed: {ErrorMessage}", result.ErrorMessage);
            return StatusCode(500, result.ErrorMessage);
        }

        return CreatedAtAction(nameof(RequestPermission), new { id = result.Value }, null);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ModifyPermission(int id, [FromBody] ModifyPermissionCommand command)
    {
        if (id != command.Id)
        {
            _logger.LogWarning("ID mismatch: URL ID {UrlId} does not match body ID {BodyId}", id, command.Id);
            return BadRequest("ID mismatch: URL ID {UrlId} does not match body ID {BodyId}");
        }

        var validator = new ModifyPermissionValidator();
        var validationResult = await validator.ValidateAsync(command);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for ModifyPermission command: {Errors}", validationResult.Errors);
            return BadRequest(validationResult.Errors);
        }

        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            _logger.LogWarning("ModifyPermission failed: {ErrorMessage}", result.ErrorMessage);
            return StatusCode(500, result.ErrorMessage);
        }

        return NoContent();
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
