using System.Collections.Generic;
using System.Linq;

public class WinCondition
{
    private int minStrike;
    private List<ExtractArray<Tile>> extractStrategies = new List<ExtractArray<Tile>>();
    private List<int> winIds = new List<int>();

    public WinCondition(int minStrike)
    {
        this.minStrike = minStrike;
        SetupExtractStrategies();
    }
    
    private void SetupExtractStrategies()
    {
        extractStrategies.Add(new ExtractRow<Tile>());
        extractStrategies.Add(new ExtractColumn<Tile>());
        extractStrategies.Add(new ExtractDiagonal<Tile>());
        extractStrategies.Add(new ExtractReverseDiagonal<Tile>());
    }
    
    public bool IsRoundWon(int playerId, int elementId, Tile[,] table)
    {
        foreach (ExtractArray<Tile> strategy in extractStrategies)
        {
            if (IsWin(playerId, strategy.Extract(table, elementId)))
            {
                return true;
            }   
        }

        return false;
    }
    
    public bool IsTableFull(Tile[,] tilesTable)
    {
        int count = tilesTable.Cast<Tile>().Count(t => t.Id != 0);
        return count == tilesTable.Length;
    }

    public List<int> GetWinIds()
    {
        return winIds;
    }
    
    private bool IsWin(int id, Tile[] array)
    {
        int winCount = 0;
        int bound = array.Length - minStrike;

        if (bound < 0) return false;
        
        for (int offset = 0; offset <= bound; offset++)
        {
            winIds.Clear();
            
            for (int i = offset; i < offset + minStrike; i++)
            {
                Tile tile = array[i];
                
                if (tile.PlayerId == id)
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

            if (winCount >= minStrike) return true;
            
            winCount = 0;
        }

        return winCount >= 3;
    }

}