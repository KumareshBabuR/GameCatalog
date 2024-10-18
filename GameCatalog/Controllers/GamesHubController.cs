using GameCatalog.Data;
using GameCatalog.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameCatalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesHubController : ControllerBase
    {
        private readonly GameCatalogContext _gameCatalogContext;
        public GamesHubController(GameCatalogContext context)
        {
            _gameCatalogContext= context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames(int pageNumber = 1, int pageSize = 10)
        {
            var games = await _gameCatalogContext.Game
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(games);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var game = await _gameCatalogContext.Game.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return game;
        }

        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(Game game)
        {
            //ID is an identity column. No need to enter it.

            _gameCatalogContext.Game.Add(game);
            await _gameCatalogContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetGame), new { id = game.ID }, game);          
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, Game game)
        {
            if (id != game.ID)
            {
                return BadRequest();
            }
            _gameCatalogContext.Entry(game).State = EntityState.Modified;
            await _gameCatalogContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _gameCatalogContext.Game.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            _gameCatalogContext.Game.Remove(game);
            await _gameCatalogContext.SaveChangesAsync();
            return NoContent();
        }


    }
}
