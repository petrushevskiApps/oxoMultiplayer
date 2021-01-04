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

    private int playerId;
    public int PlayerId
    {
        get => playerId;
        set
        {
            if(playerId == value ||  value < 0) return;
            playerId = value;
        }
    }

    public TilePlayerSign TilePlayerSign => (TilePlayerSign) PlayerId;

    private BoxCollider2D clickCollider;
    private TileView tileView;
    
    private void Awake()
    {
        clickCollider = GetComponent<BoxCollider2D>();
        tileView = GetComponent<TileView>();
        
        MatchController.RoundStarting.AddListener(OnRoundStarting);
        MatchController.RoundStarted.AddListener(OnRoundStarted);
    }
    private void OnDestroy()
    {
        MatchController.RoundStarting.RemoveListener(OnRoundStarting);
        MatchController.RoundStarted.RemoveListener(OnRoundStarted);
    }
    
    private void Start()
    {
        SetTile();
    }

    public void SetTile()
    {
        PlayerId = 0;
        tileView.SetView(TilePlayerSign, TileBackground.Default);
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
        
        if (TilePlayerSign == TilePlayerSign.Empty)
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
        if (PlayerId != 0) return;
        PlayerId = playerId;
        tileView.ChangeStateView(TilePlayerSign);
    }
    
    public void ShowEndEffect(bool isWin)
    {
        tileView.StrikeTileEffect(isWin, TilePlayerSign);
    }

}

