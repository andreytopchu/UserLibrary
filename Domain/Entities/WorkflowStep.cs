namespace Domain.Entities;

/// <summary>
/// Шаг процесса
/// </summary>
public class WorkflowStep
{
    /// <summary>
    /// Номер шага
    /// </summary>
    public int StepN { get; }

    /// <summary>
    /// Пользователь, который может одобрить выполнение этого шага
    /// </summary>
    public User? ApprovedUser { get; }

    /// <summary>
    /// Роль, которая может одобрить выполнение этого шага
    /// </summary>
    public Role? ApprovedRole { get; }

    public WorkflowStep(User user, int stepN)
    {
        ApprovedUser = user ?? throw new ArgumentNullException(nameof(user));
        ApprovedRole = null;
        StepN = stepN;
    }

    public WorkflowStep(Role role, int stepN)
    {
        ApprovedUser = null;
        ApprovedRole = role ?? throw new ArgumentNullException(nameof(role));
        StepN = stepN;
    }

    /// <summary>
    /// Может ли быть одобрено выполнение шага пользователем
    /// </summary>
    /// <param name="user">Пользователь для проверки</param>
    /// <returns>Может ли быть одобрена заявка</returns>
    public bool IsCanApprove(User user)
    {
        if (user is null)
        {
            throw new NullReferenceException("User cannot be null");
        }

        return ApprovedUser?.Equals(user) ?? ApprovedRole!.Equals(user.Role);
    }

    public override bool Equals(object? obj)
    {
        if (obj is WorkflowStep equalStep)
        {
            return equalStep.StepN == StepN && Equals(equalStep.ApprovedRole, ApprovedRole) && Equals(equalStep.ApprovedUser, ApprovedUser);
        }

        return false;
    }

    public override int GetHashCode()
    {
        var hash = StepN;
        if (ApprovedRole is not null)
        {
            hash += ApprovedRole.GetHashCode();
        }

        if (ApprovedUser is not null)
        {
            hash += ApprovedUser.GetHashCode();
        }

        return hash;
    }
}