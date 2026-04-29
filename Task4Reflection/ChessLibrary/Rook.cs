namespace ChessLibrary
{
    public class Rook : ChessPiece
    {
        public Rook(string color, int row, int column)
            : base(color, row, column)
        {
        }

        protected override bool CanMoveTo(int newRow, int newColumn)
        {
            return Row == newRow || Column == newColumn;
        }
    }
}