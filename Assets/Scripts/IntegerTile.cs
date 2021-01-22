public class IntegerTile : ITile
{
    public int Id { get; set; }
    public int Value { get; set; }
        
    public static IntegerTile[,] Parse(ITile[,] tileBoard)
    {
        int rows = tileBoard.GetUpperBound(0) + 1;
        int columns = tileBoard.GetUpperBound(1) + 1;
            
        IntegerTile[,] board = new IntegerTile[rows, columns];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                board[i, j] = new IntegerTile
                {
                    Id = tileBoard[i, j].Id,
                    Value = tileBoard[i, j].Value
                };
            }
        }

        return board;
    }
}