using Microsoft.Extensions.Logging;
using MineSweeper.Business.Contracts;
using MineSweeper.Models;

namespace MineSweeper.Business
{
    /// <summary>
    /// Main handler class designed to read user input and perform the game logic on the injected service.
    /// </summary>
    public class MineSweeperHandler
    {
        private readonly IMineSweeperService _service;

        private readonly ILogger<MineSweeperHandler> _logger;

        public MineSweeperHandler(IMineSweeperService service, ILogger<MineSweeperHandler> logger)
        {
            _service = service;
            _logger = logger;
        }

        public void Run()
        {
            _logger.LogInformation("Welcome to a console based Mine Sweeper game demo");
            _logger.LogInformation("Please use one of the 4 arrow keys on your keyboard to move around the grid");
            _logger.LogInformation("The aim of the game is to traverse the grid from the starting position to the far right of the grid without losing all your lives.");
            _logger.LogInformation("Best of luck!");


            while (true)
            {
                // Read the player's input.
                var key = Console.ReadKey().Key;

                // Press 'x' to terminate the game at any time.
                if (key == ConsoleKey.X)
                {
                    break;
                }

                // Invoke move on the service with a direction corresponding to the arrow key pressed
                // by the player.
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        _service.Move(Direction.Up);
                        break;

                    case ConsoleKey.DownArrow:
                        _service.Move(Direction.Down);
                        break;

                    case ConsoleKey.LeftArrow:
                        _service.Move(Direction.Left);
                        break;

                    case ConsoleKey.RightArrow:
                        _service.Move(Direction.Right);
                        break;
                }
            }
        }
    }
}
