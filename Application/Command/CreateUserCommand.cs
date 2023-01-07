namespace Application.Command;

public record CreateUserCommand(string Name, Guid RoleId);