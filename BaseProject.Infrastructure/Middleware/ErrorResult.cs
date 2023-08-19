namespace BaseProject.Infrastructure.Middleware;

public class ErrorResult
{
    public List<string>? Messages { get; set; } = new();

    public string? Source { get; set; }
    public string? Exception { get; set; }
    public int? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
    public string? ErrorId { get; set; }
    // public string? SupportMessage { get; set; }
    public int StatusCode { get; set; } 
}