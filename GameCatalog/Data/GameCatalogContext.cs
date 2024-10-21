using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using GameCatalog.Models;

namespace GameCatalog.Data
{
    public class GameCatalogContext: DbContext
    { 
        public GameCatalogContext(DbContextOptions<GameCatalogContext> options)
           : base(options)
        {
        }
        public DbSet<GameCatalog.Models.Game> Game { get; set; } = default!;
    }
}
