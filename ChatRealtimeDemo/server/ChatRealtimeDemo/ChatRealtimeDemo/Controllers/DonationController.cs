using ChatRealtimeDemo.Data;
using ChatRealtimeDemo.Models;
using ChatRealtimeDemo.Models.Donate;
using Microsoft.AspNetCore.Mvc;

namespace ChatRealtimeDemo.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class DonationController : Controller
    {
        private readonly ChatRealtimeDbContext db;

        public DonationController(ChatRealtimeDbContext db) {
            this.db = db;
        }

        [HttpGet]
        [Route("get-donations")]
        public IActionResult GetDonations()
        {
            return Ok(db.Donations.OrderByDescending(x => x.DonateDate).ToList());
        }

        [HttpPost]
        [Route("add-donation")]
        public async Task<ActionResult> AddDonation(AddDonation d)
        {
            DateTime currentTime = DateTime.Now;

            var donate = new Donation()
            {
                Id = Guid.NewGuid(),
                UserId = 2,
                DonateMessage = d.DonateMessage,
                DonateUserName = d.DonateUserName,
                DonateMoney = d.DonateMoney,
                DonateDate = currentTime,
            };

            await db.Donations.AddAsync(donate);
            await db.SaveChangesAsync();

            return Ok(donate);
        }
    }
}
