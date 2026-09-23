using UnityEngine;

public class CatStateController : MonoBehaviour
{
    [SerializeField] private ECatState _currentCatState = ECatState.Walking;

    private void Start()
    {
        ChangeState(ECatState.Walking);
    }
    public void ChangeState(ECatState newState)
    {
        if(_currentCatState == newState) { return; }

        _currentCatState = newState;
    }

    public ECatState GetCurrentState()
    {
        return _currentCatState;
    }
}
