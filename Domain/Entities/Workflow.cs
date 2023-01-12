using Domain.Common.Errors;
using ErrorOr;

namespace Domain.Entities;

/// <summary>
/// Описание рабочего процесса
/// </summary>
public class Workflow
{
    public Guid Id { get; }

    /// <summary>
    /// Номер текущего шага
    /// </summary>
    public int CurrentStepNumber { get; private set; }

    /// <summary>
    /// Шаги рабочего процесса
    /// </summary>
    public ICollection<WorkflowStep> Steps { get; }

    /// <summary>
    /// Логи статусов рабочего процесса
    /// </summary>
    public ICollection<StatusLogItem> Logs { get; }

    /// <summary>
    /// Выполнен ли процесс
    /// </summary>
    public bool IsCompleted { get; private set; }

    /// <summary>
    /// Отклонен ли процесс
    /// </summary>
    public bool IsRejected { get; private set; }

    public Workflow(ICollection<WorkflowStep> steps)
    {
        if (steps.Count == 0)
        {
            throw new ArgumentException("Steps should not be empty", nameof(steps));
        }

        Steps = steps;
        Logs = new List<StatusLogItem>();
        IsCompleted = false;
        CurrentStepNumber = steps.Min(i => i.StepN);
        Id = new Guid();
    }

    internal ErrorOr<Task> Approved(User user)
    {
        if (IsCompleted)
        {
            return Errors.Workflow.IsAlreadyCompleted;
        }

        if (IsRejected)
        {
            return Errors.Workflow.IsAlreadyRejected;
        }

        var currentStep = Steps.First(i => i.StepN == CurrentStepNumber);
        if (!currentStep.IsCanApprove(user))
        {
            return Errors.WorkflowStep.ForbiddenForUser;
        }
        
        Logs.Add(new StatusLogItem(user, $"Step {currentStep.StepN} approved"));
        IsCompleted = currentStep.StepN == Steps.Max(i => i.StepN);
        CurrentStepNumber = IsCompleted ? CurrentStepNumber : CurrentStepNumber + 1;
        return Task.CompletedTask;
    }

    internal ErrorOr<Task> Rejected(User user)
    {
        if (IsCompleted)
        {
            return Errors.Workflow.IsAlreadyCompleted;
        }

        if (IsRejected)
        {
            return Errors.Workflow.IsAlreadyRejected;
        }

        var currentStep = Steps.First(i => i.StepN == CurrentStepNumber);
        if (!currentStep.IsCanApprove(user))
        {
            return Errors.WorkflowStep.ForbiddenForUser;
        }

        Logs.Add(new StatusLogItem(user, $"step {currentStep.StepN} rejected"));
        IsRejected = true;
        return Task.CompletedTask;
    }

    public void AddStep(User user)
    {
        var newStep = new WorkflowStep(user, Steps.Max(i => i.StepN) + 1);
        Steps.Add(newStep);
        IsCompleted = false;
    }

    public void AddStep(Role role)
    {
        var newStep = new WorkflowStep(role, Steps.Max(i => i.StepN) + 1);
        Steps.Add(newStep);
        IsCompleted = false;
    }

    public void Reset(User user)
    {
        var currentStep = Steps.First(i => i.StepN == CurrentStepNumber);

        if (!currentStep.IsCanApprove(user))
        {
            return;
        }

        IsRejected = false;
        IsCompleted = false;
        CurrentStepNumber = Steps.Min(i => i.StepN);
    }
}