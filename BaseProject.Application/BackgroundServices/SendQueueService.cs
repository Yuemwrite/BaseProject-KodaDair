using System.Text;
using BaseProject.Application.Abstraction.Base;
using Domain.Concrete;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ServiceStack;

namespace BaseProject.Application.BackgroundServices;

public class SendQueueService : IHostedService
{
    

    // protected override Task ExecuteAsync(CancellationToken stoppingToken)
    // {
    //     try
    //     {
    //         while (!stoppingToken.IsCancellationRequested)
    //         {
    //             SendQueue();
    //         }
    //         throw new NotImplementedException();
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine(e);
    //         throw;
    //     }
    // }
    //
    // private void SendQueue()
    // {
    //     var factory = new ConnectionFactory
    //     {
    //         Uri = new Uri("amqps://hazblhpu:KYSaWr86uHNtRrH3GmWak90vE5T4L696@rat.rmq2.cloudamqp.com/hazblhpu")
    //     };
    //
    //     using var connection = factory.CreateConnection();
    //
    //     var channel = connection.CreateModel();
    //
    //     channel.QueueDeclare("send-queue", true, false, false);
    //
    //     var consumer = new EventingBasicConsumer(channel);
    //     channel.BasicConsume("send-queue", false, consumer);
    //     consumer.Received += (obj, e) =>
    //     {
    //         var result = Encoding.UTF8.GetString(e.Body.ToArray());
    //         SendInfo(result).GetResult();
    //     };
    // }
    //
    // private async Task SendInfo(string info)
    // {
    //     // RabbitMqInfo rabbitMq = new RabbitMqInfo()
    //     // {
    //     //     Info = info
    //     // };
    //     //
    //     // _rabbitMqInfoRepository.Add(rabbitMq);
    //     // await _rabbitMqInfoRepository.SaveChangesAsync();
    // }
    

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}