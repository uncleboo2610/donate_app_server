namespace ChatRealtimeDemo.Models.Donate
{
    public class Donation
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public string DonateUserName { get; set; }
        public double DonateMoney { get; set; }
        public string DonateMessage { get; set; }
        public DateTime DonateDate { get; set; }
    }
}
