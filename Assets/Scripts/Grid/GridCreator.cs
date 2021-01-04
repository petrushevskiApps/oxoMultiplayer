using System;
using UnityEngine;
using UnityEngine.Events;

namespace Grid
{
    public class GridCreator : MonoBehaviour
    {
        private const int LEFT = -1;
        private const int TOP = 1;

        public static UnityGridWorldEvent GridCreated = new UnityGridWorldEvent();

        [SerializeField] private GameObject element;

        public GridWorldSize worldSize;

        private Vector3 extents;
        private Vector3 size;

        public T[,] CreateGrid<T>(int rows, int columns)
        {
            worldSize = new GridWorldSize();
            T[,] gridElements = new T[rows,columns];
            
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
                    gridElements[i, j] = CreateElementAt<T>(x, y);
                }
            }

            GridCreated.Invoke(worldSize);
            return gridElements;
        }

        private T CreateElementAt<T>(float x, float y)
        {
            Vector3 position = new Vector3(x, y, 0);
            GameObject go = Instantiate(element, position, Quaternion.identity, transform);
            return go.GetComponent<T>();
        }

        private float GetStartPosition(int totalElements, float extent, int side)
        {
            float result = 0;

            int elements = (int) Math.Ceiling(totalElements / 2f);

            float halfExtends = totalElements % 2 + 1;
            result += halfExtends * extent;

            float fullExtents = elements - halfExtends;
            result += fullExtents * (extent * 2);

            Debug.Log($"Position X: {(result)} - Half elements: {elements}");
            return result * side;
        }

        public class UnityGridWorldEvent : UnityEvent<GridWorldSize>
        {

        }
    }
}