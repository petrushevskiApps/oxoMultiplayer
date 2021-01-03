using System;
using com.petrushevskiapps.Oxo.Utilities;

public class ExtractRow<T> : ExtractArray<T>
{
    public override T[] Extract(T[,] table, ElementIndex index)
    {
        indexes.Clear();

        if (index == null)
        {
            throw new ArgumentNullException();
        }

        if (index.Row < table.GetLowerBound(0) || index.Row > table.GetUpperBound(0))
        {
            throw new ArgumentOutOfRangeException();
        }
        
        int columnLength = table.GetUpperBound(1) + 1;
        T[] row = new T[columnLength];

        for (int columnIndex = 0; columnIndex < columnLength; columnIndex++)
        {
            row[columnIndex] = table[index.Row, columnIndex];
            indexes.Add(new ElementIndex(index.Row, columnIndex));
        }
        
        Utilities<T>.PrintArray(row);
        
        return row;
    }
}