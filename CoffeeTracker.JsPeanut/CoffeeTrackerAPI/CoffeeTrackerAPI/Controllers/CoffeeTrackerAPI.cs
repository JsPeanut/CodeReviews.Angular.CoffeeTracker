using CoffeeTrackerAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoffeeTrackerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CoffeeTrackerAPI : ControllerBase
    {
        private readonly CoffeeContext _context;

        public CoffeeTrackerAPI(CoffeeContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Coffee>>> GetCoffees()
        {
            if (_context.Coffees == null)
            {
                return NotFound();
            }
            return await _context.Coffees.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Coffee>> GetCoffee(int id)
        {
            if (_context.Coffees == null)
            {
                return NotFound();
            }

            var coffee = await _context.Coffees.FindAsync(id);

            if (coffee == null)
            {
                return NotFound();
            }

            return coffee;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCoffee(int id, Coffee coffee)
        {
            if (id != coffee.Id)
            {
                return BadRequest();
            }

            _context.Entry(coffee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CoffeeExists(id))
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

        [HttpPost]
        public async Task<ActionResult<Coffee>> PostCoffee(Coffee coffee)
        {
            _context.Coffees.Add(coffee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCoffee), new { id = coffee.Id }, coffee);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCoffee(int id)
        {
            if (_context.Coffees == null)
            {
                return NotFound();
            }
            var coffee = await _context.Coffees.FindAsync(id);
            if (coffee == null)
            {
                return NotFound();
            }
            _context.Coffees.Remove(coffee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CoffeeExists(int id)
        {
            return (_context.Coffees?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}