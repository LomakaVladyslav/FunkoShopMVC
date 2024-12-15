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
    public class FiguresapiController : ControllerBase
    {
        private readonly FunkoShopContext _context;

        public FiguresapiController(FunkoShopContext context)
        {
            _context = context;
        }

        // GET: api/Figuresapi
        [HttpGet]
        public async Task<ActionResult> GetFigures([FromQuery] int skip = 0, [FromQuery] int limit = 3)
        {
            // Загальна кількість елементів
            var totalCount = await _context.Figures.CountAsync();

            // Отримання фігур з урахуванням пропуску та обмеження
            var figures = await _context.Figures
                .Skip(skip)
                .Take(limit)
                .ToListAsync();

            // Формування посилання на наступну сторінку
            string nextLink = null;
            if (skip + limit < totalCount)
            {
                nextLink = Url.Action("GetFigures", new { skip = skip + limit, limit });
            }

            // Повернення результату
            return Ok(new { Items = figures, NextLink = nextLink });
        }

        // GET: api/Figuresapi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Figure>> GetFigure(int id)
        {
            var figure = await _context.Figures.FindAsync(id);

            if (figure == null)
            {
                return NotFound();
            }

            return figure;
        }

        // PUT: api/Figuresapi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFigure(int id, Figure figure)
        {
            if (id != figure.Id)
            {
                return BadRequest();
            }

            _context.Entry(figure).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FigureExists(id))
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

        // POST: api/Figuresapi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostFigure(Figure figure)
        {
            try
            {
                _context.Figures.Add(figure);
                await _context.SaveChangesAsync();

                return Ok(new { status = "Ok" });
            }
            catch (Exception)
            {
                return BadRequest(new { status = "Error" });
            }
        }


        // DELETE: api/Figuresapi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFigure(int id)
        {
            var figure = await _context.Figures.FindAsync(id);
            if (figure == null)
            {
                return NotFound();
            }

            _context.Figures.Remove(figure);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FigureExists(int id)
        {
            return _context.Figures.Any(e => e.Id == id);
        }
    }
}
