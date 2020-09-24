using UnityEngine;

public class UIHelpButton : UIAnimatedButton
{
    protected override void Awake()
    {
        base.Awake();
        onClick.AddListener(ClickedUp);
    }

    private void ClickedDown()
    {
        Debug.Log("Button Clicked Down");
    }

    private void ClickedUp()
    {
        Debug.Log("Button Clicked Up");
    }
}
