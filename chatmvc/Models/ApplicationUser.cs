using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace chatmvc.Models;

public class ApplicationUser : IdentityUser
{ 
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }
    public ICollection<ChatMessage> Messages { get; set; }
    public string? AvatarPath { get; set; }
}

public class ChatMessage
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public string Text { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
}