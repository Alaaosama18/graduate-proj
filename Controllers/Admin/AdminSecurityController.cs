using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using graduate_proj.Data;
using graduate_proj.Models;

namespace graduate_proj.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminSecurityController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminSecurityController(AppDbContext context)
        {
            _context = context;
        }

        // 1. عرض قائمة بكل المستخدمين المسجلين (للمراقبة)
        [HttpGet("all-users")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            return await _context.Users
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();
        }

        // 2. حظر أو تفعيل حساب مستخدم (Toggle Active Status)
        [HttpPut("{id}/toggle-status")]
        public async Task<IActionResult> ToggleUserStatus(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound("المستخدم غير موجود");

            user.IsActive = !user.IsActive; // يعكس الحالة (لو نشط يحظره والعكس)
            await _context.SaveChangesAsync();

            string status = user.IsActive ? "تنشيط" : "حظر";
            return Ok(new { message = $"تم {status} حساب المستخدم بنجاح" });
        }

        // 3. تغيير صلاحية مستخدم (ترقية مستخدم ليكون أدمن)
        [HttpPut("{id}/change-role")]
        public async Task<IActionResult> ChangeUserRole(int id, [FromBody] string newRole)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            if (newRole != "Admin" && newRole != "User")
                return BadRequest("الصلاحية المكتوبة غير صحيحة");

            user.Role = newRole;
            await _context.SaveChangesAsync();

            return Ok(new { message = $"تم تغيير صلاحية المستخدم إلى {newRole}" });
        }

        // 4. حذف مستخدم نهائياً
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "تم حذف بيانات المستخدم نهائياً" });
        }
    }
}