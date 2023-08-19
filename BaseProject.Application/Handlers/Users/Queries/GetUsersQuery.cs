using BaseProject.Application.Abstraction.Abstract;
using BaseProject.Application.Wrapper;
using Domain.Concrete;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BaseProject.Application.Handlers.Users.Queries;

public class GetUsersQuery : IRequest<Result>
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _memoryCache;
        
        public GetUsersQueryHandler(IUserRepository userRepository, IMemoryCache memoryCache)
        {
            _userRepository = userRepository;
            _memoryCache = memoryCache;
        }

        public async Task<Result> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            
            
            List<User>? users = new List<User>();

            users = _memoryCache.Get<List<User>>("users");
            if (users is null)
            {
               users =  await _userRepository.Query().Take(1000).ToListAsync();

               _memoryCache.Set("users", users, TimeSpan.FromSeconds(10));
            }
            
            
            return await Result<List<User>>
                .SuccessAsync(users);
        }
    }
}