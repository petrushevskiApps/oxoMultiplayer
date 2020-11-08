using Data;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "TileStates", menuName = "Data/TileStates", order = 1)]
public class TileImages : ScriptableObject
{
    [Header("Tile Background")]
    [SerializeField] private  Sprite emptyBackground;
    [SerializeField] private  Sprite winBackground;
    [SerializeField] private  Sprite loseBackground;

    [Header("Tile State")]
    
    [SerializeField] private  Sprite crossState;
    [SerializeField] private  Sprite circleState;
    [SerializeField] private  Sprite endCrossState;
    [SerializeField] private  Sprite endCircleState;

    public Sprite GetTileBackground(TileType state)
    {
        switch (state)
        {
            case TileType.Empty : return emptyBackground;
            case TileType.Win   : return winBackground;
            case TileType.Lose   : return loseBackground;
            default: return emptyBackground;
        }
    }
    public Sprite GetTileState(TileType state)
    {
        switch (state)
        {
            case TileType.Cross : return crossState;
            case TileType.Circle: return circleState;
            default: return null;
        }
    }

    public Sprite GetEndTileState(TileType tile)
    {
        switch (tile)
        {
            case TileType.Cross : return endCrossState;
            case TileType.Circle: return endCircleState;
            default: return null;
        }
    }
}
