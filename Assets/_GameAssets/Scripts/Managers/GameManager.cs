using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public event Action<EGameState> OnGameStateChanged;

    [SerializeField] private CatController _catController;
    [SerializeField] private EggCounterUI _eggCounterUI;
    [SerializeField] private WinLoseUI _winLoseUI;
    [SerializeField] private PlayerHealthUI _playerHealthUI;
    
    [SerializeField] private int _maxEggCount = 5;
    [SerializeField] private float _delay;
    private int _currentEggCount;

    private EGameState _currentGameState;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        HealthManager.Instance.OnPlayerDeath += HealthManager_OnPlayerDeath;
        _catController.OnCatCatched += CatController_OnCatCatched;
    }

    private void CatController_OnCatCatched()
    {
        _playerHealthUI.AnimateDamageForAll();
        StartCoroutine(OnGameOver());
    }

    private void HealthManager_OnPlayerDeath()
    {
        StartCoroutine(OnGameOver());
    }

    private void OnEnable()
    {
        ChangeGameState(EGameState.Play);
    }

    public void OnEggCollected()
    {
        _currentEggCount++;
        
        _eggCounterUI.SetEggCounterText(_currentEggCount,_maxEggCount);

        if(_currentEggCount == _maxEggCount)
        {
            _eggCounterUI.SetEggCompleted();
            ChangeGameState(EGameState.GameOver);
            _winLoseUI.OnGameWin();
        }        
    }

    private IEnumerator OnGameOver()
    {
        yield return new WaitForSeconds(_delay);

        ChangeGameState(EGameState.GameOver);
        _winLoseUI.OnGameLose();
    }

    public void ChangeGameState(EGameState gameState)
    {
        OnGameStateChanged?.Invoke(gameState);
        _currentGameState = gameState;
        Debug.Log("Game State: " + gameState);
    }

    public EGameState GetCurrentGameState()
    {
        return _currentGameState;
    }
}


 