using MultiplayerGame.Code.Services.EntityContainer;
using MultiplayerGame.Code.Services.Input;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace MultiplayerGame.Code.Core.Player
{
    public class Player : MonoBehaviour, IFactoryEntity
    {
        public PhotonView PhotonView;
        public PlayerMovement PlayerMovement;
        public PlayerCamera PlayerCamera;
        [SerializeField] private TMP_Text _nicknameText;

        public void Construct(IInputService inputService, string nickname)
        {
            PlayerMovement.Construct(inputService);
            PlayerCamera.Construct(inputService);
            PlayerCamera.enabled = true;
            _nicknameText.gameObject.SetActive(false);
            PhotonView.RPC("SetNickname", RpcTarget.AllBuffered, nickname);
            inputService.Enable();
        }

        [PunRPC]
        public void SetNickname(string nickname) => _nicknameText.text = nickname;
    }
}