using System;
using com.petrushevskiapps.Oxo.Utilities;

public class ExtractColumn<T> : ExtractArray<T>
{
    public override T[] Extract(T[,] table, int elementId)
    {
        if (elementId < 0 || elementId > table.Length - 1) throw new ArgumentOutOfRangeException();

        int rowLength = table.GetUpperBound(0) + 1;
        T[] column = new T[rowLength];

        for (int rowIndex = 0; rowIndex < rowLength; rowIndex++)
        {
            column[rowIndex] = table[rowIndex, Utilities.GetColumnFromId(elementId, table)];
        }
        
//        Utilities.PrintArray(column);

        return column;
    }

}