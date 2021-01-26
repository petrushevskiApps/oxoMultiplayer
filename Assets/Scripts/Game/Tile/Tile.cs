﻿using System;
using System.Linq;
using com.petrushevskiapps.Oxo;
using com.petrushevskiapps.Oxo.Utilities;
using Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Tile : MonoBehaviour, ITile
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

    private int value;
    public int Value
    {
        get => value;
        set
        {
            if(this.value == value ||  value < 0) return;
            this.value = value;
        }
    }

    public TilePlayerSign TilePlayerSign => (TilePlayerSign) Value;

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
        Value = 0;
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
        if (!NetworkManager.Instance.RoomController.LocalPlayer.IsActive()) return;
        TileClicked();
    }

    public void TileClicked()
    {
        if (TilePlayerSign == TilePlayerSign.Empty)
        {
            ChangeState(NetworkManager.Instance.RoomController.ActivePlayer?.GetId() ?? 0);
            StateChange.Invoke(Id);
        }
        else
        {
            tileView.WrongTileClickedEffect();
        }
    }
    
    public void ChangeState(int playerId)
    {
        if (Value != 0) return;
        Value = playerId;
        tileView.ChangeStateView(TilePlayerSign);
    }
    
    public void EndEffect(bool isWin)
    {
        tileView.StrikeTileEffect(isWin, TilePlayerSign);
    }

}

public interface ITile
{
    int Id { get; set; }
    int Value { get; set; }
}
