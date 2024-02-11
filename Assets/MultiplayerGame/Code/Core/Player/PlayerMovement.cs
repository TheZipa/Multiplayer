using MultiplayerGame.Code.Services.Input;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _groundDrag;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpCooldown;
    [SerializeField] private float _airMultiplier;
    [Space]
    [SerializeField] private LayerMask _groundLayers;

    private IInputService _inputService;
    private Rigidbody _rigidbody;
    private Transform _orientation;
    private Vector3 _moveDirection;
    private Vector2 _moveInput;
    private float _walkSpeed;
    private float _sprintSpeed;
    private bool _grounded;
    private bool _readyToJump;

    public void Construct(IInputService inputService, Transform orientation, Rigidbody rigidbody)
    {
        _orientation = orientation;
        _rigidbody = rigidbody;
        _inputService = inputService;
        _inputService.OnJump += TryJump;
    }
    
    private void Start()
    {
        _rigidbody.freezeRotation = true;
        _readyToJump = true;
    }

    private void Update()
    {
        if (_inputService is null) return;
        _moveInput = _inputService.MovementAxes;
        SpeedControl();
        _rigidbody.drag = _grounded ? _groundDrag : 0;
    }

    private void DefinePlayerGrounded() => _grounded = Physics.Raycast(transform.position, 
        -transform.up * 1.3f, _groundLayers);

    private void FixedUpdate()
    {
        DefinePlayerGrounded();
        if (_moveInput == Vector2.zero) return;
        MovePlayer();
    }

    private void TryJump()
    {
        if (!_readyToJump || !_grounded) return;
        _readyToJump = false;
        Jump();
        Invoke(nameof(ResetJump), _jumpCooldown);
    }

    private void MovePlayer()
    {
        _moveDirection = _orientation.forward * _moveInput.y + _orientation.right * _moveInput.x;
        float speed = _moveSpeed * 10f;
        if (_grounded) speed *= _airMultiplier;
        _rigidbody.AddForce(_moveDirection.normalized * speed, ForceMode.VelocityChange);
    }

    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);
        
        if(flatVelocity.magnitude > _moveSpeed)
        {
            Vector3 clampedVelocity = flatVelocity.normalized * _moveSpeed;
            _rigidbody.velocity = new Vector3(clampedVelocity.x, _rigidbody.velocity.y, clampedVelocity.z);
        }
    }

    private void Jump()
    {
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);
        _rigidbody.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }
    
    private void ResetJump() => _readyToJump = true;
}