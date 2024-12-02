namespace EventRegistrationWebAPI.DTOs.RegistrationDto
{
    public class SimplifiedRegistrationDto
    {
        public int RegistrationId { get; set; }
        public string UserId { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }
        public DateTime RegistrationDateTime { get; set; }
        public string RegistrationStatus { get; set; }
        public int PlatinumTicketsCount { get; set; }
        public int GoldTicketsCount { get; set; }
        public int SilverTicketsCount { get; set; }
        public DateTime? ApproveDate { get; set; }
        public decimal PaymentAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
    }

}
