﻿using MediatR;
using N5PermissionsAPI.Core.Common;

namespace N5PermissionsAPI.Application.CQRS.PermissionTypes
{
    public class AddPermissionTypeCommand : IRequest<Result<int>>
    {
        public string Description { get; set; }
    }
}
