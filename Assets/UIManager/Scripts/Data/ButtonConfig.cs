using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/ButtonConfig", fileName = "ButtonConfig")]
public class ButtonConfig : ScriptableObject
{
    public AudioClip clickSound;
}
