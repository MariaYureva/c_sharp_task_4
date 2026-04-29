using System;

namespace ChessLibrary
{
    public class Queen : ChessPiece
    {
        public Queen(string color, int row, int column)
            : base(color, row, column)
        {
        }

        protected override bool CanMoveTo(int newRow, int newColumn)
        {
            bool rookMove = Row == newRow || Column == newColumn;
            bool bishopMove = Math.Abs(newRow - Row) == Math.Abs(newColumn - Column);

            return rookMove || bishopMove;
        }
    }
}