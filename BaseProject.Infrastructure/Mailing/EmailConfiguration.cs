namespace BaseProject.Infrastructure.Mailing;

public class EmailConfiguration
{
    public bool IsEnabled { get; set; }
    public string Host { get; set; }
    
    public int Port { get; set; }
    
    public string Username { get; set; }

    public string Password { get; set; }
    
    public string SenderEmail { get; set; }
    
    public string SenderName { get; set; }
    
    public bool SSL { get; set; }
}