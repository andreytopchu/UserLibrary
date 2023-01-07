using Domain.Entities;

namespace Application.Command;

public record CreateApplicantCommand(Workflow ApplicantWorkflow, Document ApplicantDocument);