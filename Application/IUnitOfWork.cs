using Application.Repository;

namespace Application;

public interface IUnitOfWork
{
    public IRoleRepository GetRoleRepository();

    public IUserRepository GetUserRepository();

    public IApplicantRepository GetApplicantRepository();

    public void Commit();
}