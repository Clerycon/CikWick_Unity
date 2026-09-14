using UnityEngine;

public class HolyWheatCollectible : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;

    [SerializeField] private float _increaseBoostForce;
    [SerializeField] private float _resetBoostDuration;

    public void Collect()
    {
        _playerController.SetJumpForce(_increaseBoostForce, _resetBoostDuration);
        Destroy(gameObject);
    }
}
