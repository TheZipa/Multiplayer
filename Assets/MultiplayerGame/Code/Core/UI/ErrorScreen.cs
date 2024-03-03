using MultiplayerGame.Code.Core.UI.Base;
using MultiplayerGame.Code.Services.EntityContainer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MultiplayerGame.Code.Core.UI
{
    public class ErrorScreen : FadeBaseWindow, IFactoryEntity
    {
        [SerializeField] private Button _okButton;
        [SerializeField] private TextMeshProUGUI _errorMessage;

        protected override void OnAwake()
        {
            base.OnAwake();
            _okButton.onClick.AddListener(Hide);
        }

        public void ShowError(string errorMessage)
        {
            _errorMessage.text = errorMessage;
            Show();
        }
    }
}