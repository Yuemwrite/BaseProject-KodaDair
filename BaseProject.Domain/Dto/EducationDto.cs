using Domain.Enum;

namespace Domain.Dto;

public class EducationDto : BaseDto.BaseDto
{
    public long Id { get; set; }
    
    public EducationLevel EducationLevel { get; set; }
    
    public string SchoolName { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
}