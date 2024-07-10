using MultiplayerGame.Code.Services.Input;
using UnityEngine;

namespace MultiplayerGame.Code.Core.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Player")]
        public float MoveSpeed = 2.0f;
        public float CrouchSpeed = 1.25f;
        public float SprintSpeed = 5.335f;
        [Range(0.0f, 0.3f)] public float RotationSmoothTime = 0.12f;
        public float SpeedChangeRate = 10.0f;

        private float _speed;
        private float _animationBlend;
        private float _targetRotation;
        private float _rotationVelocity;

        [SerializeField] private CharacterController _controller;
        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private PlayerJump _playerJump;
        private IInputService _inputService;
        private GameObject _mainCamera;

        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
            _playerJump.Construct(inputService);
            _playerJump.enabled = enabled = true;
        }

        private void Awake() => _mainCamera = Camera.main.gameObject;

        private void Update() => Move();

        private void Move()
        { 
            ApplySpeed();
            ApplyRotation();
            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
            
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _playerJump.VerticalVelocity, 0.0f) * Time.deltaTime);
            _playerAnimator.SetSpeedAnimation(_animationBlend);
        }

        private void ApplyRotation()
        {
            if (_inputService.Move == Vector2.zero) return;
            
            Vector3 inputDirection = new Vector3(_inputService.Move.x, 0.0f, _inputService.Move.y).normalized;
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);

            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        private void ApplySpeed()
        {
            bool crouch = ApplyCrouch();
            float targetSpeed = GetTargetSpeed(crouch);
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
            float speedOffset = 0.1f;

            CalculateSpeed(currentHorizontalSpeed, targetSpeed, speedOffset);
            CalculateAnimationBlend(targetSpeed);

            _playerAnimator.SetMotionSpeedAnimation(1);
        }

        private void CalculateSpeed(float currentHorizontalSpeed, float targetSpeed, float speedOffset)
        {
            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * SpeedChangeRate);
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }
        }

        private void CalculateAnimationBlend(float targetSpeed)
        {
            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;
        }

        private float GetTargetSpeed(bool crouch)
        {
            if (_inputService.Move == Vector2.zero) return 0.0f;
            if (crouch) return CrouchSpeed;
            if (_inputService.IsSprint) return SprintSpeed;
            return MoveSpeed;
        }

        private bool ApplyCrouch()
        {
            bool crouch = _inputService.IsCrouch && _playerJump.Grounded;
            _playerAnimator.SetCrouchAnimation(crouch);
            return crouch;
        }
    }
}