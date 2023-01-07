using ErrorOr;

namespace Domain.Common.Errors;

public static partial class Errors
{
    public static class WorkflowStep
    {
        public static Error ForbiddenForUser => Error.Conflict(code: "WorkflowStep.ForbiddenForUser",
            description: "This user cannot approve the current step");
    }
}