using BaseProject.Application.Abstraction.Base;
using Domain.Concrete;
using Domain.Enum;

namespace BaseProject.Application.Abstraction.Abstract;

public interface IFollowerRepository : IEntityRepository<Follower>
{
    public ApprovalStatus ChangeApprovalStatus(bool isActive, bool isPrivate);
}