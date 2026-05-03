using System.ComponentModel.DataAnnotations;

namespace graduate_proj.Models
{
    public class Booking
    {
        [Key] // المعرف الخاص بالحجز
        public int Id { get; set; }

        [Required]
        public int BikeId { get; set; } // رقم العجلة التي تم حجزها

        [Required]
        public int UserId { get; set; } // رقم المستخدم الذي قام بالحجز

        public DateTime BookingDate { get; set; } = DateTime.Now; // تاريخ الحجز (افتراضياً الآن)

        public DateTime? ReturnDate { get; set; } // تاريخ الإرجاع (اختياري)

        public decimal TotalPrice { get; set; } // إجمالي السعر
    }
}
