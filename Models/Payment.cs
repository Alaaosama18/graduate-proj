using System.ComponentModel.DataAnnotations;

namespace graduate_proj.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BookingId { get; set; } // ربط الدفع بعملية حجز معينة

        [Required]
        public decimal Amount { get; set; } // المبلغ المدفوع

        public DateTime PaymentDate { get; set; } = DateTime.Now;

        public string PaymentMethod { get; set; } = "Credit Card"; // (Cash, Visa, PayPal)

        public string Status { get; set; } = "Pending"; // (Pending, Completed, Failed)
    }
}