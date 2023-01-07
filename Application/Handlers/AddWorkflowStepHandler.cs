using Application.Command;

namespace Application.Handlers;

public class AddWorkflowStepHandler
{
    public readonly IUnitOfWork UnitOfWork;

    public AddWorkflowStepHandler(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    public Guid Handle(AddWorkflowStepByRoleCommand command)
    {
        var roleRepository = UnitOfWork.GetRoleRepository();
        var currentRole = roleRepository.GetById(command.RoleId);
        var applicantRepository = UnitOfWork.GetApplicantRepository();
        var currentApplicant = applicantRepository.GetById(command.ApplicantId);

        currentApplicant.Workflow.AddStep(currentRole);
        UnitOfWork.Commit();
        return currentApplicant.Id;
    }

    public Guid Handle(AddWorkflowStepByUserCommand command)
    {
        var userRepository = UnitOfWork.GetUserRepository();
        var currentUser = userRepository.GetById(command.UserId);
        var applicantRepository = UnitOfWork.GetApplicantRepository();
        var currentApplicant = applicantRepository.GetById(command.ApplicantId);

        currentApplicant.Workflow.AddStep(currentUser);
        UnitOfWork.Commit();
        return currentApplicant.Id;
    }
}