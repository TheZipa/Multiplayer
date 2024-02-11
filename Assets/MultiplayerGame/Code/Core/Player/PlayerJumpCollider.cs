using UnityEngine;

namespace MultiplayerGame.Code.Core.Player
{
    public class PlayerJumpCollider : MonoBehaviour
    { 
        [HideInInspector] public bool InGrounded = true;
        [SerializeField] private LayerMask _groundLayers;

        private void OnTriggerEnter(Collider other)
        {
            if (_groundLayers.value << other.gameObject.layer == 1) return;
            InGrounded = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (_groundLayers.value << other.gameObject.layer == 1) return;
            InGrounded = false;
        }
    }
}