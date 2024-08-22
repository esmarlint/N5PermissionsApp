using MediatR;
using Microsoft.AspNetCore.Mvc;
using N5PermissionsAPI.Core.Models;

namespace N5PermissionsAPI.Features.GetPermissionTypes
{
    [ApiController]
    [Route("api/permission-types")]
    public class GetPermissionTypesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<GetPermissionTypesController> _logger;

        public GetPermissionTypesController(IMediator mediator, ILogger<GetPermissionTypesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PermissionType>>> GetPermissionTypes()
        {
            _logger.LogInformation("Received GetPermissionTypes query");

            var query = new GetPermissionTypesQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("GetPermissionTypes query failed: {ErrorMessage}", result.ErrorMessage);
                return StatusCode(500, result.ErrorMessage);
            }

            return Ok(result.Value);
        }

    }
}