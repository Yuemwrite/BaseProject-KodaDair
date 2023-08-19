using System.Text;
using RabbitMQ.Client;

namespace BaseProject.Application.RabbitMQ;

public class SetQueues
{
    public static void SendQueue(byte[] datas)
    {
        var factory = new ConnectionFactory();
        factory.Uri = new Uri("amqps://hazblhpu:KYSaWr86uHNtRrH3GmWak90vE5T4L696@rat.rmq2.cloudamqp.com/hazblhpu");

        using var connection = factory.CreateConnection();

        var channel = connection.CreateModel();

        channel.QueueDeclare("send-queue", true, false, false);

        channel.BasicPublish(String.Empty, "send-queue", null, datas);
    }
}