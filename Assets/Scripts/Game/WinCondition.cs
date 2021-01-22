using System.Collections.Generic;
using System.Linq;

public class WinCondition<T> where T : ITile
{
    public int WinStrike { get; private set; }
    
    private List<ExtractArray<T>> extractStrategies = new List<ExtractArray<T>>();
    private List<int> winIds = new List<int>();

    public WinCondition(int winStrike)
    {
        WinStrike = winStrike;
        SetupExtractStrategies();
    }
    
    public void SetupExtractStrategies()
    {
        extractStrategies.Add(new ExtractRow<T>());
        extractStrategies.Add(new ExtractColumn<T>());
        extractStrategies.Add(new ExtractDiagonal<T>());
        extractStrategies.Add(new ExtractReverseDiagonal<T>());
    }
    
    public bool IsRoundWon(int playerId, int elementId, T[,] table)
    {
        foreach (ExtractArray<T> strategy in extractStrategies)
        {
            if (IsWin(playerId, strategy.Extract(table, elementId)))
            {
                return true;
            }   
        }

        return false;
    }
    
    public bool IsRoundTie(T[,] tilesTable)
    {
        int count = tilesTable.Cast<T>().Count(t => t.Value != 0);
        return count == tilesTable.Length;
    }

    public List<int> GetWinIds()
    {
        return winIds;
    }
    
    private bool IsWin(int id, T[] array)
    {
        int winCount = 0;
        int bound = array.Length - WinStrike;

        if (bound < 0) return false;
        
        for (int offset = 0; offset <= bound; offset++)
        {
            winIds.Clear();
            
            for (int i = offset; i < offset + WinStrike; i++)
            {
                T tile = array[i];
                
                if (tile.Value == id)
                {
                    winCount++;
                    winIds.Add(tile.Id);
                }
                else
                {
                    winCount = 0;
                    break;
                }
            }

            if (winCount >= WinStrike) return true;
            
            winCount = 0;
        }

        return winCount >= 3;
    }

}