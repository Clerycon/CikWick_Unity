using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _settingsPopupObject;
    [SerializeField] private GameObject _blackBackgroundObject;

    [Header("Buttons")]
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _musicButton;
    [SerializeField] private Button _soundButton;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _mainMenuButton;

    [Header("Settings")]
    [SerializeField] private float _animationDuration;
    private Image _blackBackgroundImage;


    private void Awake()
    {
        _blackBackgroundImage = _blackBackgroundObject.GetComponent<Image>();
        _settingsPopupObject.transform.localScale = Vector3.zero;


        _settingsButton.onClick.AddListener(OnSettingsButtonCLicked);
        _resumeButton.onClick.AddListener(OnResumeButtonClicked);
    }

    private void OnSettingsButtonCLicked()
    {
        GameManager.Instance.ChangeGameState(EGameState.Pause);

        _blackBackgroundObject.SetActive(true);
        _settingsPopupObject.SetActive(true);
        
        _blackBackgroundImage.DOFade(0.8f, _animationDuration).SetEase(Ease.Linear);
        _settingsPopupObject.transform.DOScale(1.5f, _animationDuration).SetEase(Ease.OutBack);
    }

    private void OnResumeButtonClicked()
    {
        _blackBackgroundImage.DOFade(0f, _animationDuration).SetEase(Ease.Linear);
        _settingsPopupObject.transform.DOScale(0f, _animationDuration).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            _settingsPopupObject.SetActive(false);
            _blackBackgroundObject.SetActive(false);
             GameManager.Instance.ChangeGameState(EGameState.Resume);
        });
    }

}
