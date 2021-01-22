using System;
using com.petrushevskiapps.Oxo.Utilities;

public class ExtractDiagonal<T> : ExtractArray<T>
{
    public override T[] Extract(T[,] table, int elementId)
    {
        if (elementId < 0 || elementId > table.Length - 1) throw new ArgumentOutOfRangeException();

        int length = table.GetUpperBound(0) + 1;
        int dLength = 0;
        
        int startRow = 0;
        int startColumn = 0;

        int elementRow = Utilities.GetRowFromId(elementId, table);
        int elementColumn = Utilities.GetColumnFromId(elementId, table);
        
        if (elementRow > elementColumn)
        {
            //Below Main Diagonal
            startRow = elementRow - elementColumn;
            startColumn = 0;
            dLength = length - startRow;
        }
        else if (elementRow < elementColumn)
        {
            //Above Main Diagonal
            startRow = 0;
            startColumn = elementColumn - elementRow;
            dLength = length - startColumn;
        }
        else
        {
            // Main Diagonal
            dLength = length;
        }
        
        T[] diagonal = new T[dLength];

        for (int di=0, r = startRow, c = startColumn; di < dLength; di++, r++, c++)
        {
            diagonal[di] = table[r, c];
        }

//        Utilities.PrintArray(diagonal);
        return diagonal;
    }

}