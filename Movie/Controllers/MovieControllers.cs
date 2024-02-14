using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie.Context;
using Movie.Movie;

namespace Movie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieControllers : ControllerBase
    {
        private readonly MovieDbContext _dbcontext;
        public MovieControllers(MovieDbContext dbContext)
        {
            _dbcontext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMovies()
        {
            if (_dbcontext.Movies == null)
            {
                return NotFound();
            }
            return Ok(await _dbcontext.Movies.ToListAsync());
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<Film>> GetMovie([FromRoute] int Id)
        {
            if(_dbcontext.Movies == null)
            {
                return NotFound();
            }
            var film = await _dbcontext.Movies.FirstOrDefaultAsync(a=>a.Id == Id);
            if(film == null)
            {
                return NotFound();
            }
            return film;
        }

        [HttpPost]
        public async Task<IActionResult> CreatMovie([FromBody] Film film)
        {
            _dbcontext.Movies.Add(film);
            await _dbcontext.SaveChangesAsync();
            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return _dbcontext.Movies.Any(f => f.Id == id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie([FromRoute] int id, [FromBody] Film film)
        {
            if (id != film.Id)
            {
                return BadRequest();
            }

            _dbcontext.Movies.Update(film);
            try
            {
                await _dbcontext.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if(!MovieExists(id))
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

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteMovie(int Id)
        {
            var film = await _dbcontext.Movies.FindAsync(Id);
            if(film == null)
            {
                return NotFound();
            }
            _dbcontext.Movies.Remove(film);
            await _dbcontext.SaveChangesAsync();
            return NoContent();
        }
    }
}
