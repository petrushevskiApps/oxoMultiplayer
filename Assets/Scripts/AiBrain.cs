using System;
using System.Linq;

public class AiBrain
{
    private const int MAX_DEPTH = 3;

    private int aiSign = 0;
    private int playerSign = 0;
    
    private WinCondition<ITile> winCondition;
    
    public void Initialize(int aiSign, int playerSign, WinCondition<ITile> winCondition)
    {
        this.aiSign = aiSign;
        this.playerSign = playerSign;
        this.winCondition = winCondition;
    }

    public int Evaluate(int[,]tileBoard)
    {
        IntegerTile[,] board = IntegerTile.Parse(tileBoard);
        
        if (board.Cast<IntegerTile>().Any(tile => winCondition.IsRoundWon(aiSign, tile.Id, board)))
        {
            return 10;
        }

        if (board.Cast<IntegerTile>().Any(tile => winCondition.IsRoundWon(playerSign, tile.Id, board)))
        {
            return -10;
        }

        if (winCondition.IsTableFull(board))
        {
            return 0;
        }

        return -1;
    }
    
    public int FindBestMove(ITile[,] tileBoard)
    {
        Result result = Minimax(IntegerTile.Parse(tileBoard), 0, true, int.MinValue, int.MaxValue);
            
        return tileBoard[result.row,result.col].Id;
    }
    
    private Result Minimax(int [,] board, int depth, bool isMax, int alpha, int beta)
    {
        int rows = board.GetUpperBound(0);
        int cols = board.GetUpperBound(1);
        int sign = isMax ? aiSign : playerSign;

        Result bestResult = new Result();
        
        int score = Evaluate(board);
 
        // If Round is completed return score
        if (score != -1 || depth > MAX_DEPTH)
        {
            bestResult.score = score;
            return bestResult;
        }

        bestResult.score = isMax ? int.MinValue : int.MaxValue;
            
        // Traverse all cells
        for (int i = 0; i <= rows; i++)
        {
            for (int j = 0; j <= cols; j++)
            {
                // Check if cell is empty
                if (board[i, j] == 0)
                {
                    // Make the move
                    board[i, j] = sign;

                    int value = Minimax(board, depth + 1, !isMax, alpha, beta).score;
                    
                    if (isMax)
                    {
                        if (value > bestResult.score)
                        {
                            bestResult.score = value;
                            bestResult.row = i;
                            bestResult.col = j;
                        }
                        alpha = Math.Max(alpha, bestResult.score);
                    }
                    else
                    {
                        if (value < bestResult.score)
                        {
                            bestResult.score = value;
                            bestResult.row = i;
                            bestResult.col = j;
                        }
                        beta = Math.Min(beta, bestResult.score);
                    }
                    // Undo the move
                    board[i, j] = 0;
                        
                    if(alpha >= beta) break;
                }
            }
        }

        if (isMax) bestResult.score -= depth;
        else bestResult.score += depth;
        
        return bestResult;
    }

    
}
public class Result
{
    public int score;
    public int row;
    public int col;

    public Result()
    {
        row = -1;
        col = -1;
        score = 0;
    }
}