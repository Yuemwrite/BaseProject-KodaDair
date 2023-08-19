using BaseProject.Application.Abstraction.Abstract;
using BaseProject.Infrastructure.Context;
using BaseProject.Infrastructure.Persistence.Base;
using Domain.Concrete;
using Domain.Enum;

namespace BaseProject.Infrastructure.Persistence.Repository;

public class FollowerRepository :  EfEntityRepositoryBase<Follower, ApplicationDbContext>, IFollowerRepository
{
    public FollowerRepository(ApplicationDbContext context) : base(context)
    {
    }

    public ApprovalStatus ChangeApprovalStatus(bool isActive, bool isPrivate)
    {
        var result = (ApprovalStatus)0;
        
        switch (isActive)
        {
            case true when isPrivate:
                result = ApprovalStatus.Unfollow;
                break;
            case false when isPrivate:
                result = ApprovalStatus.Pending;
                break;
        }

        if (!isActive && !isPrivate)
        {
            result = ApprovalStatus.Approved;
        }

        return result;
    }
}