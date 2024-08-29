using MediatR;
using Microsoft.AspNetCore.Mvc;
using N5PermissionsAPI.Features.RequestPermission;

namespace N5PermissionsAPI.Application.Controllers;

[ApiController]
[Route("api/permissions")]
public class RequestPermissionController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<RequestPermissionController> _logger;

    public RequestPermissionController(IMediator mediator, ILogger<RequestPermissionController> logger)
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
}
