using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using graduate_proj.Data;
using graduate_proj.Models;

namespace graduate_proj.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminPaymentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminPaymentController(AppDbContext context)
        {
            _context = context;
        }

        // 1. عرض كل عمليات الدفع التي تمت في الموقع
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetAllPayments()
        {
            return await _context.Payments.ToListAsync();
        }

        // 2. عرض تفاصيل عملية دفع واحدة برقمها
        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPaymentById(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null) return NotFound("لم يتم العثور على سجل الدفع");
            return Ok(payment);
        }

        // 3. تحديث حالة الدفع (مثلاً من Pending إلى Completed)
        [HttpPut("{id}/update-status")]
        public async Task<IActionResult> UpdatePaymentStatus(int id, [FromBody] string newStatus)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null) return NotFound();

            payment.Status = newStatus;
            await _context.SaveChangesAsync();

            return Ok(new { message = "تم تحديث حالة الدفع بنجاح" });
        }

        // 4. حذف سجل دفع (للأخطاء الإدارية فقط)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null) return NotFound();

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();

            return Ok(new { message = "تم حذف سجل الدفع بنجاح" });
        }

        // 5. إحصائية: إجمالي الأرباح الكلية
        [HttpGet("total-revenue")]
        public async Task<IActionResult> GetTotalRevenue()
        {
            var total = await _context.Payments
                .Where(p => p.Status == "Completed")
                .SumAsync(p => p.Amount);

            return Ok(new { TotalRevenue = total });
        }
    }
}