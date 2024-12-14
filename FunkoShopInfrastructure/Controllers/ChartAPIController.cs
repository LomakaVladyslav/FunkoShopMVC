using FunkoShopInfrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace lab_infrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartAPIController : ControllerBase
    {
        private readonly FunkoShopContext _context;

        public ChartAPIController(FunkoShopContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryData()
        {
            var categoryData = await _context.Figures
                .GroupBy(f => f.Category!.Name)
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .ToListAsync();

            return Ok(categoryData);
        }
    }
}
