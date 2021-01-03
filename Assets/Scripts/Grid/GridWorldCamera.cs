using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridWorldCamera : MonoBehaviour
{
    private Camera camera;
    private void Awake()
    {
        Grid.GridCreated.AddListener(OnGridCreated);
        camera = GetComponent<Camera>();
    }

    private void OnDestroy()
    {
        Grid.GridCreated.RemoveListener(OnGridCreated);
    }

    private void OnGridCreated(Grid.GridWorldSize worldSize)
    {
        if (camera != null)
        {
            camera.orthographicSize = worldSize.GetWorldSize().x + 2f;
        }
    }
}
