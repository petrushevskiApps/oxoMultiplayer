using System;
using com.petrushevskiapps.Oxo.Utilities;

public class ExtractReverseDiagonal<T> : ExtractArray<T>
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
        
        if (elementRow + elementColumn < table.GetUpperBound(0))
        {
            //Above Main R-Diagonal
            startRow = 0;
            startColumn = elementColumn + elementRow;
            dLength = startColumn + 1;
        }
        else if (elementColumn + elementRow > table.GetUpperBound(0))
        {
            int columnUpperBound = table.GetUpperBound(1);
            //Below Main R-Diagonal
            startRow = elementRow - (columnUpperBound - elementColumn);
            startColumn = columnUpperBound;
            dLength = (columnUpperBound + 1) - startRow;
        }
        else
        {
            startRow = 0;
            startColumn = table.GetUpperBound(1);
            dLength = table.GetUpperBound(1) + 1;
        }

        T[] diagonal = new T[dLength];

        for (int di=0, r = startRow, c = startColumn; di < dLength; di++, r++, c--)
        {
            diagonal[di] = table[r, c];
        }
        
        Utilities.PrintArray(diagonal);
        return diagonal;
    }

}