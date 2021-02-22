using System;
using com.petrushevskiapps.Oxo.Utilities;
using Data;
using UnityEngine.Events;

public interface IPlayer
{
    bool IsActive();
    TilePlayerSign Sign { get; }

    string Nickname { get; }

    int GetId();
    int GetScore();
    void IncrementScore();
    void SetScore(int score);

    void RegisterScoreListener(UnityAction<int> listener);
    void UnregisterScoreListener(UnityAction<int> listener);
}