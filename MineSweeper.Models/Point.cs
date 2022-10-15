namespace MineSweeper.Models
{
    public class Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Coordinate along the x-axis
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// coordinate along the y-axis
        /// </summary>
        public int Y { get; set; }
    }
}