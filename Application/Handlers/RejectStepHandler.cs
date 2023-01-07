using Application.Command;

namespace Application.Handlers;

public class RejectStepHandler
{
    public IUnitOfWork UnitOfWork { get; }
    public IUserContext UserContext { get; }

    public RejectStepHandler(IUnitOfWork unitOfWork, IUserContext userContext)
    {
        UnitOfWork = unitOfWork;
        UserContext = userContext;
    }

    public void Handle(RejectStepCommand command)
    {
        var applicantRepository = UnitOfWork.GetApplicantRepository();
        var currentApplication = applicantRepository.GetById(command.ApplicantId);
        var currentUser = UserContext.GetCurrentUser();

        currentApplication.Reject(currentUser);
        UnitOfWork.Commit();
    }
}