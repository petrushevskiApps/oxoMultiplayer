using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TileState : MonoBehaviour
{
    [SerializeField] private StateImages stateImages;
    private TileType _tile;
    private Button tileButton;
    private SpriteRenderer tileImage;
    public int tileId = 0;

    [HideInInspector] 
    public TileStateChange OnStateChange = new TileStateChange();
    private void Awake()
    {
        _tile = TileType.Empty;
        tileImage = GetComponent<SpriteRenderer>();
        tileImage.sprite = stateImages.GetStateImage(TileType.Empty);
    }

    private void OnMouseDown()
    {
        if (Player.LocalInstance.IsActive)
        {
            ChangeState();
            OnStateChange.Invoke(tileId);
        }
    }
    
    public void ChangeState()
    {
        if (_tile == TileType.Empty)
        {
            _tile = BoardController.LocalInstance.turnController.GetActivePlayer().GetPlayerSymbol();
            tileImage.sprite = stateImages.GetStateImage(_tile);
        }
    }

    public class TileStateChange : UnityEvent<int>{}

}

public enum TileType
{
    Empty,
    Cross,
    Circle
}