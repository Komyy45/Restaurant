using Moq;
using Restaurant.Application.Contracts;
using Restaurant.Application.Features.Users.Commands.AssignRoleToUser;
using Restaurant.Application.Features.Users.Commands.RemoveRoleFromUser;

namespace Restaurant.Application.Test.Features.Users.Commands;

public class RemoveRoleFromUserHandlerTests
{
    [Fact]
    public async Task Handle_ValidCommand_RoleShouldBeRemovedFromUser()
    {
        var roleService = new Mock<IRoleService>();
        var handler = new RemoveRoleFromUserCommandHandler(roleService.Object);
        var request = new RemoveRoleFromUserCommand(
            "1",
            "Admin"
        );
        roleService
            .Setup(mock => mock.RemoveRoleFromUser(It.IsAny<RemoveRoleFromUserCommand>()));
        
        await handler.Handle(request, CancellationToken.None);

        
        roleService.Verify(
            mock => mock.RemoveRoleFromUser(It.IsAny<RemoveRoleFromUserCommand>()), 
            Times.Once);
    }
}