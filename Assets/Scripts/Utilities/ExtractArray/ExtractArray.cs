using System.Collections.Generic;

public abstract class ExtractArray<T>
{
    public abstract T[] Extract(T[,] table, int elementId);
}