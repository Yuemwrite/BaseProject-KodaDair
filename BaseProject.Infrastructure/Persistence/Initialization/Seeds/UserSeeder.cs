using BaseProject.Application.Utilities.Encryption;
using BaseProject.Infrastructure.Context;
using Domain.Concrete;
using Domain.Enum;
using Microsoft.Extensions.Logging;

namespace BaseProject.Infrastructure.Persistence.Initialization.Seeds;

public class UserSeeder : ICustomSeeder
{
    private readonly ApplicationDbContext _db;


    public UserSeeder(ApplicationDbContext db)
    {
        _db = db;
    }

    public void Initialize()
    {
        if (_db.Users.Any()) return;
        _db.Users.AddRange(
            new User()
            {
                UserName = "Admin",
                Password = PasswordToolkit.EnhancedHashPassword("123456"),
                Email = "admin@admin.com",
                MobilePhoneNumber = "05555555555",
                Role = Role.Administrator,
                ExpirationTime = DateTime.UtcNow.AddYears(3)
            });
        _db.SaveChanges();
    }
}