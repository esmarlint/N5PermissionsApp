using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace N5PermissionsAPI.Features.ModifyPermission;

[ApiController]
[Route("api/permissions")]
public class ModifyPermissionController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ModifyPermissionController> _logger;

    public ModifyPermissionController(IMediator mediator, ILogger<ModifyPermissionController> logger)
    {
        _mediator = mediator;
        _logger = logger;
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
            return StatusCode(500, result.ErrorMessage); // O un código de error genérico
        }

        return NoContent();
    }
}
