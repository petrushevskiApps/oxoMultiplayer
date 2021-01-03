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
    [HideInInspector] 
    public UnityIntegerEvent StateChange = new UnityIntegerEvent();

    private int id = 0;

    public int Id
    {
        get => id;
        set
        {
            if (value >= 0) id = value;
        }
    }
    
    private TilePlayerSign tilePlayerSign;
    private BoxCollider2D clickCollider;
    private TileView tileView;
    
    private void Awake()
    {
        clickCollider = GetComponent<BoxCollider2D>();
        tileView = GetComponent<TileView>();
        
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
        tilePlayerSign = TilePlayerSign.Empty;
        tileView.SetView(tilePlayerSign, TileBackground.Default);
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
        
        if (tilePlayerSign == TilePlayerSign.Empty)
        {
            ChangeState(NetworkManager.Instance.RoomController.ActivePlayer?.PlayerId ?? 0);
            StateChange.Invoke(Id);
        }
        else
        {
            tileView.WrongTileClickedEffect();
        }
    }
    
    public void ChangeState(int playerId)
    {
        if (tilePlayerSign != TilePlayerSign.Empty) return;
        tilePlayerSign = (TilePlayerSign) playerId;
        tileView.ChangeStateView(tilePlayerSign);
    }
    
    public void ShowStrikeEffect(bool isWin)
    {
        tileView.StrikeTileEffect(isWin, tilePlayerSign);
    }

}

