using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using graduate_proj.Data;
using graduate_proj.Models;

namespace graduate_proj.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminRideController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminRideController(AppDbContext context)
        {
            _context = context;
        }

        // 1. عرض كل الرحلات (تاريخ الرحلات)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ride>>> GetAllRides()
        {
            return await _context.Rides.ToListAsync();
        }

        // 2. عرض الرحلات المستمرة الآن فقط (التي لم تنتهِ بعد)
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<Ride>>> GetActiveRides()
        {
            var activeRides = await _context.Rides
                .Where(r => r.Status == "Ongoing")
                .ToListAsync();
            return Ok(activeRides);
        }

        // 3. إنهاء رحلة يدوياً (مثلاً إذا تعطل هاتف المستخدم)
        [HttpPut("{id}/end-ride")]
        public async Task<IActionResult> EndRide(int id)
        {
            var ride = await _context.Rides.FindAsync(id);
            if (ride == null) return NotFound("الرحلة غير موجودة");

            ride.EndTime = DateTime.Now;
            ride.Status = "Completed";

            // تحديث حالة العجلة المرتبطة بهذه الرحلة لتصبح "متاحة" مرة أخرى
            var booking = await _context.Bookings.FindAsync(ride.BookingId);
            if (booking != null)
            {
                var bike = await _context.Bikes.FindAsync(booking.BikeId);
                if (bike != null) bike.IsAvailable = true;
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "تم إنهاء الرحلة بنجاح وتحديث حالة العجلة" });
        }

        // 4. حذف سجل رحلة
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRide(int id)
        {
            var ride = await _context.Rides.FindAsync(id);
            if (ride == null) return NotFound();

            _context.Rides.Remove(ride);
            await _context.SaveChangesAsync();
            return Ok(new { message = "تم حذف سجل الرحلة" });
        }
    }
}