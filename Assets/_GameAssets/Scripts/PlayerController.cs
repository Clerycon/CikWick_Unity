using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _orientation;

    private Rigidbody _rigidbody;
    

    [Header("Movement Settings")]
    [SerializeField] private KeyCode _movementKey;
    [SerializeField] private float _movementSpeed;

    private float _horizontalInput, _verticalInput;
    private Vector3 _movementDirection;


    [Header("Jump Settings")]
    [SerializeField] private KeyCode _jumpKey;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpCooldown;
    [SerializeField] private bool _canJump;

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
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.freezeRotation = true;
        _rigidbody.linearDamping = _groundDrag;
    }

    private void Update()
    {
        SetInputs();
    }

    private void FixedUpdate()
    {
        SetPlayerMovement();
    }

    private void SetInputs()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(_slideKey))
        {
            _isSliding = true;
            _rigidbody.linearDamping = _slideDrag;
        }
        else if (Input.GetKeyDown(_movementKey))
        {
            _isSliding = false;
            _rigidbody.linearDamping = _groundDrag;
        }
        else if (Input.GetKeyDown(_jumpKey) && _canJump && IsGrounded())
        {
            _canJump = false;
            SetPlayerJump();
            Invoke(nameof(ResetPlayerJump), _jumpCooldown);
        }
    }

    private void SetPlayerMovement()
    {
        _movementDirection = (_orientation.forward * _verticalInput 
            + _orientation.right * _horizontalInput).normalized;
        if (_isSliding)
        {
            _rigidbody.AddForce(_movementDirection * _movementSpeed * _slideMultiplier, ForceMode.Force);
        }
        else
        {
            _rigidbody.AddForce(_movementDirection * _movementSpeed, ForceMode.Force);
        }
        LimitPlayerSpeed();
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
}
