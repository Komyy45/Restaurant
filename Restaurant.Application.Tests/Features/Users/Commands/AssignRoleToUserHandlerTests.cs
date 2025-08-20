using Moq;
using Restaurant.Application.Contracts;
using Restaurant.Application.Features.Users.Commands.AssignRoleToUser;

namespace Restaurant.Application.Test.Features.Users.Commands;

public class AssignRoleToUserHandlerTests
{
    [Fact] public async Task Handle_ValidCommand_RoleShouldBeAssignedToUser()
    {
        var roleService = new Mock<IRoleService>();
        var handler = new AssignRoleToUserCommandHandler(roleService.Object);
        var request = new AssignRoleToUserCommand(
            "1",
            "Admin"
            );
        roleService
            .Setup(mock => mock.AssignRoleToUser(It.IsAny<AssignRoleToUserCommand>()));
        
        await handler.Handle(request, CancellationToken.None);

        
        roleService.Verify(
            mock => mock.AssignRoleToUser(It.IsAny<AssignRoleToUserCommand>()), 
            Times.Once);
    }
    
}