using Application.Command;
using Domain.Entities;

namespace Application.Handlers;

public class CreateUserHandler
{
    public IUnitOfWork UnitWork { get; }

    public CreateUserHandler(IUnitOfWork unitWork)
    {
        UnitWork = unitWork;
    }

    public Guid Handle(CreateUserCommand createUserCommand)
    {
        var roleRepository = UnitWork.GetRoleRepository();
        var currentRole = roleRepository.GetById(createUserCommand.RoleId);
        var newUser = new User(createUserCommand.Name, currentRole);
        var userRepository = UnitWork.GetUserRepository();
        var newId = userRepository.Create(newUser);
        UnitWork.Commit();
        return newId;
    }
}