using BaseProject.Infrastructure.Context;
using Domain.Concrete;
using Domain.Enum;

namespace BaseProject.Infrastructure.Persistence.Initialization.Seeds;

public class CategorySeeder : ICustomSeeder
{
    private readonly ApplicationDbContext _db;

    public CategorySeeder(ApplicationDbContext db)
    {
        _db = db;
    }

    public void Initialize()
    {
        if (_db.Categories.Any()) return;
        _db.Categories.AddRange(
            new Category()
            {
                Name = "Backend",
                CreationTime = DateTime.UtcNow,
                CreatorUserId = _db.Users.First(_ => _.Role == Role.Administrator).Id,
                IsDeleted = false
            },
            new Category()
            {
                Name = "Frontend",
                CreationTime = DateTime.UtcNow,
                CreatorUserId = _db.Users.First(_ => _.Role == Role.Administrator).Id,
                IsDeleted = false
            },
            new Category()
            {
                Name = "Mobile",
                CreationTime = DateTime.UtcNow,
                CreatorUserId = _db.Users.First(_ => _.Role == Role.Administrator).Id,
                IsDeleted = false
            },
            new Category()
            {
                Name = "Game",
                CreationTime = DateTime.UtcNow,
                CreatorUserId = _db.Users.First(_ => _.Role == Role.Administrator).Id,
                IsDeleted = false
            });
        _db.SaveChanges();
    }
}