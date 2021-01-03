using System.Collections.Generic;

public abstract class ExtractArray<T>
{
    protected List<ElementIndex> indexes = new List<ElementIndex>();
    
    public abstract T[] Extract(T[,] table, ElementIndex index);

    
    public List<ElementIndex> GetIndexes()
    {
        return indexes;
    }

}