using Domain.Base.Auditing;
using Domain.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BaseProject.Infrastructure.Context;

public class ApplicationDbContext : DbContext
{
    
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
        
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<RabbitMqInfo> RabbitMqInfos { get; set; }
    public DbSet<TwoFactorAuthenticationTransaction> TwoFactorAuthenticationTransactions { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Follower> Followers { get; set; }
    public DbSet<Sharing> Sharings { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Reply> Replies { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<SubCategory> SubCategories { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<SavedPost> SavedPosts { get; set; }
    public DbSet<TransactionUser> TransactionUsers{ get; set; }
    public DbSet<Education> Educations{ get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        base.OnModelCreating(builder);
    }

   
}