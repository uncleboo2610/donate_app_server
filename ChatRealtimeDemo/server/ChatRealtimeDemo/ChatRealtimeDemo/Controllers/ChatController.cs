using ChatRealtimeDemo.Data;
using ChatRealtimeDemo.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChatRealtimeDemo.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class ChatController : Controller
    {
        private readonly ChatRealtimeDbContext db;

        public ChatController(ChatRealtimeDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult GetMessages()
        {
            return Ok(db.Messages.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> AddMessages(ChatMessages m)
        {
            var message = new ChatMessages()
            {
                Id = Guid.NewGuid(),
                UserId = m.UserId,
                Message = m.Message,
            };

            await db.Messages.AddAsync(message);
            await db.SaveChangesAsync();

            return Ok(message);
        }
    }
}
