public class IntegerTile : ITile
{
    public int Id { get; set; }
    public int Value { get; set; }
        
//    public static IntegerTile[,] Parse(ITile[,] tileBoard)
//    {
//        int rows = tileBoard.GetUpperBound(0) + 1;
//        int columns = tileBoard.GetUpperBound(1) + 1;
//            
//        IntegerTile[,] board = new IntegerTile[rows, columns];
//
//        for (int i = 0; i < rows; i++)
//        {
//            for (int j = 0; j < columns; j++)
//            {
//                board[i, j] = new IntegerTile
//                {
//                    Id = tileBoard[i, j].Id,
//                    Value = tileBoard[i, j].Value
//                };
//            }
//        }
//
//        return board;
//    }
    public static int[,] Parse(ITile[,] tileBoard)
    {
        int rows = tileBoard.GetUpperBound(0) + 1;
        int columns = tileBoard.GetUpperBound(1) + 1;
            
        int[,] board = new int[rows, columns];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
//                board[i, j] = new IntegerTile
//                {
//                    Id = tileBoard[i, j].Id,
//                    Value = tileBoard[i, j].Value
//                };
                board[i, j] = tileBoard[i, j].Value;
            }
        }

        return board;
    }
    public static IntegerTile[,] Parse(int[,] tileBoard)
    {
        int rows = tileBoard.GetUpperBound(0) + 1;
        int columns = tileBoard.GetUpperBound(1) + 1;
        int id = 0;
        IntegerTile[,] board = new IntegerTile[rows, columns];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
//                board[i, j] = new IntegerTile
//                {
//                    Id = tileBoard[i, j].Id,
//                    Value = tileBoard[i, j].Value
//                };
                board[i, j] = new IntegerTile {Id = id, Value = tileBoard[i, j]};
                id++;
            }
        }

        return board;
    }
}