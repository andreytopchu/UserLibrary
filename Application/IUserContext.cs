using Domain.Entities;

namespace Application;

public interface IUserContext
{
    public User GetCurrentUser();
}