using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action OnPlayerJumped;
    public event Action<EPlayerState> OnPlayerStateChanged;

    [Header("References")]
    [SerializeField] private Transform _orientation;

    private StateController _stateController;
    private Rigidbody _rigidbody;
    

    [Header("Movement Settings")]
    [SerializeField] private KeyCode _movementKey;
    [SerializeField] private float _movementSpeed;

    private float _horizontalInput, _verticalInput;
    private Vector3 _movementDirection;
    private float _defaultMovementSpeed;


    [Header("Jump Settings")]
    [SerializeField] private KeyCode _jumpKey;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpCooldown;
    [SerializeField] private bool _canJump;
    [SerializeField] private float _airMultiplier;
    [SerializeField] private float _airDrag;
    private float _defaultJumpForce;

    [Header("Sliding Settings")]
    [SerializeField] private KeyCode _slideKey;
    [SerializeField] private float _slideMultiplier;
    [SerializeField] private float _slideDrag;

    private bool _isSliding;



    [Header("Ground Check Settings")]
    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundDrag;
     
    
    private void Awake()
    {
        _stateController = GetComponent<StateController>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.freezeRotation = true;
        _rigidbody.linearDamping = _groundDrag;

        _defaultMovementSpeed = _movementSpeed;
        _defaultJumpForce = _jumpForce;
    }

    private void Update()
    {
        if(GameManager.Instance.GetCurrentGameState() != EGameState.Play &&
            GameManager.Instance.GetCurrentGameState() != EGameState.Resume)
        {
            return;
        }

        SetInputs();
        SetStates();
        SetPlayerDrag();
    }

    private void FixedUpdate()
    {
        if(GameManager.Instance.GetCurrentGameState() != EGameState.Play &&
            GameManager.Instance.GetCurrentGameState() != EGameState.Resume)
        {
            return;
        }

        SetPlayerMovement();
    }

    private void SetInputs()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(_slideKey))
        {
            _isSliding = true;
        }
        else if (Input.GetKeyDown(_movementKey))
        {
            _isSliding = false;
        }
        else if (Input.GetKeyDown(_jumpKey) && _canJump && IsGrounded())
        {
            _canJump = false;
            SetPlayerJump();
            Invoke(nameof(ResetPlayerJump), _jumpCooldown);
        }
    }

    private void SetStates()
    {
        var newState = _stateController.GetCurrentState() switch
        {
            _ when _movementDirection.normalized == Vector3.zero && IsGrounded() && !_isSliding => EPlayerState.Idle,
            _ when _movementDirection.normalized != Vector3.zero && IsGrounded() && !_isSliding => EPlayerState.Move,
            _ when _movementDirection.normalized != Vector3.zero && IsGrounded() && _isSliding => EPlayerState.Slide,
            _ when _movementDirection.normalized == Vector3.zero && IsGrounded() && _isSliding => EPlayerState.SlideIdle,
            _ when !_canJump && !IsGrounded() => EPlayerState.Jump,
            _ => _stateController.GetCurrentState()
        };

        if(newState != _stateController.GetCurrentState())
        {
            _stateController.ChangeState(newState);
            OnPlayerStateChanged?.Invoke(newState);
        }
    }

    private void SetPlayerMovement()
    {
        _movementDirection = (_orientation.forward * _verticalInput 
            + _orientation.right * _horizontalInput).normalized;

        float forceMultiplier = _stateController.GetCurrentState() switch
        {
            EPlayerState.Move => 1f,
            EPlayerState.Slide => _slideMultiplier,
            EPlayerState.Jump => _airMultiplier,
            _ => 1f
        };

        _rigidbody.AddForce(_movementDirection * _movementSpeed * forceMultiplier, ForceMode.Force);
        LimitPlayerSpeed();
    }

    private void SetPlayerDrag()
    {
        _rigidbody.linearDamping = _stateController.GetCurrentState() switch
        {
          EPlayerState.Move => _groundDrag,
          EPlayerState.Slide => _slideDrag,
          EPlayerState.Jump => _airDrag,
          _ => _rigidbody.linearDamping  
        };
    }

    private void LimitPlayerSpeed()
    {
        float currentSpeedLimit = _isSliding ? _movementSpeed * _slideMultiplier : _movementSpeed;
        Vector3 flatVelocity = new Vector3(_rigidbody.linearVelocity.x, 0f, _rigidbody.linearVelocity.z);

        if(flatVelocity.magnitude > currentSpeedLimit)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * currentSpeedLimit;
            _rigidbody.linearVelocity = new Vector3(limitedVelocity.x, _rigidbody.linearVelocity.y,limitedVelocity.z);
        }
    }

    private void SetPlayerJump()
    {
        OnPlayerJumped?.Invoke();
        
        _rigidbody.linearVelocity = new Vector3(_rigidbody.linearVelocity.x, 0f, _rigidbody.linearVelocity.z);
        _rigidbody.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    private void ResetPlayerJump()
    {
        _canJump = true;
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f, _groundLayer);
    }
    
    public void SetMovementSpeed(float speed, float duration)
    {
        _movementSpeed += speed;
        Invoke(nameof(ResetMovementSpeed),duration);
    }

    public void SetJumpForce(float force, float duration)
    {
        _jumpForce += force;
        Invoke(nameof(ResetJumpForce),duration);
    }

    private void ResetMovementSpeed()
    {
        _movementSpeed = _defaultMovementSpeed;
    }

    private void ResetJumpForce()
    {
        _jumpForce = _defaultJumpForce;
    }

    public Rigidbody GetPlayerRigidbody()
    {
        return _rigidbody;
    }
}
