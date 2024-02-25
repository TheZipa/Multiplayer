using UnityEngine;

namespace MultiplayerGame.Code.Core.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        private readonly int _animIDSpeed = Animator.StringToHash("Speed");
        private readonly int _animIDGrounded = Animator.StringToHash("Grounded");
        private readonly int _animIDJump = Animator.StringToHash("Jump");
        private readonly int _animIDFreeFall = Animator.StringToHash("FreeFall");
        private readonly int _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");

        public void SetSpeedAnimation(float speed) => _animator.SetFloat(_animIDSpeed, speed);

        public void SetMotionSpeedAnimation(float speed) => _animator.SetFloat(_animIDMotionSpeed, speed);

        public void SetGroundedAnimation(bool isGrounded) => _animator.SetBool(_animIDGrounded, isGrounded);
        
        public void SetJumpAnimation(bool isJump) => _animator.SetBool(_animIDJump, isJump);

        public void SetFreeFallAnimation(bool isFreeFall) => _animator.SetBool(_animIDFreeFall, isFreeFall);
    }
}