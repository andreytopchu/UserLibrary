using Application.Command;

namespace Application.Handlers;

public class ApproveStepHandler
{
    public IUnitOfWork UnitOfWork { get; }
    public IUserContext UserContext { get; }

    public ApproveStepHandler(IUnitOfWork unitOfWork, IUserContext userContext)
    {
        UnitOfWork = unitOfWork;
        UserContext = userContext;
    }

    public void Handle(ApproveStepCommand command)
    {
        var applicantRepository = UnitOfWork.GetApplicantRepository();
        var currentApplicant = applicantRepository.GetById(command.ApplicationId);
        var currentUser = UserContext.GetCurrentUser();
        currentApplicant.Approve(currentUser);
        UnitOfWork.Commit();
    }
}