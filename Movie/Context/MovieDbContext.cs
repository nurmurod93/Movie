using Microsoft.EntityFrameworkCore;
using Movie.Movie;

namespace Movie.Context
{
    public class MovieDbContext : DbContext
    {
        public MovieDbContext(DbContextOptions<MovieDbContext>options) : base(options)
        {

        }
        public DbSet<Film> Movies { get; set; }
        public DbSet<Music> Musics { get; set; }
    }
}
