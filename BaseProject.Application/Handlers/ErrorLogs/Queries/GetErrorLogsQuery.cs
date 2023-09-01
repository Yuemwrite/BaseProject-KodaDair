using BaseProject.Application.MongoDb;
using BaseProject.Application.Wrapper;
using Domain.Concrete;
using MediatR;

namespace BaseProject.Application.Handlers.ErrorLogs.Queries;

public class GetErrorLogsQuery : IRequest<Result>
{
    public class GetErrorLogsQueryHandler : IRequestHandler<GetErrorLogsQuery, Result>
    {
        private readonly IMongoDbService _mongoDbService;

        public GetErrorLogsQueryHandler(IMongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public async Task<Result> Handle(GetErrorLogsQuery request, CancellationToken cancellationToken)
        {
            var errorLogs = await _mongoDbService.List<ErrorLog>("ErrorLog");
            
            return await Result<List<ErrorLog>>.SuccessAsync(errorLogs);
        }
    }
}