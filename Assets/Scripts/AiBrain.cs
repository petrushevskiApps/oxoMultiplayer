using System;
using System.Linq;

public class AiBrain
{
    private const int MAX_DEPTH = 5;

    private int aiSign = 0;
    private int playerSign = 0;
    
    private WinCondition<ITile> winCondition;
    
    public void Initialize(int aiSign, int playerSign, WinCondition<ITile> winCondition)
    {
        this.aiSign = aiSign;
        this.playerSign = playerSign;
        this.winCondition = winCondition;
    }

    private int Evaluate(IntegerTile [,]board)
    {
        if (board.Cast<IntegerTile>().Any(tile => winCondition.IsRoundWon(aiSign, tile.Id, board)))
        {
            return 10;
        }
        else if (board.Cast<IntegerTile>().Any(tile => winCondition.IsRoundWon(playerSign, tile.Id, board)))
        {
            return -10;
        }
        else if (winCondition.IsTableFull(board))
        {
            return 0;
        }
        else
        {
            return -1;
        }
    }
    
    public int FindBestMove(ITile[,] tileBoard)
    {
        int bestVal = int.MinValue;
        int bestMoveId = 0;
        
        IntegerTile[,] board = IntegerTile.Parse(tileBoard);
        
        // Traverse all cells, evaluate minimax function 
        // for all empty cells. And return the cell 
        // with optimal value.
        for (int i = 0; i <= board.GetUpperBound(0); i++)
        {
            for (int j = 0; j <= board.GetUpperBound(1); j++)
            {
                // Check if cell is empty
                if (board[i, j].Value == 0)
                {
                    // Make the move
                    board[i, j].Value = aiSign;
 
                    // compute evaluation function for this
                    // move.
                    int moveVal = Minimax(board, 0, false, int.MinValue, int.MaxValue);
 
                    // Undo the move
                    board[i, j].Value = 0;
 
                    // If the value of the current move is
                    // more than the best value, then update
                    // best/
                    if (moveVal > bestVal)
                    {
                        bestMoveId = board[i, j].Id;
                        bestVal = moveVal;
                    }
                }
            }
        }
 
        return bestMoveId;
    }
    private int Minimax(IntegerTile [,] board, int depth, bool isMax, int alpha, int beta)
    {
        int rows = board.GetUpperBound(0);
        int cols = board.GetUpperBound(1);
        int sign = isMax ? aiSign : playerSign;
        
        int score = Evaluate(board);
 
        // If Round is completed return score
        if (score == 10 || score == -10 || score == 0) return score;

        if (depth > MAX_DEPTH) return score;
        
        int bestScore = isMax ? int.MinValue : int.MaxValue;
            
        // Traverse all cells
        for (int i = 0; i <= rows; i++)
        {
            for (int j = 0; j <= cols; j++)
            {
                // Check if cell is empty
                if (board[i, j].Value == 0)
                {
                    // Make the move
                    board[i, j].Value = sign;

                    int value = Minimax(board, depth + 1, !isMax, alpha, beta);
                    
                    if (isMax)
                    {
                        bestScore = Math.Max(bestScore, value);
                        alpha = Math.Max(alpha, bestScore);
                    }
                    else
                    {
                        bestScore = Math.Min(bestScore, value);
                        beta = Math.Min(bestScore, beta);
                    }
                    // Undo the move
                    board[i, j].Value = 0;
                        
                    if(alpha >= beta) break;
                }
            }
        }
        return isMax ? (bestScore - depth) : (bestScore + depth);
    }

    
}
