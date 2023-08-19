using Domain.Base;

namespace Domain.Concrete;

public class RabbitMqInfo : IEntity
{
    public int Id { get; set; }
    
    public string Info { get; set; }
}