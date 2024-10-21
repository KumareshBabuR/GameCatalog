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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public GamesHubController(GameCatalogContext context)
        {
            _gameCatalogContext= context;
        }

        /// <summary>
        /// Gets the list of all games.
        /// </summary>
        /// <param name="pageNumber"> The Page Number</param>
        /// <param name="pageSize"> The Page Size </param>
        /// <returns> List of all games</returns>
        // GET: api/GamesHub/GetGames
        [HttpGet("GetGames")]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames(int pageNumber = 1, int pageSize = 10)
        {
            var games = await _gameCatalogContext.Game
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(games);
        }

        /// <summary>
        /// Gets the specific game by ID
        /// </summary>
        /// <param name="id">Game ID</param>
        /// <returns>The game with specific ID</returns>
        [HttpGet("GetGame/{id}")]
        // GET: api/GamesHub/GetGame/1
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var game = await _gameCatalogContext.Game.FindAsync(id);
            if (game == null)
            {
                //return NotFound();
                return NotFound(new { message = $"Game with ID {id} not found." });
            }
            return game;
        }

        /// <summary>
        /// To Create a New Game 
        /// </summary>
        /// <param name="game">Details of the Game to be created</param>
        /// <returns> The new created game</returns>
       [HttpPost("PostGame")]
        public async Task<ActionResult<Game>> PostGame(Game game)
        {
            //ID is an identity column. No need to enter it.
            _gameCatalogContext.Game.Add(game);
            await _gameCatalogContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetGame), new { id = game.ID }, game);          
        }

        /// <summary>
        /// To Update an existing game
        /// </summary>
        /// <param name="id"> ID of the game to update</param>
        /// <param name="game"> Game object</param>
        /// <returns> Action result</returns>
        // [HttpPut("{id}")]
        [HttpPut("PutGame/{id}")]
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

        /// <summary>
        /// To Delete a Game
        /// </summary>
        /// <param name="id">ID of the game to be deleted</param>
        /// <returns>Action result</returns>
        [HttpDelete("DeleteGame/{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _gameCatalogContext.Game.FindAsync(id);
            if (game == null)
            {
                // return NotFound();
                return NotFound(new { message = $"Game with ID {id} not found." });
            }
            _gameCatalogContext.Game.Remove(game);
            await _gameCatalogContext.SaveChangesAsync();
            return NoContent();
        }


    }
}
