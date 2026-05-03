using System.ComponentModel.DataAnnotations;

namespace graduate_proj.Models
{
    public class Ride
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int BookingId { get; set; } // ربط الرحلة بالحجز الخاص بها

        public DateTime StartTime { get; set; } = DateTime.Now;

        public DateTime? EndTime { get; set; } // تكون فارغة طالما الرحلة مستمرة

        public double? Distance { get; set; } // المسافة المقطوعة (اختياري)

        public string Status { get; set; } = "Ongoing"; // (Ongoing, Completed, Cancelled)
    }
}