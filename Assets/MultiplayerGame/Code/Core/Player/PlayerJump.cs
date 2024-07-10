using MultiplayerGame.Code.Services.Input;
using UnityEngine;

namespace MultiplayerGame.Code.Core.Player
{
    public class PlayerJump : MonoBehaviour
    {
        public bool Grounded { get; private set; } = true;
        public float VerticalVelocity { get; private set; }
        
        [Space(10)] 
        [SerializeField] private float _jumpHeight = 1.2f;
        [SerializeField] private float _gravity = -15.0f;
        [Space(10)] 
        [SerializeField] private float _jumpTimeout = 0.50f;
        [SerializeField] private float _fallTimeout = 0.15f;
        [SerializeField] private float _terminalVelocity = 53.0f;
        
        [Space(12)] 
        [SerializeField] private float _roundedOffset = -0.14f;
        [SerializeField] private float _groundedRadius = 0.28f;
        [SerializeField] private LayerMask _groundLayers;

        [Space(12)]
        [SerializeField] private PlayerAnimator _playerAnimator;
        private IInputService _inputService;
        private float _fallTimeoutDelta;
        private float _jumpTimeoutDelta;

        public void Construct(IInputService inputService) => _inputService = inputService;

        private void Start()
        {
            _jumpTimeoutDelta = _jumpTimeout;
            _fallTimeoutDelta = _fallTimeout;
        }

        private void Update()
        {
            ApplyJumpAndGravity();
            GroundedCheck();
        }

        private void ApplyJumpAndGravity()
        {
            if (Grounded)
            {
                Jump();
            }
            else
            {
                _jumpTimeoutDelta = _jumpTimeout;
                if (_fallTimeoutDelta >= 0.0f) _fallTimeoutDelta -= Time.deltaTime;
                else _playerAnimator.SetFreeFallAnimation(true);
            }
            
            if (VerticalVelocity < _terminalVelocity) VerticalVelocity += _gravity * Time.deltaTime;
        }
        
        private void Jump()
        {
            _fallTimeoutDelta = _fallTimeout;
            _playerAnimator.SetJumpAnimation(false);
            _playerAnimator.SetFreeFallAnimation(false);

            if (VerticalVelocity < 0.0f) VerticalVelocity = -2f;

            if (_inputService.IsJump && _jumpTimeoutDelta <= 0.0f)
            {
                VerticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
                _playerAnimator.SetJumpAnimation(true);
            }

            if (_jumpTimeoutDelta >= 0.0f) _jumpTimeoutDelta -= Time.deltaTime;
        }
        
        private void GroundedCheck()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - _roundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, _groundedRadius, _groundLayers,
                QueryTriggerInteraction.Ignore);
            
            _playerAnimator.SetGroundedAnimation(Grounded);
        }
    }
}