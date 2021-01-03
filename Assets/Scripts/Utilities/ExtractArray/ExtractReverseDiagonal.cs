using System;
using com.petrushevskiapps.Oxo.Utilities;

public class ExtractReverseDiagonal<T> : ExtractArray<T>
{
    public override T[] Extract(T[,] table, ElementIndex index)
    {
        indexes.Clear();
        
        int length = table.GetUpperBound(0) + 1;
        int dLength = 0;
        
        int startRow = 0;
        int startColumn = 0;
        
        if (index == null) throw new ArgumentNullException();

        if (index.Row < 0 || index.Row > table.GetUpperBound(0))
        {
            throw new ArgumentOutOfRangeException();
        }

        if (index.Column < 0 || index.Column > table.GetUpperBound(1))
        {
            throw new ArgumentOutOfRangeException();
        }
        
        if (index.Row + index.Column < table.GetUpperBound(0))
        {
            //Above Main R-Diagonal
            startRow = 0;
            startColumn = index.Column + index.Row;
            dLength = startColumn + 1;
        }
        else if (index.Column + index.Row > table.GetUpperBound(0))
        {
            int columnUpperBound = table.GetUpperBound(1);
            //Below Main R-Diagonal
            startRow = index.Row - (columnUpperBound - index.Column);
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
            indexes.Add(new ElementIndex(r, c));
        }
        
        Utilities<T>.PrintArray(diagonal);
        return diagonal;
    }

}