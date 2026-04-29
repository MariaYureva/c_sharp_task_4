using System;

namespace ChessLibrary
{
    public class Bishop : ChessPiece
    {
        public Bishop(string color, int row, int column)
            : base(color, row, column)
        {
        }

        protected override bool CanMoveTo(int newRow, int newColumn)
        {
            return Math.Abs(newRow - Row) == Math.Abs(newColumn - Column);
        }
    }
}