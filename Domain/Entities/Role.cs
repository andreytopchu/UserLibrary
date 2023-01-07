namespace Domain.Entities;

/// <summary>
/// Роль
/// </summary>
public class Role
{
    /// <summary>
    /// Идентифиактор роли
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Название
    /// </summary>
    public string Name { get; }

    public Role(string name)
    {
        Name = name;
        Id = new Guid();
    }

    public override bool Equals(object? obj)
    {
        if (obj is Role role)
        {
            return Id == role.Id && string.Equals(Name, role.Name, StringComparison.OrdinalIgnoreCase);
        }

        return false;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode() + Name.GetHashCode();
    }

    public override string ToString()
    {
        return $"Role: {Name}";
    }
}