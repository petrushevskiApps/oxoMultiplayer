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
    private TileType tile;
    private BoxCollider2D clickCollider;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        tileBackgroundImage = GetComponent<SpriteRenderer>();
        clickCollider = GetComponent<BoxCollider2D>();
        
        MatchController.RoundStarting.AddListener(OnRoundStarting);
        MatchController.RoundStarted.AddListener(OnRoundStarted);
        
        SetTile();
    }

    private void OnDestroy()
    {
        MatchController.RoundStarting.RemoveListener(OnRoundStarting);
        MatchController.RoundStarted.RemoveListener(OnRoundStarted);
    }

    public void SetTile()
    {
        tile = TileType.Empty;
        
        tileBackgroundImage.sprite = images.GetTileBackground(TileType.Empty);
        tileStateImage.sprite = images.GetTileState(TileType.Empty);
    }
    
    private void OnRoundStarting(int arg0)
    {
        clickCollider.enabled = false;
    }
    private void OnRoundStarted()
    {
        clickCollider.enabled = true;
    }
    
    private void OnMouseDown()
    {
        if (!NetworkManager.Instance.RoomController.LocalPlayer.IsActive) return;
        
        if (tile == TileType.Empty)
        {
            ChangeState(NetworkManager.Instance.RoomController.ActivePlayer?.PlayerId ?? 0);
            TileStateChange.Invoke(tileId);
        }
        else
        {
            //TODO:: "Show wrong tile clicked here with effects!"
        }
    }
    
    public void ChangeState(int playerId)
    {
        if (tile != TileType.Empty) return;
        tile = (TileType) playerId;
        tileStateImage.sprite = images.GetTileState(tile);
    }

    public void StrikeTileEffect(bool isWin)
    {
        animator.SetTrigger(Animator.StringToHash("Zoom"));
        tileBackgroundImage.sprite = images.GetTileBackground(isWin ? TileType.Win : TileType.Lose);
        tileStateImage.sprite = images.GetEndTileState(tile);
    }

}

