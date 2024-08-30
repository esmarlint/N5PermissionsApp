using MediatR;
using Microsoft.AspNetCore.Mvc;
using N5PermissionsAPI.Core.Models;
using N5PermissionsAPI.CQRS.GetPermissionTypes;

namespace N5PermissionsAPI.Application.Controllers
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


    }
}