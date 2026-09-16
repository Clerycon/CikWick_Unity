using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image[] _playerHealthImages;
    private RectTransform[] _playerHealthTransforms;
    
    [Header("Sprites")]

    [SerializeField] private Sprite _playerHealthySprite;
    [SerializeField] private Sprite _playerUnhealthySprite;
    
    
    [Header("Settings")]
    [SerializeField] private float _scaleDuration;
    

    private void Awake()
    {
        _playerHealthTransforms = new RectTransform[_playerHealthImages.Length];
        for(int i = 0; i < _playerHealthImages.Length; i++)
        {
            _playerHealthTransforms[i] = _playerHealthImages[i].gameObject.GetComponent<RectTransform>();
        }
    }

    // FOR TESTING REASONS
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            AnimateDamage();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            AnimateDamageForAll();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            AnimateHeal();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            AnimateHealForAll();
        }
    }

    public void AnimateDamage()
    {
        for(int i = 0; i < _playerHealthImages.Length; i++)
        {
            if(_playerHealthImages[i].sprite == _playerHealthySprite)
            {
                AnimateDamageSprite(_playerHealthImages[i], _playerHealthTransforms[i]);
                break;
            }
        }
    }

    public void AnimateDamageForAll()
    {
        for(int i = 0; i < _playerHealthImages.Length; i++)
        {
            if(_playerHealthImages[i].sprite == _playerHealthySprite)
            {
                AnimateDamageSprite(_playerHealthImages[i], _playerHealthTransforms[i]);
            }
        }
    }

    public void AnimateHeal()
    {
        for(int i = _playerHealthImages.Length - 1; i >= 0; i--)
        {
            if(_playerHealthImages[i].sprite == _playerUnhealthySprite)
            {
                AnimateHealSprite(_playerHealthImages[i],_playerHealthTransforms[i]);
                break;
            }   
        }
    }

    public void AnimateHealForAll()
    {
        for(int i = _playerHealthImages.Length - 1; i >= 0; i--)
        {
            if(_playerHealthImages[i].sprite == _playerUnhealthySprite)
            {
                AnimateHealSprite(_playerHealthImages[i],_playerHealthTransforms[i]);
            }   
        }
    }

    private void AnimateDamageSprite(Image activeImage, RectTransform activeImageTransform)
    {
        activeImageTransform.DOScale(0f, _scaleDuration).SetEase(Ease.InBack).OnComplete(() =>
        {
            activeImage.sprite = _playerUnhealthySprite;
            activeImageTransform.DOScale(1f, _scaleDuration).SetEase(Ease.OutBack);
        });
    }

    private void AnimateHealSprite(Image activeImage, RectTransform activeImageTransform)
    {
        activeImageTransform.DOScale(0f, _scaleDuration).SetEase(Ease.InBack).OnComplete(() =>
        {
            activeImage.sprite = _playerHealthySprite;
            activeImageTransform.DOScale(1f, _scaleDuration).SetEase(Ease.OutBack);
        });
    }
}
