namespace ChessLibrary
{
    public interface IChessPiece
    {
        string Color { get; }
        int Row { get; }
        int Column { get; }

        bool MoveTo(int newRow, int newColumn);

        string GetInfo();
    }
}