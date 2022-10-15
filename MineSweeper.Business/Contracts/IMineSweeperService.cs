using MineSweeper.Models;

namespace MineSweeper.Business.Contracts
{
    public interface IMineSweeperService
    {
        /// <summary>
        /// Is the game still active?
        /// </summary>
        bool GameOver { get; set; }

        /// <summary>
        /// The number of lives left for the player
        /// </summary>
        int NumberOfLives { get; set; }

        /// <summary>
        /// The number of moves performed by the player
        /// </summary>
        int NumberOfMoves { get; }

        /// <summary>
        /// The grid definition for the game  - This defines the number of rows and columns on the grid
        /// </summary>
        Grid GameGrid { get; set; }

        /// <summary>
        /// The player's location on the grid
        /// </summary>
        Point PlayerLocation { get; }

        /// <summary>
        /// The player's move in a certain direction on the grid
        /// </summary>
        /// <param name="direction">The player's move in a certain direction as defined by <see cref="Direction"/></param>
        /// <returns>true if the move succeeds, false otherwise</returns>
        bool Move(Direction direction);

        /// <summary>
        /// The number of mines placed on the grid as a percentage of the size of the grid
        /// </summary>
        int MinePercentage { get; set; }

        /// <summary>
        /// The location of the mines on the grid
        /// </summary>
        List<Point> MineLocations { get; set; }
    }
}
