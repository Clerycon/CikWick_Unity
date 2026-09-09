using System;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    /*
    * Kameranın güncel bakış açısını referans alarak oyuncunun yatay hareket yönünü (orientation) hesaplar.
    * Karakterin mantıksal hareket ekseni ile 3D modelini (visual) birbirinden ayırır ve 
    * hareket girdisi olduğunda görsel modelin hedeflenen hareket yönüne pürüzsüzce (Slerp) dönmesini sağlar.
    */

    [Header("References")]
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _orientationTransform;
    [SerializeField] private Transform _playerVisualTransform;

    [Header("Settings")]
    [SerializeField] private float _rotationSpeed;

    private void Update()
    {
        Vector3 viewDirection = 
            _playerTransform.position - new Vector3(transform.position.x, _playerTransform.position.y, transform.position.z);
            
        _orientationTransform.forward = viewDirection.normalized;

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 inputDirection = _orientationTransform.forward * verticalInput 
            + _orientationTransform.right * horizontalInput;

        if(inputDirection != Vector3.zero)
        {
            _playerVisualTransform.forward = 
                Vector3.Slerp(_playerVisualTransform.forward, inputDirection.normalized, _rotationSpeed * Time.deltaTime);
        }
        
    }

}
