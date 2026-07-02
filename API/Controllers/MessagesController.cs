using AdvancedChat.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdvancedChat.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("room/{roomId}")]
        public async Task<IActionResult> GetRoomMessages(Guid roomId)
        {
            var messages = await _context.Messages
                .Where(m => m.RoomId == roomId)
                .Include(m => m.Sender)
                .OrderBy(m => m.SentAt)
                .Select(m => new
                {
                    m.Id,
                    m.Content,
                    Sender = m.Sender.FullName,
                    m.SentAt
                })
                .ToListAsync();

            return Ok(messages);
        }
    }
}