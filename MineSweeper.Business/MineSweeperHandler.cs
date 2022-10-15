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

        public MineSweeperHandler(IMineSweeperService service)
        {
            _service = service;
        }

        public void Run()
        {
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
