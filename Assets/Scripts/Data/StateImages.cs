using UnityEngine;

[CreateAssetMenu(fileName = "TileStates", menuName = "Data/TileStates", order = 1)]
public class StateImages : ScriptableObject
{
    [SerializeField] private  Sprite emptyState;
    [SerializeField] private  Sprite crossState;
    [SerializeField] private  Sprite circleState;

    public Sprite GetStateImage(TileType state)
    {
        switch (state)
        {
            case TileType.Empty : return emptyState;
            case TileType.Cross : return crossState;
            case TileType.Circle: return circleState;
            default: return emptyState;
        }
    }
}
