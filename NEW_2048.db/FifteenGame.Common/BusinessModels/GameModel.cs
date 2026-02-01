using System;

namespace FifteenGame.Common.BusinessModels
{
    public class GameModel
    {
        public const int RowCount = 4;
        public const int ColumnCount = 4;

        public int Score { get; set; }

        private readonly int[,] _cells = new int[RowCount, ColumnCount];

        public int this[int r, int c]
        {
            get => _cells[r, c];
            set => _cells[r, c] = value;
        }

        public void Clear()
        {
            Score = 0;
            Array.Clear(_cells, 0, _cells.Length);
        }

        public int[,] CloneGrid()
        {
            var copy = new int[RowCount, ColumnCount];
            for (int r = 0; r < RowCount; r++)
                for (int c = 0; c < ColumnCount; c++)
                    copy[r, c] = _cells[r, c];
            return copy;
        }

        public void SetGrid(int[,] grid)
        {
            if (grid.GetLength(0) != RowCount || grid.GetLength(1) != ColumnCount)
                throw new ArgumentException("Invalid grid size.");

            for (int r = 0; r < RowCount; r++)
                for (int c = 0; c < ColumnCount; c++)
                    _cells[r, c] = grid[r, c];
        }
    }
}
