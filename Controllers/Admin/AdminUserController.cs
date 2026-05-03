using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using graduate_proj.Data;
using graduate_proj.Models;

namespace graduate_proj.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminUserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminUserController(AppDbContext context)
        {
            _context = context;
        }

        // 1. عرض قائمة بكل المستخدمين (بيانات أساسية)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // 2. البحث عن مستخدم معين عن طريق البريد الإلكتروني (Email)
        [HttpGet("search/{email}")]
        public async Task<ActionResult<User>> SearchByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return NotFound("لم يتم العثور على مستخدم بهذا البريد");
            return Ok(user);
        }

        // 3. عرض تفاصيل مستخدم "مع تاريخ حجوزاته" 
        // ميزة قوية للأدمن ليرى ماذا استأجر هذا الشخص
        [HttpGet("{id}/history")]
        public async Task<IActionResult> GetUserWithHistory(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound("المستخدم غير موجود");

            var history = await _context.Bookings
                .Where(b => b.UserId == id)
                .ToListAsync();

            var result = new
            {
                UserDetails = user,
                TotalBookings = history.Count,
                BookingHistory = history
            };

            return Ok(result);
        }

        // 4. تعديل بيانات مستخدم (مثلاً الأدمن يعدل اسم مستخدم بناءً على طلبه)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserProfile(int id, [FromBody] User updatedData)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.FullName = updatedData.FullName;
            user.Email = updatedData.Email;
            // لا نعدل الباسورد هنا لأغراض أمنية

            await _context.SaveChangesAsync();
            return Ok(new { message = "تم تحديث بيانات المستخدم بنجاح" });
        }

        // 5. إحصائية سريعة للأدمن: عدد المستخدمين الجدد الذين سجلوا اليوم
        [HttpGet("stats/new-today")]
        public async Task<IActionResult> GetNewUsersToday()
        {
            var count = await _context.Users
                .CountAsync(u => u.CreatedAt.Date == DateTime.Today);

            return Ok(new { NewUsersToday = count });
        }
    }
}