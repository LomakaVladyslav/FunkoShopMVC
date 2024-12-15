using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FunkoShopDomain.Model;
using FunkoShopInfrastructure;

namespace FunkoShopInfrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesapiController : ControllerBase
    {
        private readonly FunkoShopContext _context;

        public CategoriesapiController(FunkoShopContext context)
        {
            _context = context;
        }

        // GET: api/Categoriesapi
        [HttpGet]
        public async Task<ActionResult> GetCategories([FromQuery] int skip = 0, [FromQuery] int limit = 2)
        {
            // Загальна кількість елементів
            var totalCount = await _context.Categories.CountAsync();

            // Отримання категорій з урахуванням пропуску та обмеження
            var categories = await _context.Categories
                .Skip(skip)
                .Take(limit)
                .ToListAsync();

            // Формування посилання на наступну сторінку
            string nextLink = null;
            if (skip + limit < totalCount)
            {
                nextLink = Url.Action("GetCategories", new { skip = skip + limit, limit });
            }

            // Повернення результату
            return Ok(new { Items = categories, NextLink = nextLink });
        }

        // GET: api/Categoriesapi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        // PUT: api/Categoriesapi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Categoriesapi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostCategory(Category category)
        {
            try
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                return Ok(new { status = "Ok" });
            }
            catch (Exception)
            {
                return BadRequest(new { status = "Error" });
            }
        }

        // DELETE: api/Categoriesapi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
