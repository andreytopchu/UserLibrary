using Domain.Entities;

namespace Application.Repository;

public interface IRoleRepository
{
    public Role GetById(Guid idRole);
}