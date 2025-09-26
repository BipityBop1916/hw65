using chatmvc.Data;
using chatmvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class ChatController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ChatController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: /Chat
    public async Task<IActionResult> Index()
    {
        var messages = await _context.ChatMessages
            .Include(m => m.User)
            .OrderByDescending(m => m.SentAt)
            .Take(30)
            .OrderBy(m => m.SentAt)
            .ToListAsync();

        return View(messages);
    }

    // POST: /Chat/Send
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Send(string content)
    {
        if (string.IsNullOrWhiteSpace(content) || content.Length > 150)
            return BadRequest("Invalid message.");

        var user = await _userManager.GetUserAsync(User);

        var message = new ChatMessage
        {
            UserId = user.Id,
            Text = content,
            SentAt = DateTime.UtcNow
        };

        _context.ChatMessages.Add(message);
        await _context.SaveChangesAsync();

        return PartialView("_ChatMessagePartial", message);
    }

    // GET: /Chat/Update
    public async Task<IActionResult> Update(int lastMessageId)
    {
        var messages = await _context.ChatMessages
            .Include(m => m.User)
            .Where(m => m.Id > lastMessageId)
            .OrderBy(m => m.SentAt)
            .ToListAsync();

        return PartialView("_ChatMessagesPartial", messages);
    }
}