using System;
using System.Linq;
using com.petrushevskiapps.Oxo;
using com.petrushevskiapps.Oxo.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TileState : MonoBehaviour
{
    [SerializeField] private TileImages images;
    [SerializeField] private SpriteRenderer tileStateImage;
    
    [HideInInspector] 
    public UnityIntegerEvent TileStateChange = new UnityIntegerEvent();
    
    public int tileId = 0;

    private Animator animator;
    private SpriteRenderer tileBackgroundImage;
    private TileType _tile;
    private Button tileButton;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        tileBackgroundImage = GetComponent<SpriteRenderer>();
       
        SetTile();
    }

    public void SetTile()
    {
        _tile = TileType.Empty;
        
        tileBackgroundImage.sprite = images.GetTileBackground(TileType.Empty);
        tileStateImage.sprite = images.GetTileState(TileType.Empty);
    }
    
    private void OnMouseDown()
    {
        if (NetworkManager.Instance.RoomController.LocalPlayer.IsActive())
        {
            if (_tile == TileType.Empty)
            {
                Debug.Log("TileID: " + tileId);
                ChangeState();
                TileStateChange.Invoke(tileId);
            }
            else
            {
                Debug.Log("Show wrong tile clicked here with effects!");
            }
        }
    }
    
    public void ChangeState()
    {
        if (_tile == TileType.Empty)
        {
            _tile = NetworkManager.Instance.RoomController.ActivePlayer?.PlayerSymbol ?? TileType.Empty;
            tileStateImage.sprite = images.GetTileState(_tile);
        }
    }

    public void EndGameTileEffect(bool isWin)
    {
        animator.SetTrigger(Animator.StringToHash("Zoom"));
        tileBackgroundImage.sprite = images.GetTileBackground(isWin ? TileType.Win : TileType.Lose);
        tileStateImage.sprite = images.GetEndTileState(_tile);
    }

}

public enum TileType
{
    Empty,
    Cross,
    Circle,
    Win,
    Lose,
}