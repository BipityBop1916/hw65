namespace chatmvc.Models;

public class UserStatsViewModel
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string AvatarPath { get; set; }
    public int MessageCount { get; set; }
}