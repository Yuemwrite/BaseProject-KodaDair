namespace Domain.Dto;

public class SavedSharingDto : BaseDto.BaseDto
{
    public Guid Id { get; set; }
    public Guid SharingId { get; set; }
    
    public string SharingTitle { get; set; }

    public bool IsActive { get; set; }
    
    
}