using System;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private PlayerController _playerController;
    private StateController _stateController;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _stateController = GetComponent<StateController>();
    }

    private void Start()
    {
        _playerController.OnPlayerJumped += PlayerController_OnPlayerJumped;
    }

    private void Update()
    {
        SetPlayerAnimations();        
    }

    private void SetPlayerAnimations()
    {
        switch (_stateController.GetCurrentState())
        {
            case EPlayerState.Idle:
                _animator.SetBool(Consts.PlayerAnimations.IS_SLIDING, false);
                _animator.SetBool(Consts.PlayerAnimations.IS_MOVING, false);
                break;
            case EPlayerState.Move:
                _animator.SetBool(Consts.PlayerAnimations.IS_SLIDING, false);
                _animator.SetBool(Consts.PlayerAnimations.IS_MOVING, true);
                break;
            case EPlayerState.SlideIdle:
                _animator.SetBool(Consts.PlayerAnimations.IS_SLIDING, true);
                _animator.SetBool(Consts.PlayerAnimations.IS_SLIDING_ACTIVE, false);
                break;
            case EPlayerState.Slide:
                _animator.SetBool(Consts.PlayerAnimations.IS_SLIDING, true);
                _animator.SetBool(Consts.PlayerAnimations.IS_SLIDING_ACTIVE, true);
                break;
        }
    }

    private void PlayerController_OnPlayerJumped()
    {
        _animator.SetBool(Consts.PlayerAnimations.IS_JUMPING, true);
        Invoke(nameof(ResetJumping), 0.5f);
    }

    private void ResetJumping()
    {
        _animator.SetBool(Consts.PlayerAnimations.IS_JUMPING, false);
    }

}
