using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    public bool CheckWinCondition(int[,] tilesTable)
    {
        return CheckRows(tilesTable) 
               || CheckColumns(tilesTable) 
               || CheckDiagonal(tilesTable) 
               || CheckReverseDiagonal(tilesTable);
    }
    public bool CheckTie(int[,] tilesTable)
    {
        int count = 0;
        foreach (int t in tilesTable)
        {
            if (t != 0) count++;
        }
        
        return count == tilesTable.Length;
    }
    private bool CheckRows(int[,] tilesTable)
    {
        for (int i = 0; i <= tilesTable.GetUpperBound(0); i++)
        {
            int t1 = tilesTable[i, 0];
            int t2 = tilesTable[i, 1];
            int t3 = tilesTable[i, 2];
            if( t1 == 0 || t2 == 0 || t3 == 0) continue;
            if( t1 == t2  &&  t2 == t3) return true;
        }

        return false;
    }

    private bool CheckColumns(int[,] tilesTable)
    {
        // Compare columns
        for (int i = 0; i <= tilesTable.GetUpperBound(1); i++)
        {
            int t1 = tilesTable[0, i];
            int t2 = tilesTable[1, i];
            int t3 = tilesTable[2, i];
            if( t1 == 0 || t2 == 0 || t3 == 0) continue;
            if( t1 == t2  &&  t2 == t3) return true;
        }

        return false;
    }

    private bool CheckDiagonal(int[,] tilesTable)
    {
        // Compare diagonal elements
        int m1 = tilesTable[0, 0];
        for (int i = 0; i <= tilesTable.GetUpperBound(0); i++)
        {
            if (tilesTable[i, i] != m1 || tilesTable[i, i] == 0) break;
            if (i == tilesTable.GetUpperBound(0))
            {
                if(tilesTable[i, i] == m1) return true;
            }
        }

        return false;
    }
    
    private bool CheckReverseDiagonal(int[,] tilesTable)
    {
        // Compare reverse diagonal elements
        int i = 0;
        int j = tilesTable.GetUpperBound(0);
        bool isEqual = true;
        while (j > 0 && isEqual)
        {
            int currentPosition = tilesTable[i, j];
            if (currentPosition == 0)
            {
                isEqual = false;
                break;
            }
            int nextPosition = tilesTable[++i, --j];
            if (currentPosition != nextPosition) isEqual = false;
        }

        return isEqual;
    }

}
