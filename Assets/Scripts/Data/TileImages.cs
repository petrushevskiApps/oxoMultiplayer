using Data;
using UnityEngine;

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

    public Sprite GetTileBackground(TileBackground playerState)
    {
        switch (playerState)
        {
            case TileBackground.Default : return emptyBackground;
            case TileBackground.Win     : return winBackground;
            case TileBackground.Lose    : return loseBackground;
            default: return emptyBackground;
        }
    }
    public Sprite GetTilePlayerSign(TilePlayerSign playerSign)
    {
        switch (playerSign)
        {
            case TilePlayerSign.Cross : return crossState;
            case TilePlayerSign.Circle: return circleState;
            case TilePlayerSign.Empty : 
            default: return null;
        }
    }

    public Sprite GetEndTileState(TilePlayerSign tile)
    {
        switch (tile)
        {
            case TilePlayerSign.Cross : return endCrossState;
            case TilePlayerSign.Circle: return endCircleState;
            case TilePlayerSign.Empty : 
            default: return null;
        }
    }
}
