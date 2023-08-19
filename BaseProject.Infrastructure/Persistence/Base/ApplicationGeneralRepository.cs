using BaseProject.Infrastructure.Context;

namespace BaseProject.Infrastructure.Persistence.Base;

public class ApplicationGeneralRepository: EfGeneralRepository<ApplicationDbContext>
{
    public ApplicationGeneralRepository(ApplicationDbContext context)
        : base(context)
    { }
}