using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public partial class Grid : MonoBehaviour
{
    private const int LEFT = -1;
    private const int TOP = 1;
 
    public static UnityGridWorldEvent GridCreated = new UnityGridWorldEvent();
    
    [SerializeField] private int rows = 0;
    [SerializeField] private int columns = 0;
    [SerializeField] private GameObject element;

    public GridWorldSize worldSize;
    private List<GameObject> gridElements = new List<GameObject>();
    private Vector3 extents;
    private Vector3 size;
    
    public void CreateGrid(int rows, int columns)
    {
        this.rows = rows;
        this.columns = columns;
        worldSize = new GridWorldSize();
        
        extents = element.GetComponent<SpriteRenderer>().sprite.bounds.extents;
        size = element.GetComponent<SpriteRenderer>().sprite.bounds.size;
        
        float xPosition = GetStartPosition(rows, extents.x, LEFT);
        float yPosition = GetStartPosition(columns, extents.y, TOP);
        
        worldSize.SetXPosition(xPosition);
        worldSize.SetYPosition(yPosition);
        
        for (int i = 0; i < rows; i++)
        {
            float y = yPosition - (i * size.y);
            worldSize.SetYPosition(y);
            
            for (int j = 0; j < columns; j++)
            {
                float x = xPosition + (j * size.x);
                worldSize.SetYPosition(x);
                AddTile(x, y);
            }
        }
        
        GridCreated.Invoke(worldSize);
    }

    private void AddTile(float x, float y)
    {
        Vector3 position = new Vector3(x, y, 0);
        gridElements.Add(Instantiate(element, position, Quaternion.identity, transform));
    }
    
    private float GetStartPosition(int totalElements, float extent, int side)
    {
        float result = 0;
        
        int elements =  (int) Math.Ceiling(totalElements / 2f);

        float halfExtends = totalElements % 2 + 1;
        result += halfExtends * extent;
        
        float fullExtents = elements - halfExtends;
        result += fullExtents * (extent * 2);

        Debug.Log($"Position X: {(result)} - Half elements: {elements}");
        return result * side;
    }

    public void ForEachOfComponent<T>(Action<T> action)
    {
        gridElements.ForEach(x => action?.Invoke(x.GetComponent<T>()));
    }

    public T ElementAt<T>(int index)
    {
        return gridElements[index].GetComponent<T>();
    }

    public class UnityGridWorldEvent : UnityEvent<GridWorldSize>
    {
        
    }
}
