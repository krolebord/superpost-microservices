namespace NotifierService.Models;

public class NotificationDto
{
    public required Guid Id { get; set; }
    
    public required DateTime SentAt { get; set; }
    
    public DateTime? ReadAt { get; set; }
    
    public required string Message { get; set; }
    
    public string? ContextType { get; set; }
    
    public string? ContextId { get; set; }
}
