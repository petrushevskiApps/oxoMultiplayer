using System;
using System.Linq;
using com.petrushevskiapps.Oxo;
using com.petrushevskiapps.Oxo.Utilities;
using Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TileView : MonoBehaviour
{
    [SerializeField] private TileImages images;
    [SerializeField] private SpriteRenderer tileStateImage;
    
    private Animator animator;
    private SpriteRenderer tileBackgroundImage;

    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        tileBackgroundImage = GetComponent<SpriteRenderer>();
    }
    
    public void SetView(TilePlayerSign playerSign, TileBackground background)
    {
        tileBackgroundImage.sprite = images.GetTileBackground(background);
        tileStateImage.sprite = images.GetTilePlayerSign(playerSign);
    }
    
    public void ChangeStateView(TilePlayerSign tilePlayerSign)
    {
        tileStateImage.sprite = images.GetTilePlayerSign(tilePlayerSign);
    }

    public void WrongTileClickedEffect()
    {
        //TODO:: Show wrong tile clicked here with effects!
    }
    
    public void StrikeTileEffect(bool isWin, TilePlayerSign playerSign)
    {
        TileBackground background = isWin ? TileBackground.Win : TileBackground.Lose;
        animator.SetTrigger(Animator.StringToHash("Zoom"));
        tileBackgroundImage.sprite = images.GetTileBackground(background);
        tileStateImage.sprite = images.GetEndTileState(playerSign);
    }

}

