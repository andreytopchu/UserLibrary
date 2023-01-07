namespace Application.Command;

public record AddWorkflowStepByUserCommand(Guid UserId, Guid ApplicantId);