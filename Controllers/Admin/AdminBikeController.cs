using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using graduate_proj.Data;
using graduate_proj.Models;

namespace graduate_proj.Controllers.Admin
{
    // عنوان الوصول للـ API سيكون: api/AdminBike
    [Route("api/[controller]")]
    [ApiController]
    public class AdminBikeController : ControllerBase
    {
        private readonly AppDbContext _context;

        // ربط الكنترولر بقاعدة البيانات من خلال الـ Context
        public AdminBikeController(AppDbContext context)
        {
            _context = context;
        }

        // 1. عرض كل العجلات (GET: api/AdminBike)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bike>>> GetAllBikes()
        {
            var bikes = await _context.Bikes.ToListAsync();
            return Ok(bikes);
        }

        // 2. عرض عجلة واحدة بالـ ID (GET: api/AdminBike/5)
        [HttpGet("{id}")]
        public async Task<ActionResult<Bike>> GetBike(int id)
        {
            var bike = await _context.Bikes.FindAsync(id);

            if (bike == null)
            {
                return NotFound(new { message = "العجلة غير موجودة" });
            }

            return Ok(bike);
        }

        // 3. إضافة عجلة جديدة (POST: api/AdminBike)
        [HttpPost]
        public async Task<ActionResult<Bike>> AddBike(Bike bike)
        {
            if (bike == null)
            {
                return BadRequest("بيانات العجلة غير صحيحة");
            }

            _context.Bikes.Add(bike);
            await _context.SaveChangesAsync();

            // يرجع استجابة برقم 201 مع مكان الوصول للعجلة الجديدة
            return CreatedAtAction(nameof(GetBike), new { id = bike.Id }, bike);
        }

        // 4. تعديل بيانات عجلة موجودة (PUT: api/AdminBike/5)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBike(int id, Bike bike)
        {
            if (id != bike.Id)
            {
                return BadRequest("المعرف (ID) غير متطابق");
            }

            // إعلام قاعدة البيانات أن حالة هذا الكائن تم تعديلها
            _context.Entry(bike).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BikeExists(id))
                {
                    return NotFound(new { message = "العجلة المراد تعديلها غير موجودة" });
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "تم التعديل بنجاح" });
        }

        // 5. حذف عجلة نهائياً (DELETE: api/AdminBike/5)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBike(int id)
        {
            var bike = await _context.Bikes.FindAsync(id);
            if (bike == null)
            {
                return NotFound(new { message = "العجلة غير موجودة" });
            }

            _context.Bikes.Remove(bike);
            await _context.SaveChangesAsync();

            return Ok(new { message = "تم حذف العجلة بنجاح" });
        }

        // دالة مساعدة للتأكد من وجود العجلة
        private bool BikeExists(int id)
        {
            return _context.Bikes.Any(e => e.Id == id);
        }
    }
}
