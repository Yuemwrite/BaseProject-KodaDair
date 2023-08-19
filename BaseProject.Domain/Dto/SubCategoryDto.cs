namespace Domain.Dto;

public class SubCategoryDto : BaseDto.BaseDto
{
    public long Id { get; set; }
    
    public string Name { get; set; }
    
    public long CategoryId { get; set; }
    
    public string CategoryName { get; set; }
    
}