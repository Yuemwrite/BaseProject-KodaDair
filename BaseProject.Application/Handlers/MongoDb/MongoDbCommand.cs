using BaseProject.Application.MongoDb;
using BaseProject.Application.Wrapper;
using Domain.Concrete;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BaseProject.Application.Handlers.MongoDb;

public class MongoDbCommand : IRequest<Result>
{
    public class MongoDbCommandHandler : IRequestHandler<MongoDbCommand, Result>
    {

        private readonly IMongoDbService _mongoDbService;

        public MongoDbCommandHandler(IMongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public async Task<Result> Handle(MongoDbCommand request, CancellationToken cancellationToken)
        {
            var collection = await _mongoDbService.GetCollection<Report>("Report");

            var report = new Report()
            {
                Id = ObjectId.GenerateNewId(),
                Name = "Test - Repository"
            };

            await collection.InsertOneAsync(report, cancellationToken: cancellationToken);
            
            return await Result<ObjectId>.SuccessAsync("MONGODB TEST");

        }
    }
}