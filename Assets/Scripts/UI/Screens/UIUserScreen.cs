using PetrushevskiApps.UIManager;
using TMPro;
using UnityEngine;

namespace com.petrushevskiapps.Oxo
{
    public class UIUserScreen : UIScreen
    {
        [SerializeField] private UIButton usernameBtn;
        [SerializeField] private TextMeshProUGUI usernameText;
        [SerializeField] private TextMeshProUGUI gamesPlayedStat;
        [SerializeField] private TextMeshProUGUI gamesWonStat;
        [SerializeField] private TextMeshProUGUI gamesLostStat;

        private void Awake()
        {
            base.Awake();
            usernameBtn.onClick.AddListener(OnUsernameBtnClicked);
            PlayerDataController.usernameChanged.AddListener(OnUsernameChanged);
        }
        private void OnDestroy()
        {
            usernameBtn.onClick.RemoveListener(OnUsernameBtnClicked);
            PlayerDataController.usernameChanged.RemoveListener(OnUsernameChanged);
        }

        private void OnEnable()
        {
            SetStats();
        }
        
        private void OnUsernameBtnClicked()
        {
            UIManager.Instance.OpenPopup<UIChangeUsernamePopup>();
        }
        private void SetStats()
        {
            usernameText.text = PlayerDataController.Username;
            gamesPlayedStat.text = PlayerDataController.PlayedGames.ToString();
            gamesWonStat.text = PlayerDataController.WonGames.ToString();
            gamesLostStat.text = PlayerDataController.LostGames.ToString();
        }

        private void OnUsernameChanged(string username)
        {
            usernameText.text = username;
        }
    }
}


