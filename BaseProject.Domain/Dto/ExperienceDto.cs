namespace Domain.Dto;

public class ExperienceDto : BaseDto.BaseDto
{

    
    public string CompanyName { get; set; }
    
    public string? Remark { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
}