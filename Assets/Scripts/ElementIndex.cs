using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementIndex
{
    public int Row    { get; private set; }
    public int Column { get; private set; }

    public ElementIndex(int row, int column)
    {
        if (row < 0 || column < 0) throw new ArgumentException();
        
        Row = row;
        Column = column;
    }

    public ElementIndex(int tileId, int rLength, int cLength)
    {
        if (rLength < 0 || cLength < 0) throw new ArgumentException();
        
        Row    = tileId / (rLength + 1);
        Column = tileId % (cLength + 1);
    }

    public int GetTileId(int cLength)
    {
        return (Row * cLength) + Column;
    }
    
    public override string ToString()
    {
        return $"[{Row},{Column}] ";
    }
}
