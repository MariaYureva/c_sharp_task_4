using System;

namespace ChessLibrary
{
    public abstract class ChessPiece : IChessPiece
    {
        public string Color { get; }

        public int Row { get; protected set; }

        public int Column { get; protected set; }

        protected ChessPiece(string color, int row, int column)
        {
            Color = color;
            SetPosition(row, column);
        }

        public bool MoveTo(int newRow, int newColumn)
        {
            if (!IsInsideBoard(newRow, newColumn))
            {
                return false;
            }

            if (Row == newRow && Column == newColumn)
            {
                return false;
            }

            if (!CanMoveTo(newRow, newColumn))
            {
                return false;
            }

            Row = newRow;
            Column = newColumn;
            return true;
        }

        public virtual string GetInfo()
        {
            return $"{GetType().Name}: цвет = {Color}, позиция = ({Row}, {Column})";
        }

        protected abstract bool CanMoveTo(int newRow, int newColumn);

        protected bool IsInsideBoard(int row, int column)
        {
            return row >= 1 && row <= 8 && column >= 1 && column <= 8;
        }

        private void SetPosition(int row, int column)
        {
            if (!IsInsideBoard(row, column))
            {
                throw new ArgumentOutOfRangeException(nameof(row), "Координаты должны быть от 1 до 8.");
            }

            Row = row;
            Column = column;
        }
    }
}