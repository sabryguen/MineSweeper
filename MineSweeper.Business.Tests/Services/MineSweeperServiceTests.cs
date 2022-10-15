using Microsoft.Extensions.Logging;
using MineSweeper.Business.Contracts;
using MineSweeper.Business.Services;
using MineSweeper.Models;
using Moq;

namespace MineSweeper.Business.Tests.Services
{
    public class MineSweeperServiceTests
    {
        [Fact]
        public void Move_WhenPlayerMovesAcrossTheGrid_NoMines_ExpectGameOver()
        {
            // Arrange
            Mock<ILogger<MineSweeperService>> moqLogger = new Mock<ILogger<MineSweeperService>>();

            IMineSweeperService sut = new MineSweeperService(moqLogger.Object);
            sut.GameGrid = new Models.Grid(2, 2);
            
            // Don't allocate any mines!
            sut.MineLocations.Clear();


            // Act
            sut.Move(Models.Direction.Right);
            sut.Move(Models.Direction.Right);


            // Assert
            Assert.True(sut.GameOver);
            Assert.Equal(3, sut.NumberOfLives);
            Assert.Equal(2, sut.NumberOfMoves);
        }

        [Fact]
        public void Move_WhenThreeMinesHit_ExpectGameOver()
        {
            // Arrange
            Mock<ILogger<MineSweeperService>> moqLogger = new Mock<ILogger<MineSweeperService>>();

            IMineSweeperService sut = new MineSweeperService(moqLogger.Object);
            sut.GameGrid = new Models.Grid(3, 3);


            sut.MineLocations.Add(new Point(1, 0));
            sut.MineLocations.Add(new Point(2, 0));
            sut.MineLocations.Add(new Point(3, 0));

            // Act
            sut.Move(Direction.Right);
            sut.Move(Direction.Right);
            sut.Move(Direction.Right);

            // Assert
            Assert.True(sut.GameOver);
            Assert.Equal(0, sut.NumberOfLives);
            Assert.Equal(3, sut.NumberOfMoves);
        }

        [Fact]
        public void Move_WhenOffGrid_ExpectFalse()
        {
            // Arrange
            Mock<ILogger<MineSweeperService>> moqLogger = new Mock<ILogger<MineSweeperService>>();

            IMineSweeperService sut = new MineSweeperService(moqLogger.Object);
            sut.GameGrid = new Grid(3, 3);
            
            sut.MineLocations.Clear();
            
            // Moves from 0,0 on a 3 x 3 grid should be good.
            Assert.True(sut.Move(Direction.Down));
            Assert.True(sut.Move(Direction.Down));
            Assert.True(sut.Move(Direction.Down));

            // The fourth move down should be false since this move is 
            // expected to take us off grid
            Assert.False(sut.Move(Direction.Down));

            // Assert
            Assert.False(sut.GameOver);
            Assert.Equal(3, sut.NumberOfLives);
            Assert.Equal(3, sut.NumberOfMoves);
        }

        [Fact]
        public void Move_UpFromStartPosition_ExpectOffGrid()
        {
            // Arrange
            Mock<ILogger<MineSweeperService>> moqLogger = new Mock<ILogger<MineSweeperService>>();

            IMineSweeperService sut = new MineSweeperService(moqLogger.Object);
            sut.GameGrid = new Grid(3, 3);

            sut.MineLocations.Clear();

            // Assert
            Assert.False(sut.Move(Direction.Up));
            Assert.False(sut.GameOver);
            Assert.Equal(3, sut.NumberOfLives);
            Assert.Equal(0, sut.NumberOfMoves);
        }

        [Fact]
        public void Move_LeftFromStartPosition_ExpectOffGrid()
        {
            // Arrange
            Mock<ILogger<MineSweeperService>> moqLogger = new Mock<ILogger<MineSweeperService>>();

            IMineSweeperService sut = new MineSweeperService(moqLogger.Object);
            sut.GameGrid = new Grid(3, 3);

            sut.MineLocations.Clear();

            // Assert
            Assert.False(sut.Move(Direction.Left));

            Assert.False(sut.GameOver);
            Assert.Equal(3, sut.NumberOfLives);
            Assert.Equal(0, sut.NumberOfMoves);
        }

        [Fact]
        public void WhenGameOver_ExpectMoveToReturnFalse()
        {
            // Arrange
            Mock<ILogger<MineSweeperService>> moqLogger = new Mock<ILogger<MineSweeperService>>();

            IMineSweeperService sut = new MineSweeperService(moqLogger.Object);
            sut.GameGrid = new Models.Grid(3, 3);


            sut.MineLocations.Add(new Point(1, 0));
            sut.MineLocations.Add(new Point(2, 0));
            sut.MineLocations.Add(new Point(3, 0));

            // Act
            Assert.True(sut.Move(Direction.Right));
            Assert.True(sut.Move(Direction.Right));
            Assert.True(sut.Move(Direction.Right));

            // Move backwards now should be rejected since the game is over.
            Assert.False(sut.Move(Direction.Left));
            
            // Assert
            Assert.True(sut.GameOver);
            Assert.Equal(0, sut.NumberOfLives);
            Assert.Equal(3, sut.NumberOfMoves);
        }

        [Fact]
        public void Move_CheckPlayerLocation()
        {
            // Arrange
            Mock<ILogger<MineSweeperService>> moqLogger = new Mock<ILogger<MineSweeperService>>();

            IMineSweeperService sut = new MineSweeperService(moqLogger.Object);
            sut.GameGrid = new Models.Grid(3, 3);
            
            sut.MineLocations.Clear();
            
            // Act
            Assert.True(sut.Move(Direction.Right));
            Assert.Equal(1, sut.PlayerLocation.X);
            Assert.Equal(0, sut.PlayerLocation.Y);
            Assert.Equal(1, sut.NumberOfMoves);

            Assert.True(sut.Move(Direction.Down));
            Assert.Equal(1, sut.PlayerLocation.X);
            Assert.Equal(1, sut.PlayerLocation.Y);
            Assert.Equal(2, sut.NumberOfMoves);

            Assert.True(sut.Move(Direction.Up));
            Assert.Equal(1, sut.PlayerLocation.X);
            Assert.Equal(0, sut.PlayerLocation.Y);
            Assert.Equal(3, sut.NumberOfMoves);

            Assert.True(sut.Move(Direction.Left));
            Assert.Equal(0, sut.PlayerLocation.X);
            Assert.Equal(0, sut.PlayerLocation.Y);
            Assert.Equal(4, sut.NumberOfMoves);
        }
    }
}