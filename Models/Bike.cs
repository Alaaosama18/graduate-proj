using System.ComponentModel.DataAnnotations;

namespace graduate_proj.Models
{
    public class Bike
    {
        [Key]
        public int Id { get; set; }

        public string? ModelName { get; set; } // إضافة علامة الاستفهام تجعله يقبل Null

        public string? Color { get; set; } // إضافة علامة الاستفهام

        public decimal RentalPrice { get; set; }

        public bool IsAvailable { get; set; } = true;
    }
}