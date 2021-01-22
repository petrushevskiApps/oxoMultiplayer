using System;
using System.Linq;

public class AiBrain
{
    private const int MAX_DEPTH = 2;

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
        else if (winCondition.IsRoundTie(board))
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
                    int moveVal = Minimax(board, 0, false);
 
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
    private int Minimax(IntegerTile [,] board, int depth, bool isMax)
    {
        
        int score = Evaluate(board);
 
        // If Round is completed return score
        if (score == 10 || score == -10 || score == 0) return score;

        if (depth > MAX_DEPTH) return score;
        
        // If this maximizer's move
        if (isMax)
        {
            int best = int.MinValue;
 
            // Traverse all cells
            for (int i = 0; i <= board.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= board.GetUpperBound(1); j++)
                {
                    // Check if cell is empty
                    if (board[i, j].Value == 0)
                    {
                        // Make the move
                        board[i, j].Value = aiSign;
 
                        // Call minimax recursively and choose the maximum value
                        best = Math.Max(best, Minimax(board, depth + 1, !isMax));
 
                        // Undo the move
                        board[i, j].Value = 0;
                    }
                }
            }
            return best - depth;
        }
 
        // If this minimizer's move
        else
        {
            int best = int.MaxValue;
 
            // Traverse all cells
            for (int i = 0; i <= board.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= board.GetUpperBound(1); j++)
                {
                    // Check if cell is empty
                    if (board[i, j].Value == 0)
                    {
                        // Make the move
                        board[i, j].Value = playerSign;
 
                        // Call minimax recursively and choose
                        // the minimum value
                        best = Math.Min(best, Minimax(board, depth + 1, !isMax));
 
                        // Undo the move
                        board[i, j].Value = 0;
                    }
                }
            }
            
            return best + depth;
        }
    }

    
}
