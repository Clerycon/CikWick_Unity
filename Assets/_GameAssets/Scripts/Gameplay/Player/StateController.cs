using UnityEngine;

public class StateController : MonoBehaviour
{
    private EPlayerState _currentPlayerState = EPlayerState.Idle;

    private void Start()
    {
        ChangeState(EPlayerState.Idle);
    }

    public void ChangeState(EPlayerState newPlayerState)
    {
        if (_currentPlayerState == newPlayerState) { return; }

        _currentPlayerState = newPlayerState;
    }

    public EPlayerState GetCurrentState()
    {
        return _currentPlayerState;
    }


}
