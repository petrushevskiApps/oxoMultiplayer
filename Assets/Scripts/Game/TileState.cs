using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TileState : MonoBehaviour
{
    [SerializeField] private TileImages images;
    
    [SerializeField] private SpriteRenderer tileStateImage;
    
    private Animator animator;
    private SpriteRenderer tileBackgroundImage;
    private TileType _tile;
    private Button tileButton;
    public int tileId = 0;

    [HideInInspector] 
    public TileStateChange OnStateChange = new TileStateChange();
    
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
        if (Player.LocalInstance.IsActive)
        {
            if (_tile == TileType.Empty)
            {
                Debug.Log("TileID: " + tileId);
                ChangeState();
                OnStateChange.Invoke(tileId);
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
            _tile = BoardController.LocalInstance.turnController.GetActivePlayer().GetPlayerSymbol();
            tileStateImage.sprite = images.GetTileState(_tile);
        }
    }

    public void EndGameTileEffect(bool isWin)
    {
        animator.SetTrigger(Animator.StringToHash("Zoom"));
        tileBackgroundImage.sprite = images.GetTileBackground(isWin ? TileType.Win : TileType.Lose);
        tileStateImage.sprite = images.GetEndTileState(_tile);
    }
    
    public class TileStateChange : UnityEvent<int>{}

}

public enum TileType
{
    Empty,
    Cross,
    Circle,
    Win,
    Lose,
}