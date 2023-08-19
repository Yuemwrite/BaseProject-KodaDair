namespace Domain.Dto.BaseDto;

public class BaseDto
{
    
    public Guid UserId { get; set; }
    
    public string UserName { get; set; }
    
    public DateTime CreationTime { get; set; }
    
    public byte[] RowVersion { get; set; }
}