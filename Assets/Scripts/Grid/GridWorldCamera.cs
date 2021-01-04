using UnityEngine;

namespace Grid
{
    public class GridWorldCamera : MonoBehaviour
    {
        private Camera camera;
        private void Awake()
        {
            GridCreator.GridCreated.AddListener(OnGridCreated);
            camera = GetComponent<Camera>();
        }

        private void OnDestroy()
        {
            GridCreator.GridCreated.RemoveListener(OnGridCreated);
        }

        private void OnGridCreated(GridWorldSize worldSize)
        {
            if (camera != null)
            {
                camera.orthographicSize = worldSize.GetWorldSize().x + 2f;
            }
        }
    }

}

