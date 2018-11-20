using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealCarAPI.Models;

namespace RealCarAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarItemsController : ControllerBase
    {
        private readonly RealCarAPIContext _context;

        public CarItemsController(RealCarAPIContext context)
        {
            _context = context;
        }

        // GET: api/CarItems
        [HttpGet]
        public IEnumerable<CarItem> GetCarItem()
        {
            return _context.CarItem;
        }

        // GET: api/CarItems/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var carItem = await _context.CarItem.FindAsync(id);

            if (carItem == null)
            {
                return NotFound();
            }

            return Ok(carItem);
        }
   

        // PUT: api/CarItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCarItem([FromRoute] int id, [FromBody] CarItem carItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != carItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(carItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarItemExists(id))
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

        // POST: api/CarItems
        [HttpPost]
        public async Task<IActionResult> PostCarItem([FromBody] CarItem carItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.CarItem.Add(carItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCarItem", new { id = carItem.Id }, carItem);
        }

        // DELETE: api/CarItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var carItem = await _context.CarItem.FindAsync(id);
            if (carItem == null)
            {
                return NotFound();
            }

            _context.CarItem.Remove(carItem);
            await _context.SaveChangesAsync();

            return Ok(carItem);
        }

        private bool CarItemExists(int id)
        {
            return _context.CarItem.Any(e => e.Id == id);
        }

        // GET: api/Meme/Tags
        [Route("tags")]
        [HttpGet]
        public async Task<List<string>> GetTags()
        {
            var CarItem = (from c in _context.CarItem
                         select c.Tags).Distinct();

            var returned = await CarItem.ToListAsync();

            return returned;
        }
    }
}