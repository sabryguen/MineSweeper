using Microsoft.Extensions.Logging;
using MineSweeper.Business.Contracts;
using MineSweeper.Models;

namespace MineSweeper.Business.Services
{
    public class MineSweeperService : IMineSweeperService
    {
        /// <summary>
        /// Initialise the player's position at (0,0) on the grid
        /// </summary>
        private Point _playerLocation = new Point(0, 0);

        private ILogger<MineSweeperService> _logger; 

        /// <summary>
        /// Number of moves performed by the player.
        /// </summary>
        private int _moves;

        public MineSweeperService(ILogger<MineSweeperService> logger)
        {
            _logger = logger;

            _logger.LogInformation("Welcome to Mine Sweeper!");
            _logger.LogInformation("Please press the letter 'x' to terminate the game at any time.");

            // setup default game parameters, we'll assume that we're playing on a 8x8 grid
            // The player gets 3 lives and 10% of the grid contains mines!
            NumberOfLives = 3;
            GameGrid = new Grid(8, 8);
            MinePercentage = 10;

            _logger.LogInformation($"Adding mines to grid ({GameGrid.Rows} X {GameGrid.Columns}) with {MinePercentage} % distribution.");

            // Populate the grid with mines.
            AddMines();
        }

        /// <summary>
        /// Adds mines to the grid taking into account mine percentage.
        /// </summary>
        private void AddMines()
        {
            int possibilities = GameGrid.Columns * GameGrid.Rows;

            // Calculate the number of mines that should be added to the grid
            int mineCount = (possibilities * MinePercentage) / 100;

            // Loop across grid rows.
            for (int x = 0; x < GameGrid.Rows; x++)
            {
                // Loop down grid column.
                for (int y = 0; y < GameGrid.Columns; y++)
                {
                    // Randomly add a mine based on frequency.
                    var isMine = new Random().Next(0, 100) < MinePercentage && 
                                 MineLocations.Count <= mineCount;

                    if (isMine)
                    {
                        MineLocations.Add(new Point(x, y));
                    }
                }
            }
        }

        /// <summary>
        /// Is the game still active?
        /// </summary>
        public bool GameOver { get; set; }

        /// <summary>
        /// Tracker for the number of lives that the player has.
        /// </summary>
        public int NumberOfLives { get; set; }

        /// <summary>
        /// Counter used to trac the number of moves that the player has performed.
        /// </summary>
        public int NumberOfMoves => _moves;

        /// <summary>
        /// The player's current location on the grid.
        /// </summary>
        public Point PlayerLocation => _playerLocation;

        /// <summary>
        /// Percentage of mines across the grid vs the number of possibilities.
        /// </summary>
        public int MinePercentage { get; set; }
        
        /// <summary>
        /// Collection used to hold the locations on the grid where the mines have been positioned.
        /// </summary>
        public List<Point> MineLocations { get; set; } = new List<Point>();

        /// <summary>
        /// The grid definition for the game.
        /// </summary>
        public Grid GameGrid { get; set; }

        /// <summary>
        /// Function called each time the user performs a move on the grid.
        /// </summary>
        /// <param name="direction">The direction that the player moved in</param>
        /// <returns>True if the move was valid, false otherwise</returns>
        public bool Move(Direction direction)
        {
            // if the game is over then don't bother handling the move.
            if (GameOver)
            { 
                return false;
            }

            var canMove = false;

            switch (direction)
            {
                case Direction.Up:
                    canMove = (_playerLocation.Y - 1) >= 0;

                    if (canMove)
                    {
                        _playerLocation.Y--;
                    }
                        
                    break;

                case Direction.Down:
                    canMove = (_playerLocation.Y + 1) <= GameGrid.Rows;

                    if (canMove)
                    {
                        _playerLocation.Y++;
                    }   

                    break;

                case Direction.Left:
                    canMove = (_playerLocation.X - 1) >= 0;

                    if (canMove)
                    {
                        _playerLocation.X--;
                    }   

                    break;

                case Direction.Right:
                    canMove = (_playerLocation.X + 1) <= GameGrid.Columns;

                    if (canMove)
                    { 
                        _playerLocation.X++;
                    }

                    break;
            }

            if (canMove)
            {
                // Add to moves.
                _moves++;

                ValidateGameState(direction);
                return true;
            }
            else
            {
                // Warn player.
                _logger.LogWarning($"You cannot move off the grid! (Reminder: The grid size is: {GameGrid.Rows} X {GameGrid.Columns}) - Your current position ({_playerLocation.X},{ _playerLocation.Y })");
                return false;
            }
        }

        /// <summary>
        /// Function used to asses if the game has been won or lost.
        /// </summary>
        /// <param name="direction"></param>
        private void ValidateGameState(Direction direction)
        {
            // inspect the mine locations to see if the current user's position has hit a mine.
            var mineHit = MineLocations.Find(loc => loc.X == _playerLocation.X && 
                                                    loc.Y == _playerLocation.Y);

            _logger.LogInformation("Pressed " + direction + " Current position (" + _playerLocation.X + ", " + _playerLocation.Y + ") - Number of lives " + NumberOfLives + " - Moves " + NumberOfMoves);

            if (mineHit != null)
            {
                ReduceLifeCount();
            }

            // Check if player has reached the end of the grid.
            var gameWon = _playerLocation.X >= GameGrid.Columns && GameOver == false;

            if (gameWon)
            {
                _logger.LogInformation($"Congratulations you won! in { NumberOfMoves } moves! Number of lives left: { NumberOfLives }");
                _logger.LogInformation("Please press 'x' to terminate the game.");
                GameOver = true;
            }
        }

        /// <summary>
        /// Called when a user has hit a mine so we reduce the player's life count by one
        /// </summary>
        private void ReduceLifeCount()
        {
            // Reduce life count since a mine was hit.
            NumberOfLives--;

            // if the number of lives reaches zero then we declare the game over to the user and output a message.
            if (NumberOfLives == 0)
            {
                GameOver = true;
                _logger.LogInformation("You hit a mine - 1 life lost!");
                _logger.LogInformation("GAME OVER! Please press 'x' to terminate the game.");
            }
            else
            {
                _logger.LogInformation("You hit a mine - 1 life lost! - Remaining lives: " + NumberOfLives);
            }
        }
    }
}