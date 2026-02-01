using FifteenGame.Common.BusinessModels;
using FifteenGame.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FifteenGame.Business.Services
{
    public class GameService : IGameService
    {
        private static readonly Random _rnd = new Random();

        public void Initialize(GameModel model)
        {
            model.Clear();
            AddRandomTile(model);
            AddRandomTile(model);
        }

        public void Shuffle(GameModel model) => Initialize(model);

        public bool MakeMove(GameModel model, MoveDirection direction)
        {
            if (direction == MoveDirection.None)
                return false;

            bool moved = false;

            switch (direction)
            {
                case MoveDirection.Left: moved = MoveLeft(model); break;
                case MoveDirection.Right: moved = MoveRight(model); break;
                case MoveDirection.Up: moved = MoveUp(model); break;
                case MoveDirection.Down: moved = MoveDown(model); break;
            }

            if (moved)
                AddRandomTile(model);

            return moved;
        }

        public bool Has2048(GameModel model)
        {
            for (int r = 0; r < GameModel.RowCount; r++)
                for (int c = 0; c < GameModel.ColumnCount; c++)
                    if (model[r, c] == 2048) return true;
            return false;
        }

        public bool HasAnyMoves(GameModel model)
        {
            for (int r = 0; r < GameModel.RowCount; r++)
                for (int c = 0; c < GameModel.ColumnCount; c++)
                    if (model[r, c] == 0) return true;

            for (int r = 0; r < GameModel.RowCount; r++)
                for (int c = 0; c < GameModel.ColumnCount; c++)
                {
                    int v = model[r, c];
                    if (r + 1 < GameModel.RowCount && model[r + 1, c] == v) return true;
                    if (c + 1 < GameModel.ColumnCount && model[r, c + 1] == v) return true;
                }

            return false;
        }

        private void AddRandomTile(GameModel model)
        {
            var empties = new List<(int r, int c)>();
            for (int r = 0; r < GameModel.RowCount; r++)
                for (int c = 0; c < GameModel.ColumnCount; c++)
                    if (model[r, c] == 0) empties.Add((r, c));

            if (empties.Count == 0) return;

            var (rr, cc) = empties[_rnd.Next(empties.Count)];

            model[rr, cc] = _rnd.Next(10) == 0 ? 4 : 2;
        }

        private bool MoveLeft(GameModel model)
        {
            bool moved = false;
            for (int r = 0; r < GameModel.RowCount; r++)
            {
                var line = Enumerable.Range(0, GameModel.ColumnCount).Select(c => model[r, c]).ToArray();
                var (newLine, gained, changed) = ProcessLine(line);
                if (changed)
                {
                    moved = true;
                    model.Score += gained;
                    for (int c = 0; c < GameModel.ColumnCount; c++)
                        model[r, c] = newLine[c];
                }
            }
            return moved;
        }

        private bool MoveRight(GameModel model)
        {
            bool moved = false;
            for (int r = 0; r < GameModel.RowCount; r++)
            {
                var line = Enumerable.Range(0, GameModel.ColumnCount).Select(c => model[r, GameModel.ColumnCount - 1 - c]).ToArray();
                var (newLine, gained, changed) = ProcessLine(line);
                if (changed)
                {
                    moved = true;
                    model.Score += gained;
                    for (int c = 0; c < GameModel.ColumnCount; c++)
                        model[r, GameModel.ColumnCount - 1 - c] = newLine[c];
                }
            }
            return moved;
        }

        private bool MoveUp(GameModel model)
        {
            bool moved = false;
            for (int c = 0; c < GameModel.ColumnCount; c++)
            {
                var line = Enumerable.Range(0, GameModel.RowCount).Select(r => model[r, c]).ToArray();
                var (newLine, gained, changed) = ProcessLine(line);
                if (changed)
                {
                    moved = true;
                    model.Score += gained;
                    for (int r = 0; r < GameModel.RowCount; r++)
                        model[r, c] = newLine[r];
                }
            }
            return moved;
        }

        private bool MoveDown(GameModel model)
        {
            bool moved = false;
            for (int c = 0; c < GameModel.ColumnCount; c++)
            {
                var line = Enumerable.Range(0, GameModel.RowCount).Select(r => model[GameModel.RowCount - 1 - r, c]).ToArray();
                var (newLine, gained, changed) = ProcessLine(line);
                if (changed)
                {
                    moved = true;
                    model.Score += gained;
                    for (int r = 0; r < GameModel.RowCount; r++)
                        model[GameModel.RowCount - 1 - r, c] = newLine[r];
                }
            }
            return moved;
        }

        private (int[] newLine, int gainedScore, bool changed) ProcessLine(int[] line)
        {
            var original = (int[])line.Clone();

            var compact = line.Where(x => x != 0).ToList();

            int gained = 0;
            for (int i = 0; i < compact.Count - 1; i++)
            {
                if (compact[i] != 0 && compact[i] == compact[i + 1])
                {
                    compact[i] *= 2;
                    gained += compact[i];
                    compact[i + 1] = 0;
                    i++;
                }
            }

            var result = compact.Where(x => x != 0).ToList();
            while (result.Count < GameModel.ColumnCount)
                result.Add(0);

            bool changed = !result.SequenceEqual(original);
            return (result.ToArray(), gained, changed);
        }
    }
}
