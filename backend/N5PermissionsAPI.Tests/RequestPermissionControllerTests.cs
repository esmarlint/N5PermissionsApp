using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using N5PermissionsAPI.Features.RequestPermission;
using MediatR;
using System.Threading;
using N5PermissionsAPI.Application.Features.RequestPermission;
using N5PermissionsAPI.Core.Common;
using N5PermissionsAPI.Features.ModifyPermission;
using N5PermissionsAPI.Core.DTOs;
using N5PermissionsAPI.Application.Features.GetPermissions;
using N5PermissionsAPI.Application.Controllers;

namespace N5PermissionsAPI.Services;

public class RequestPermissionControllerTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<RequestPermissionController>> _mockLogger;
    private readonly RequestPermissionController _controller;

    public RequestPermissionControllerTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<RequestPermissionController>>();
        _controller = new RequestPermissionController(_mockMediator.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task RequestPermission_ShouldReturnCreatedResult()
    {
        // Arrange
        var command = new RequestPermissionCommand
        {
            EmployeeName = "John",
            EmployeeLastName = "Doe",
            PermissionTypeId = 1
        };

        _mockMediator
            .Setup(m => m.Send(It.IsAny<RequestPermissionCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<int>.Success(1));

        // Act
        var result = await _controller.RequestPermission(command);

        // Assert
        var createdResult = result as CreatedAtActionResult;
        createdResult.Should().NotBeNull();
        createdResult.StatusCode.Should().Be(201);
        _mockMediator.Verify(m => m.Send(It.IsAny<RequestPermissionCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RequestPermission_ShouldReturnBadRequestWhenValidationFails()
    {
        // Arrange
        var command = new RequestPermissionCommand
        {
            EmployeeName = "",  // Invalid data
            EmployeeLastName = "Doe",
            PermissionTypeId = 1
        };

        _mockMediator
            .Setup(m => m.Send(It.IsAny<RequestPermissionCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<int>.Fail("Validation failed"));

        // Act
        var result = await _controller.RequestPermission(command);

        // Assert
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult.StatusCode.Should().Be(400);

        // Verify that LogWarning was called
        _mockLogger.Verify(x => x.Log(
            LogLevel.Warning,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Validation failed")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
    }

}

public class ModifyPermissionControllerTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<ModifyPermissionController>> _mockLogger;
    private readonly ModifyPermissionController _controller;

    public ModifyPermissionControllerTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<ModifyPermissionController>>();
        _controller = new ModifyPermissionController(_mockMediator.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task ModifyPermission_ShouldReturnNoContentResult()
    {
        // Arrange
        var command = new ModifyPermissionCommand
        {
            Id = 1,
            EmployeeName = "John",
            EmployeeLastName = "Doe",
            PermissionTypeId = 2
        };

        _mockMediator
            .Setup(m => m.Send(It.IsAny<ModifyPermissionCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<bool>.Success(true));

        // Act
        var result = await _controller.ModifyPermission(1, command);

        // Assert
        var noContentResult = result as NoContentResult;
        noContentResult.Should().NotBeNull();
        noContentResult.StatusCode.Should().Be(204);
        _mockMediator.Verify(m => m.Send(It.IsAny<ModifyPermissionCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ModifyPermission_ShouldReturnBadRequestWhenIdMismatch()
    {
        // Arrange
        var command = new ModifyPermissionCommand
        {
            Id = 2,  // ID does not match the URL
            EmployeeName = "John",
            EmployeeLastName = "Doe",
            PermissionTypeId = 2
        };

        // Act
        var result = await _controller.ModifyPermission(1, command);

        // Assert
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult.StatusCode.Should().Be(400);

        _mockLogger.Verify(x => x.Log(
            LogLevel.Warning,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("ID mismatch")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
    }

}

public class GetPermissionsControllerTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<GetPermissionsController>> _mockLogger;
    private readonly GetPermissionsController _controller;

    public GetPermissionsControllerTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<GetPermissionsController>>();
        _controller = new GetPermissionsController(_mockMediator.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetPermissionById_ShouldReturnOkResult_WhenPermissionExists()
    {
        // Arrange
        var permissionId = 1;
        var permissionDto = new PermissionDto
        {
            Id = permissionId,
            EmployeeName = "John",
            EmployeeLastName = "Doe",
            PermissionTypeId = 1,
            PermissionDate = DateTime.UtcNow,
            PermissionTypeName = "Admin"
        };

        _mockMediator
            .Setup(m => m.Send(It.IsAny<GetPermissionByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<PermissionDto>.Success(permissionDto));

        // Act
        var result = await _controller.GetPermissionById(permissionId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);

        var resultValue = okResult.Value as Result<PermissionDto>;
        resultValue.Should().NotBeNull(); 

        resultValue.Value.Should().BeEquivalentTo(permissionDto);
        _mockMediator.Verify(m => m.Send(It.IsAny<GetPermissionByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }



    [Fact]
    public async Task GetPermissionById_ShouldReturnNotFound_WhenPermissionDoesNotExist()
    {
        // Arrange
        var permissionId = 1;

        _mockMediator
            .Setup(m => m.Send(It.IsAny<GetPermissionByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<PermissionDto>.Fail("Permission not found"));

        // Act
        var result = await _controller.GetPermissionById(permissionId);

        // Assert
        var notFoundResult = result.Result as NotFoundObjectResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult.StatusCode.Should().Be(404);
        notFoundResult.Value.Should().Be("Permission not found");

        // Verifica el log, ajustando el mensaje al que realmente se está registrando
        _mockLogger.Verify(x => x.Log(
            LogLevel.Warning,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("GetPermissionById query failed: Permission not found")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
    }


    [Fact]
    public async Task GetAllPermissions_ShouldReturnOkResult_WithPaginatedPermissions()
    {
        // Arrange
        var paginationParams = new PaginationParams { PageNumber = 1, PageSize = 10 };
        var permissions = new PaginatedResult<PermissionDto>(new List<PermissionDto>
        {
            new PermissionDto { Id = 1, EmployeeName = "John", EmployeeLastName = "Doe", PermissionTypeId = 1, PermissionDate = DateTime.UtcNow, PermissionTypeName = "Admin" }
        }, 1, paginationParams.PageNumber, paginationParams.PageSize);

        _mockMediator
            .Setup(m => m.Send(It.IsAny<GetPermissionsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<PaginatedResult<PermissionDto>>.Success(permissions));

        // Act
        var result = await _controller.GetAllPermissions(paginationParams);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(permissions);
        _mockMediator.Verify(m => m.Send(It.IsAny<GetPermissionsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
