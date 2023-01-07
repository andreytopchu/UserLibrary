namespace Domain.Entities;

/// <summary>
/// Дто для логирования изменения статусов заявки
/// </summary>
/// <param name="User">Пользователь, поменявший статус</param>
/// <param name="Message">Сообщение, что именно поменялось</param>
public record StatusLogItem(User User, string Message)
{
    /// <summary>
    /// Дата происшедшего действия
    /// </summary>
    public DateTime LogDate { get; } = DateTime.UtcNow;
}