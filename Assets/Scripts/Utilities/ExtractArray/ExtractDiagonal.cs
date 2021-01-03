using System;
using com.petrushevskiapps.Oxo.Utilities;

public class ExtractDiagonal<T> : ExtractArray<T>
{
    public override T[] Extract(T[,] table, ElementIndex index)
    {
        indexes.Clear();
        
        if (index == null) throw new ArgumentNullException();

        if (index.Row < 0 || index.Row > table.GetUpperBound(0))
        {
            throw new ArgumentOutOfRangeException();
        }

        if (index.Column < 0 || index.Column > table.GetUpperBound(1))
        {
            throw new ArgumentOutOfRangeException();
        }
        
        int length = table.GetUpperBound(0) + 1;
        int dLength = 0;
        
        int startRow = 0;
        int startColumn = 0;
        
        if (index.Row > index.Column)
        {
            //Below Main Diagonal
            startRow = index.Row -  index.Column;
            startColumn = 0;
            dLength = length - startRow;
        }
        else if (index.Row < index.Column)
        {
            //Above Main Diagonal
            startRow = 0;
            startColumn = index.Column - index.Row;
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
            indexes.Add(new ElementIndex(r, c));
        }

        Utilities<T>.PrintArray(diagonal);
        return diagonal;
    }

}