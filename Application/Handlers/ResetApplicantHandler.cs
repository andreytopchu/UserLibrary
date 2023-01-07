using Application.Command;

namespace Application.Handlers;

internal class ResetApplicantHandler
{
    public IUnitOfWork UnitOfWork { get; }
    public IUserContext UserContext { get; }

    public ResetApplicantHandler(IUnitOfWork unitOfWork, IUserContext userContext)
    {
        UnitOfWork = unitOfWork;
        UserContext = userContext;
    }

    public Guid Handler(ResetApplicantCommand command)
    {
        var contextUser = UserContext.GetCurrentUser();
        var applicantRepository = UnitOfWork.GetApplicantRepository();
        var currentApplication = applicantRepository.GetById(command.ApplicantId);
        currentApplication.Reset(contextUser);
        UnitOfWork.Commit();
        return currentApplication.Id;
    }
}