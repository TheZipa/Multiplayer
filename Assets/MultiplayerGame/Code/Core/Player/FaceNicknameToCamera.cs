using UnityEngine;

namespace MultiplayerGame.Code.Core.Player
{
    public class FaceNicknameToCamera : MonoBehaviour
    {
        private Camera _mainCamera;
        
        private void Start() => _mainCamera = Camera.main;

        private void Update() => transform.LookAt(_mainCamera.transform);
    }
}