using MultiplayerGame.Code.Services.Input;
using UnityEngine;

namespace MultiplayerGame.Code.Core.Player
{
    public class ThirdPersonPlayerMovement : MonoBehaviour
    {
        [Header("Player")]
        public float MoveSpeed = 2.0f;
        public float SprintSpeed = 5.335f;
        [Range(0.0f, 0.3f)] public float RotationSmoothTime = 0.12f;
        public float SpeedChangeRate = 10.0f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)] public float JumpHeight = 1.2f;
        public float Gravity = -15.0f;
        [Space(10)] public float JumpTimeout = 0.50f;
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        public bool Grounded = true;
        public float GroundedOffset = -0.14f;
        public float GroundedRadius = 0.28f;
        public LayerMask GroundLayers;

        private float _speed;
        private float _animationBlend;
        private float _targetRotation;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;
        
        [SerializeField] private CharacterController _controller;
        [SerializeField] private PlayerAnimator _playerAnimator;
        private IInputService _inputService;
        private GameObject _mainCamera;

        public void Construct(IInputService inputService) => _inputService = inputService;

        private void Awake()
        {
            _mainCamera = Camera.main.gameObject;
        }

        private void Start()
        {
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }

        private void Update()
        {
            ApplyJumpAndGravity();
            GroundedCheck();
            Move();
        }
        
        private void GroundedCheck()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);
            
            _playerAnimator.SetGroundedAnimation(Grounded);
        }
        
        private void Move()
        { 
            ApplySpeed();
            ApplyRotation();
            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
            
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
            _playerAnimator.SetSpeedAnimation(_animationBlend);
        }

        private void ApplyRotation()
        {
            Vector3 inputDirection = new Vector3(_inputService.Move.x, 0.0f, _inputService.Move.y).normalized;

            if (_inputService.Move == Vector2.zero) return;
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              _mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y,
                _targetRotation, ref _rotationVelocity, RotationSmoothTime);

            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        private void ApplySpeed()
        {
            float targetSpeed = _inputService.IsSprint ? SprintSpeed : MoveSpeed;
            if (_inputService.Move == Vector2.zero) targetSpeed = 0.0f;

            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
            float speedOffset = 0.1f;

            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed,
                    Time.deltaTime * SpeedChangeRate);

                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;
            
            _playerAnimator.SetMotionSpeedAnimation(1);
        }

        private void ApplyJumpAndGravity()
        {
            if (Grounded)
            {
                Jump();
            }
            else
            {
                _jumpTimeoutDelta = JumpTimeout;
                if (_fallTimeoutDelta >= 0.0f) _fallTimeoutDelta -= Time.deltaTime;
                else _playerAnimator.SetFreeFallAnimation(true);
                //_input.IsJump = false;
            }
            
            if (_verticalVelocity < _terminalVelocity) _verticalVelocity += Gravity * Time.deltaTime;
        }

        private void Jump()
        {
            _fallTimeoutDelta = FallTimeout;
            _playerAnimator.SetJumpAnimation(false);
            _playerAnimator.SetFreeFallAnimation(false);

            if (_verticalVelocity < 0.0f) _verticalVelocity = -2f;

            if (_inputService.IsJump && _jumpTimeoutDelta <= 0.0f)
            {
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                _playerAnimator.SetJumpAnimation(true);
            }

            if (_jumpTimeoutDelta >= 0.0f) _jumpTimeoutDelta -= Time.deltaTime;
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (!(animationEvent.animatorClipInfo.weight > 0.5f)) return;
            if (FootstepAudioClips.Length <= 0) return;
            int index = Random.Range(0, FootstepAudioClips.Length);
            AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center),
                    FootstepAudioVolume);
        }
    }
}