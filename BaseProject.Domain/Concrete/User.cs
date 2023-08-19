using Domain.Base;
using Domain.Base.Auditing;
using Domain.Enum;

namespace Domain.Concrete;

public class User : IEntity
{
    public Guid Id { get; set; }
    
    public Role Role { get; set; }
    
    public string UserName { get; set; }
    
    public string Email { get; set; }
    
    public string MobilePhoneNumber { get; set; }
    
    public string Password { get; set; }

    public DateTime ExpirationTime { get; set; }
    
    public string RefreshToken { get; set; }
    
    public DateTime RefreshTokenTime { get; set; }
    
    public bool IsPrivate { get; set; }

    #region RelationEntities
    
    public virtual ICollection<Sharing> Sharings { get; set; }
    public virtual ICollection<Product> Products { get; set; }
    public virtual ICollection<Follower> FollowerUsers { get; set; }
    public virtual ICollection<Follower> FollowedUsers { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }
    public virtual ICollection<Reply> Replies { get; set; }
    public virtual ICollection<Like> Likes { get; set; }
    public virtual ICollection<Profile> Profiles { get; set; }
    public virtual ICollection<SavedPost> SavedPosts { get; set; }
    public virtual ICollection<Education> Educations { get; set; }
    public virtual ICollection<WorkExpeirence> WorkExpeirences { get; set; }


    #endregion
    
    
    
}