using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Transform _orientation;

    [Header("Value")]
    [SerializeField] private float _movementSpeed;

    private Rigidbody _rigidbody;
    private float _horizontalInput, _verticalInput;
    private Vector3 _movementDirection;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.freezeRotation = true;
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
    }

    private void SetPlayerMovement()
    {
        _movementDirection = (_orientation.forward * _verticalInput 
            + _orientation.right * _horizontalInput).normalized;
        
        _rigidbody.AddForce(_movementDirection * _movementSpeed, ForceMode.Force);
    }
}
