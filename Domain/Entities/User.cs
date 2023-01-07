namespace Domain.Entities;

/// <summary>
/// Пользователь
/// </summary>
public class User
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Полное имя
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Роль
    /// </summary>
    public Role Role { get; }

    public User(string name, Role role)
    {
        Role = role;
        Name = name;
        Id = new Guid();
    }

    public override bool Equals(object? obj)
    {
        if (obj is User user)
        {
            return user.Id == Id && string.Equals(user.Name, Name) && Equals(user.Role, Role);
        }

        return false;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode() + Name.GetHashCode() + Role.GetHashCode();
    }

    public override string ToString()
    {
        return $"User: {Name} with role: {Role}";
    }
}