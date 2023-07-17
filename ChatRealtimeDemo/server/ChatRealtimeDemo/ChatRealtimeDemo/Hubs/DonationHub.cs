using ChatRealtimeDemo.Data;
using ChatRealtimeDemo.Models.Donate;
using Microsoft.AspNetCore.SignalR;

namespace ChatRealtimeDemo.Hubs
{
    public class DonationHub : Hub
    {
        private readonly ChatRealtimeDbContext db;

        public DonationHub(ChatRealtimeDbContext db)
        {
            this.db = db;
        }
        public async Task UpdateDonation()
        {
            var donations = db.Donations.OrderByDescending(x => x.DonateDate).ToList();

            // Call the broadcastMessage method to update clients.
            await Clients.All.SendAsync("GetAllUpdateDonations", donations);
        }
    }
}
