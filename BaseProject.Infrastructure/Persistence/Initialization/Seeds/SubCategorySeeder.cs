using BaseProject.Infrastructure.Context;
using Domain.Concrete;
using Domain.Enum;

namespace BaseProject.Infrastructure.Persistence.Initialization.Seeds;

public class SubCategorySeeder : ICustomSeeder
{
    private readonly ApplicationDbContext _db;

    public SubCategorySeeder(ApplicationDbContext db)
    {
        _db = db;
    }

    public void Initialize()
    {
        if (_db.SubCategories.Any()) return;
        _db.SubCategories.AddRange(
            new SubCategory()
            {
                Name = "C#",
                CategoryId = _db.Categories.First(_ => _.Name =="Backend").Id,
                CreationTime = DateTime.UtcNow,
                CreatorUserId = _db.Users.First(_ => _.Role == Role.Administrator).Id,
                IsDeleted = false
            },
            new SubCategory()
            {
                Name = "Java",
                CategoryId = _db.Categories.First(_ => _.Name =="Backend").Id,
                CreationTime = DateTime.UtcNow,
                CreatorUserId = _db.Users.First(_ => _.Role == Role.Administrator).Id,
                IsDeleted = false
            },
            new SubCategory()
            {
                Name = "Python",
                CategoryId = _db.Categories.First(_ => _.Name =="Backend").Id,
                CreationTime = DateTime.UtcNow,
                CreatorUserId = _db.Users.First(_ => _.Role == Role.Administrator).Id,
                IsDeleted = false
            },
            new SubCategory()
            {
                Name = "Vue.JS",
                CategoryId = _db.Categories.First(_ => _.Name =="Frontend").Id,
                CreationTime = DateTime.UtcNow,
                CreatorUserId = _db.Users.First(_ => _.Role == Role.Administrator).Id,
                IsDeleted = false
            },
            new SubCategory()
            {
                Name = "React.JS",
                CategoryId = _db.Categories.First(_ => _.Name =="Frontend").Id,
                CreationTime = DateTime.UtcNow,
                CreatorUserId = _db.Users.First(_ => _.Role == Role.Administrator).Id,
                IsDeleted = false
            },
            new SubCategory()
            {
                Name = "Angular.JS",
                CategoryId = _db.Categories.First(_ => _.Name =="Frontend").Id,
                CreationTime = DateTime.UtcNow,
                CreatorUserId = _db.Users.First(_ => _.Role == Role.Administrator).Id,
                IsDeleted = false
            },
            new SubCategory()
            {
                Name = "Flutter",
                CategoryId = _db.Categories.First(_ => _.Name =="Mobile").Id,
                CreationTime = DateTime.UtcNow,
                CreatorUserId = _db.Users.First(_ => _.Role == Role.Administrator).Id,
                IsDeleted = false
            },
            new SubCategory()
            {
                Name = "React Native",
                CategoryId = _db.Categories.First(_ => _.Name =="Mobile").Id,
                CreationTime = DateTime.UtcNow,
                CreatorUserId = _db.Users.First(_ => _.Role == Role.Administrator).Id,
                IsDeleted = false
            },
            new SubCategory()
            {
                Name = "Unity",
                CategoryId = _db.Categories.First(_ => _.Name =="Game").Id,
                CreationTime = DateTime.UtcNow,
                CreatorUserId = _db.Users.First(_ => _.Role == Role.Administrator).Id,
                IsDeleted = false
            });
        _db.SaveChanges();
    }
}