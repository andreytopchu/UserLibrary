using Domain.Entities;

namespace Application.Repository;

public interface IUserRepository
{
    public Guid Create(User newUser);

    public User GetById(Guid id);
}