using ErrorOr;

namespace Domain.Common.Errors;

public static partial class Errors
{
    public static class Workflow
    {
        public static Error IsAlreadyCompleted => Error.Conflict(code: "Workflow.IsAlreadyCompleted",
            description: "All steps have been completed");

        public static Error IsAlreadyRejected => Error.Conflict(code: "Workflow.IsAlreadyRejected",
            description: "This workflow has been rejected");
    }
}