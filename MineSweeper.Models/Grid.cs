namespace MineSweeper.Models
{
    /// <summary>
    /// Represents a Matrix of rows and columns
    /// </summary>
    public class Grid
    {
        public int _rows;
        public int _columns;

        public Grid(int rows, int columns)
        {
            _rows = rows;
            _columns = columns;
        }

        /// <summary>
        /// The number of rows in the grid
        /// </summary>
        public int Rows
        {
            get { return _rows; }
        }

        /// <summary>
        /// The number of columns in the grid
        /// </summary>
        public int Columns  
        {
            get { return _columns; } 
        }
    }
}
