using UnityEngine;

public class GoldWheatCollectible : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;

    [SerializeField] private float _increaseBoostSpeed;
    [SerializeField] private float _resetBoostDuration;

    public void Collect()
    {
        _playerController.SetMovementSpeed(_increaseBoostSpeed, _resetBoostDuration);
        Destroy(gameObject);
    }
}
