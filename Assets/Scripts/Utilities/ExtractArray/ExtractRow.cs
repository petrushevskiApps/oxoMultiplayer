using System;
using com.petrushevskiapps.Oxo.Utilities;

public class ExtractRow<T> : ExtractArray<T>
{
    public override T[] Extract(T[,] table, int elementId)
    {
        if (elementId < 0 || elementId > table.Length - 1) throw new ArgumentOutOfRangeException();

        int columnLength = table.GetUpperBound(1) + 1;
        T[] row = new T[columnLength];

        for (int columnIndex = 0; columnIndex < columnLength; columnIndex++)
        {
            row[columnIndex] = table[Utilities.GetRowFromId(elementId, table), columnIndex];
        }
        
//        Utilities.PrintArray(row);
        
        return row;
    }
}