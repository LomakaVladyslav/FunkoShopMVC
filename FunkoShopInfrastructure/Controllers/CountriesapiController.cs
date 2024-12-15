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
    public class CountriesapiController : ControllerBase
    {
        private readonly FunkoShopContext _context;

        public CountriesapiController(FunkoShopContext context)
        {
            _context = context;
        }

        // GET: api/Countriesapi
        [HttpGet]
        public async Task<ActionResult> GetCountries([FromQuery] int skip = 0, [FromQuery] int limit = 2)
        {
            // Загальна кількість елементів
            var totalCount = await _context.Countries.CountAsync();

            // Отримання країн з урахуванням пропуску та обмеження
            var countries = await _context.Countries
                .Skip(skip)
                .Take(limit)
                .ToListAsync();

            // Формування посилання на наступну сторінку
            string nextLink = null;
            if (skip + limit < totalCount)
            {
                nextLink = Url.Action("GetCountries", new { skip = skip + limit, limit });
            }

            // Повернення результату
            return Ok(new { Items = countries, NextLink = nextLink });
        }

        // GET: api/Countriesapi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Country>> GetCountry(int id)
        {
            var country = await _context.Countries.FindAsync(id);

            if (country == null)
            {
                return NotFound();
            }

            return country;
        }

        // PUT: api/Countriesapi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, Country country)
        {
            if (id != country.Id)
            {
                return BadRequest();
            }

            _context.Entry(country).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountryExists(id))
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

        // POST: api/Countriesapi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostCountry(Country country)
        {
            try
            {
                _context.Countries.Add(country);
                await _context.SaveChangesAsync();

                return Ok(new { status = "Ok" });
            }
            catch (Exception)
            {
                return BadRequest(new { status = "Error" });
            }
        }

        // DELETE: api/Countriesapi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CountryExists(int id)
        {
            return _context.Countries.Any(e => e.Id == id);
        }
    }
}
