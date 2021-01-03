using System.Collections.Generic;
using System.Linq;

public class WinCondition
{
    private int minStrike;
    private List<ExtractArray<int>> extractStrategies = new List<ExtractArray<int>>();
    private List<ElementIndex> arrayIndexes;
    private int winOffset;
    
    
    public WinCondition(int minStrike)
    {
        this.minStrike = minStrike;
        SetupExtractStrategies();
    }
    
    private void SetupExtractStrategies()
    {
        extractStrategies.Add(new ExtractRow<int>());
        extractStrategies.Add(new ExtractColumn<int>());
        extractStrategies.Add(new ExtractDiagonal<int>());
        extractStrategies.Add(new ExtractReverseDiagonal<int>());
    }
    
    public bool IsRoundWon(int playerId, ElementIndex index, int[,] table)
    {
        foreach (ExtractArray<int> strategy in extractStrategies)
        {
            if (IsWin(playerId, strategy.Extract(table, index)))
            {
                arrayIndexes = strategy.GetIndexes();
                return true;
            }
        }

        return false;
    }
    
    public bool IsTableFull(int[,] tilesTable)
    {
        int count = tilesTable.Cast<int>().Count(t => t != 0);
        return count == tilesTable.Length;
    }
    
    public IEnumerable<ElementIndex> GetWinIndexes()
    {
        for (int i = winOffset; i < arrayIndexes.Count; i++)
        {
            yield return arrayIndexes[i];
        }
    }
    
    private bool IsWin(int id, int[] array)
    {
        int winCount = 0;
        int bound = array.Length - minStrike;

        if (bound < 0) return false;
        
        for (int offset = 0; offset <= bound; offset++)
        {
            for (int i = offset; i < offset + minStrike; i++)
            {
                if (array[i] == id)
                {
                    winCount++;
                }
                else
                {
                    winCount = 0;
                    break;
                }
            }

            if (winCount >= minStrike)
            {
                winOffset = offset;
                return true;
            }
            else winCount = 0;
        }

        return winCount >= 3;
    }

}