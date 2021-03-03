using UnityEngine;

public class StatisticsManager : MonoBehaviour
{
    private void Awake()
    {
        MatchController.MatchEnd.AddListener(OnMatchEnded);
    }

    private void OnDestroy()
    {
        MatchController.MatchEnd.RemoveListener(OnMatchEnded);
    }
    private void OnMatchEnded(bool isWin)
    {
        PlayerDataController.IncreasePlayedGames();

        if (isWin)
        {
            PlayerDataController.IncreaseWonGames();
        }
        else
        {
            PlayerDataController.IncreaseLostGames();
        }
    }
}
