using System.Text;
using BaseProject.Application.Wrapper;
using MediatR;
using RabbitMQ.Client;

namespace BaseProject.Application.Handlers.Users.Commands;

public class RabbitMQTestCommand : IRequest<Result>
{
    public class RabbitMQTestCommandHandler : IRequestHandler<RabbitMQTestCommand, Result>
    {
        public async Task<Result> Handle(RabbitMQTestCommand request, CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("test-url");

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            channel.QueueDeclare("kuyruk-mesaj", true, false, false);

            var body = Encoding.UTF8.GetBytes("Deneme mesaj");
            
            channel.BasicPublish(String.Empty, "kuyruk-mesaj", null, body);
            
            return await Result<object>
                .SuccessAsync("");
        }
    }
}