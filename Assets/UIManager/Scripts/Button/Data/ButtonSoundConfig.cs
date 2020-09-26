using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/ButtonSoundConfig", fileName = "ButtonSoundConfig")]
public class ButtonSoundConfig : ScriptableObject
{
    public AudioClip positiveSound;
    public AudioClip negativeSound;
}
