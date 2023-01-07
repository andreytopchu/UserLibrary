namespace Application.Command;

public record AddWorkflowStepByRoleCommand(Guid RoleId, Guid ApplicantId);