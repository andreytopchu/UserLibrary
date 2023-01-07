using Domain.Entities.Enum;
using ErrorOr;

namespace Domain.Entities;

/// <summary>
/// Кандидат
/// </summary>
public class Applicant
{
    /// <summary>
    /// Идентификатор кандидата
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Пользователь, создавший кандидата
    /// </summary>
    public User Author { get; }

    /// <summary>
    /// Рабочий процесс обработки кандидата
    /// </summary>
    public Workflow Workflow { get; }

    /// <summary>
    /// Данные о кандидате
    /// </summary>
    public Document Document { get; }

    /// <summary>
    /// Статус оформления кандитада
    /// </summary>
    public ApplicantStatus Status => CheckWorkflow();

    public Applicant(User user, Workflow workflow, Document document)
    {
        Id = new Guid();
        Author = user;
        Workflow = workflow;
        Document = document;
    }

    public ErrorOr<Task> Approve(User user)
    {
        return Workflow.Approved(user);
    }

    public ErrorOr<Task> Reject(User user)
    {
        return Workflow.Rejected(user);
    }

    public void Reset(User user)
    {
        Workflow.Reset(user);
    }

    private ApplicantStatus CheckWorkflow()
    {
        if (Workflow.IsCompleted)
        {
            return ApplicantStatus.Approved;
        }

        return Workflow.IsRejected ? ApplicantStatus.Rejected : ApplicantStatus.InProgress;
    }
}