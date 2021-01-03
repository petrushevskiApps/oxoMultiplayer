using System;
using com.petrushevskiapps.Oxo.Utilities;

public class ExtractColumn<T> : ExtractArray<T>
{
    public override T[] Extract(T[,] table, ElementIndex index)
    {
        indexes.Clear();
        
        if (index == null)
        {
            throw new ArgumentNullException();
        }

        if (index.Column < table.GetLowerBound(1) || index.Column > table.GetUpperBound(1))
        {
            throw new ArgumentOutOfRangeException();
        }
        
        int rowLength = table.GetUpperBound(0) + 1;
        T[] column = new T[rowLength];

        for (int rowIndex = 0; rowIndex < rowLength; rowIndex++)
        {
            column[rowIndex] = table[rowIndex, index.Column];
            indexes.Add(new ElementIndex(rowIndex, index.Column));
        }
        
        Utilities<T>.PrintArray(column);

        return column;
    }

}