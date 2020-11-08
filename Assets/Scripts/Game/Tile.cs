using System;
using System.Linq;
using com.petrushevskiapps.Oxo;
using com.petrushevskiapps.Oxo.Utilities;
using Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [SerializeField] private TileImages images;
    [SerializeField] private SpriteRenderer tileStateImage;
    
    [HideInInspector] 
    public UnityIntegerEvent TileStateChange = new UnityIntegerEvent();
    
    public int tileId = 0;

    private Animator animator;
    private SpriteRenderer tileBackgroundImage;
    private TileType _tile;
    
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
        if (RoomController.Instance.LocalPlayer.IsActive)
        {
            if (_tile == TileType.Empty)
            {
                ChangeState(RoomController.Instance.ActivePlayer?.PlayerId ?? 0);
                TileStateChange.Invoke(tileId);
            }
            else
            {
                //TODO:: "Show wrong tile clicked here with effects!"
            }
        }
    }
    
    public void ChangeState(int playerId)
    {
        if (_tile == TileType.Empty)
        {
            _tile = (TileType) playerId;
            tileStateImage.sprite = images.GetTileState(_tile);
        }
    }

    public void StrikeTileEffect(bool isWin)
    {
        animator.SetTrigger(Animator.StringToHash("Zoom"));
        tileBackgroundImage.sprite = images.GetTileBackground(isWin ? TileType.Win : TileType.Lose);
        tileStateImage.sprite = images.GetEndTileState(_tile);
    }

}

