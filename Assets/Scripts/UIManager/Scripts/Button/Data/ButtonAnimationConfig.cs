using UnityEngine;

[CreateAssetMenu(menuName = "Data/ButtonAnimationConfig", fileName = "ButtonAnimationConfig")]
public class ButtonAnimationConfig : ScriptableObject
{
    public Vector3 scale = new Vector3(0.85f, 0.85f, 0.85f);
    public float scaleSpeed = 1f;
}
