using System.Text;
using BaseProject.Application.Abstraction.Base;
using Domain.Concrete;
using Quartz;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServiceStack;

namespace BaseProject.Application.TaskScheduler.Jobs;

[DisallowConcurrentExecution]
public class RabbitMqJob : IJob
{
    private readonly IEntityRepository<RabbitMqInfo> _rabbitMqRepository;

    public RabbitMqJob(IEntityRepository<RabbitMqInfo> rabbitMqRepository)
    {
        _rabbitMqRepository = rabbitMqRepository;
    }

    public async Task Execute(IJobExecutionContext context)
    {
       await SendQueue();
    }
    
    private async Task SendQueue()
    {
        var factory = new ConnectionFactory
        {
            Uri = new Uri("amqps://hazblhpu:KYSaWr86uHNtRrH3GmWak90vE5T4L696@rat.rmq2.cloudamqp.com/hazblhpu")
        };
    
        using var connection = factory.CreateConnection();
    
        var channel = connection.CreateModel();
    
        channel.QueueDeclare("send-queue", true, false, false);
    
        var consumer = new EventingBasicConsumer(channel);
        channel.BasicConsume("send-queue", true, consumer);
        consumer.Received += ConsumerOnReceived;
        
    }

    private void ConsumerOnReceived(object? sender, BasicDeliverEventArgs e)
    {
        var result = Encoding.UTF8.GetString(e.Body.ToArray());
        SendInfo(result).GetResult();
    }

    private async Task SendInfo(string info)
    {
        if (_rabbitMqRepository.Query().Any(_ => _.Info == info))
        {
            return;
        }
        
        var rabbitMq = new RabbitMqInfo()
        {
            Info = info
        };
        
        _rabbitMqRepository.Add(rabbitMq);
        await _rabbitMqRepository.SaveChangesAsync();
    }
}