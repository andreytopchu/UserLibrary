namespace Domain.Entities;

/// <summary>
/// Документ кандидата
/// </summary>
/// <param name="FirstName">Имя</param>
/// <param name="LastName">Фамилия</param>
/// <param name="BirthDate">Дата рождения</param>
/// <param name="Phone">Номер телефона</param>
/// <param name="WorkExperience">Опыт работы</param>
public record Document(string FirstName, string LastName, DateTime BirthDate, string Phone, string WorkExperience);