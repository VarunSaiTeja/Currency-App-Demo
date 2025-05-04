namespace CurrencyApp.Data.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public List<UserRole> Roles { get; set; }


    public User()
    {

    }

    public static User Create(string name, string email, string password, List<UserRole> roles)
    {
        return new User
        {
            Name = name,
            Email = email,
            Password = BCrypt.Net.BCrypt.HashPassword(password),
            Roles = roles
        };
    }
}

public enum UserRole
{
    Admin,
    Analyst,
    Customer,
}