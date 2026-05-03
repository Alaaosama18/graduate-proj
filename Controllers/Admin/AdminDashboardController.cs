using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using graduate_proj.Data;

namespace graduate_proj.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminDashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminDashboardController(AppDbContext context)
        {
            _context = context;
        }

        // دالة لجلب إحصائيات عامة للموقع (GET: api/AdminDashboard/stats)
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            // 1. عدد العجلات الكلي
            var totalBikes = await _context.Bikes.CountAsync();

            // 2. عدد العجلات المتاحة حالياً
            var availableBikes = await _context.Bikes.CountAsync(b => b.IsAvailable == true);

            // 3. عدد المستخدمين المسجلين
            var totalUsers = await _context.Users.CountAsync();

            // 4. عدد الحجوزات الكلية
            var totalBookings = await _context.Bookings.CountAsync();

            // 5. حساب إجمالي الأرباح المتوقعة (مجموع أسعار العجلات التي تم حجزها)
            // ملاحظة: هذه تعتمد على بنية جدول Booking عندك
            // سنفترض هنا أننا نريد فقط الأرقام الأساسية حالياً

            var stats = new
            {
                TotalBikes = totalBikes,
                AvailableBikes = availableBikes,
                RentedBikes = totalBikes - availableBikes,
                TotalUsers = totalUsers,
                TotalBookings = totalBookings,
                LastUpdated = DateTime.Now
            };

            return Ok(stats);
        }

        // دالة لجلب آخر 5 حجوزات تمت (لعرضها في واجهة الأدمن)
        [HttpGet("recent-bookings")]
        public async Task<IActionResult> GetRecentBookings()
        {
            var recent = await _context.Bookings
                .OrderByDescending(b => b.Id) // ترتيب من الأحدث للأقدم
                .Take(5) // خذ أول 5 فقط
                .ToListAsync();

            return Ok(recent);
        }
    }
}
