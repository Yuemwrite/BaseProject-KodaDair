using Domain.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Concrete;

public class ErrorLog : IEntity
{
    [BsonId]
    public ObjectId Id { get; set; }
    
    public string UserName { get; set; }
    
    public string ErrorType { get; set; }
    
    public string ErrorDescription { get; set; }
    
    public DateTime CreationTime { get; set; }
    
}