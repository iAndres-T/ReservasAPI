using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ObjectPool;
using ReservasAPI.Data;
using ReservasAPI.Models;
using System.Text;

namespace ReservasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaController : ControllerBase
    {
        private readonly DBReservasContext _context;
        public ReservaController(DBReservasContext context)
        {
            _context = context;
        }

        [HttpGet]
        // GET: api/reserva
        public async Task<ActionResult<IEnumerable<Reserva>>> GetReservas()
        {
            try
            {
                var reservas = await _context.Reservas.ToListAsync();

                if (reservas == null || !reservas.Any())
                    return NoContent();

                return Ok(reservas);
            }
            catch (Exception ex)
            {
                // Aquí puedes loguear el error con algún logger (ej: Serilog, ILogger, etc.)
                return StatusCode(500, $"Ocurrió un error al obtener las reservas: {ex.Message}");
            }
        }

        [HttpGet("new-deployment")]
        public string NewPipeline()
        {
            return "Cambio subido desde Releases Azure DevOps";
        }


        // GET api/reserva/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reserva>> GetReserva(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);

            if (reserva == null)
                return NoContent();

            return reserva;
        }

        // POST api/reserva
        [HttpPost]
        public async Task<ActionResult<Reserva>> PostReserva(Reserva reserva)
        {
            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReserva), new { id = reserva.IdReserva }, reserva);
        }

        // PUT api/reserva/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReserva(int id, Reserva reserva)
        {
            if (id != reserva.IdReserva)
                return BadRequest();

            _context.Entry(reserva).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Reservas.Any(e => e.IdReserva == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE api/reserva/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReserva(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
                return NotFound();

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
