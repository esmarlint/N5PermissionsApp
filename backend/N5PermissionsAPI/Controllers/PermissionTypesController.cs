using MediatR;
using Microsoft.AspNetCore.Mvc;
using N5PermissionsAPI.Features.GetPermissionTypes;
using N5PermissionsAPI.Application.CQRS.PermissionTypes;
using N5PermissionsAPI.Core.Models;
using N5PermissionsAPI.Infrastructure.Services;

namespace N5PermissionsAPI.Application.Controllers
{
    [ApiController]
    [Route("api/permission-types")]
    public class PermissionTypesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PermissionTypesController> _logger;

        public PermissionTypesController(IMediator mediator, ILogger<PermissionTypesController> logger)
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


        [HttpPost]
        public async Task<IActionResult> AddPermissionType([FromBody] AddPermissionTypeCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetPermissionTypes), new { id = result.Value }, null);
            }
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditPermissionType(int id, [FromBody] EditPermissionTypeCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID mismatch");
            }

            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return NotFound(result.ErrorMessage);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemovePermissionType(int id)
        {
            var result = await _mediator.Send(new RemovePermissionTypeCommand { Id = id });
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return NotFound(result.ErrorMessage);
        }
    }

    [ApiController]
    [Route("[controller]")]
    public class KafkaTestController : ControllerBase
    {
        private readonly KafkaService _kafkaService;

        public KafkaTestController(KafkaService kafkaService)
        {
            _kafkaService = kafkaService;
        }

        [HttpPost("produce")]
        public async Task<IActionResult> ProduceMessage([FromBody] string message)
        {
            await _kafkaService.ProduceMessageAsync(message,new {messaje = "jamon"});
            return Ok("Message sent to Kafka");
        }
    }
}