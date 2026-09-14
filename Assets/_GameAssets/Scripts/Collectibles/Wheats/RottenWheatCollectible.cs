using UnityEngine;

public class RottenWheatCollectible : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;

    [SerializeField] private float _decreaseBoostSpeed;
    [SerializeField] private float _resetBoostDuration;

    public void Collect()
    {
        _playerController.SetMovementSpeed(_decreaseBoostSpeed, _resetBoostDuration);
        Destroy(gameObject);
    }
}
