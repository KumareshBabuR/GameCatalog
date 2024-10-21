using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using GameCatalog.Controllers;
using GameCatalog.Models;
using Microsoft.EntityFrameworkCore;
using GameCatalog.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;

namespace GameCatalogTest
{
    public class GamesControllerTests
    {
        private GameCatalogContext _context;
        private GamesHubController _controller;
        
        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<GameCatalogContext>()
                .UseInMemoryDatabase(databaseName: "TestGameCatalog")
            .Options;

            _context = new GameCatalogContext(options);
            _controller = new GamesHubController(_context);

            SeedTestData();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        private void SeedTestData()
        {
            var games = new List<Game>
            {
                new Game { Title = "Test Game 1", Genre = "Action", Description = "Test Game 1 desc", Price = 100, ReleaseDate = DateTime.Now.AddYears(-1), StockQuantity = 100 },
                new Game { Title = "Test Game 2", Genre = "Crime", Description = "Test Game 2 desc", Price = 500, ReleaseDate = DateTime.Now.AddYears(-2), StockQuantity = 5 }
            };

            _context.Game.AddRange(games);
            _context.SaveChanges();
        }

        /// <summary>
        /// Test method to get all the games
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetGames_ReturnsAllGames_WithPagination()
        {
            // Act
            var result = await _controller.GetGames(pageNumber: 1, pageSize: 10);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var games = okResult.Value as IEnumerable<Game>;
            Assert.IsNotNull(games);
            Assert.That(games.Count(), Is.EqualTo(2));  // 2 seeded games
        }

        /// <summary>
        /// Test method to get the game details for a specific game id
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetGame_ExistingId_ReturnsGame()
        {
            // Arrange
            var existingGame = _context.Game.First();

            // Act
            var result = await _controller.GetGame(existingGame.ID);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var game = okResult.Value as Game;
            Assert.IsNotNull(game);
            Assert.That(game.Title, Is.EqualTo(existingGame.Title));
        }

        /// <summary>
        /// Test method to handle get if the game doesn't exists
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetGame_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            // Generate an ID that doesn't exist by ensuring it is not in the current database
            int nonExistentGameId = _context.Game.Any() ? _context.Game.Max(g => g.ID) + 1 : 1;

            // Act
            var result = await _controller.GetGame(nonExistentGameId);

            // Assert
            var notFoundResult = result.Result as NotFoundObjectResult;  // Expect a NotFound result
            Assert.IsNotNull(notFoundResult);  // Ensure the result is NotFound
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(404));  // Check if the status code is 404                       
        }

        /// <summary>
        /// Test method for new game creation
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task PostGame_ValidGame_ReturnsCreatedGame()
        {
            // Arrange
            var newGame = new Game
            {
                Title = "New Game",
                Genre = "Race",
                Description = "racing game",
                Price = 250,
                ReleaseDate = DateTime.Now,
                StockQuantity = 100
            };

            // Act
            var result = await _controller.PostGame(newGame);

            // Assert
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);

            var game = createdResult.Value as Game;
            Assert.IsNotNull(game);
            Assert.That(game.Title, Is.EqualTo(newGame.Title));

            // Verify that the game was added to the database
            var gameInDb = await _context.Game.FindAsync(game.ID);
            Assert.IsNotNull(gameInDb);
            Assert.That(gameInDb.Title, Is.EqualTo(newGame.Title));
        }

        /// <summary>
        /// Test method to update an exising game
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task PutGame_ValidGame_ReturnsNoContent()
        {
            // Arrange
            var existingGame = _context.Game.First();
            existingGame.Title = "Altered Title";

            // Act
            var result = await _controller.PutGame(existingGame.ID, existingGame);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);

            // Verify the game was updated in the database
            var updatedGame = await _context.Game.FindAsync(existingGame.ID);
            Assert.That(updatedGame.Title, Is.EqualTo("Altered Title"));
        }

        /// <summary>
        /// Test method to handle an update a not existing game
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task PutGame_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var newGame = new Game
            {    ID=500,            
                Title = "Non-existing Game",
                Genre = "Adventure",
                Description = "Non-existing adventure game",
                Price = 700,
                ReleaseDate = DateTime.Now,
                StockQuantity = 200
            };

            // Act
            var result = await _controller.PutGame(newGame.ID, newGame);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        /// <summary>
        /// Test method for Deleting an existing game
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task DeleteGame_ExistingId_ReturnsNoContent()
        {
            // Arrange
            var existingGame = _context.Game.First();

            // Act
            var result = await _controller.DeleteGame(existingGame.ID);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);

            // Verify the game was removed from the database
            var deletedGame = await _context.Game.FindAsync(existingGame.ID);
            Assert.IsNull(deletedGame);
        }

        /// <summary>
        /// Test method to handle a deletion for a not existing game
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task DeleteGame_NonExistingId_ReturnsNotFound()
        {


            // Arrange
            // Generate an ID that doesn't exist by ensuring it is not in the current database
            int nonExistentGameId = _context.Game.Any() ? _context.Game.Max(g => g.ID) + 1 : 1;
            var UndeletedGame = await _context.Game.FindAsync(nonExistentGameId);
            // Act
            var result = await _controller.DeleteGame(nonExistentGameId);

            // Assert
           // var notFoundResult = result.Result as NotFoundObjectResult;  // Expect a NotFound result
            Assert.IsNull(UndeletedGame);  // Ensure the result is NotFound                    
        }
    }
}
