using Application.Command;
using Domain.Entities;

namespace Application.Handlers;

public class CreateApplicantHandler
{
    public IUnitOfWork UnitWork { get; }
    public IUserContext UserContext { get; }

    public CreateApplicantHandler(IUnitOfWork unitWork, IUserContext userContext)
    {
        UnitWork = unitWork;
        UserContext = userContext;
    }

    public Guid Handle(CreateApplicantCommand createApplicant)
    {
        var currentUser = UserContext.GetCurrentUser();
        var newApplicant = new Applicant(currentUser, createApplicant.ApplicantWorkflow, createApplicant.ApplicantDocument);
        var applicantRepository = UnitWork.GetApplicantRepository();
        var newId = applicantRepository.Create(newApplicant);
        UnitWork.Commit();
        return newId;
    }
}