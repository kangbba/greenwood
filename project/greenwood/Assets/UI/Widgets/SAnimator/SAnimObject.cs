using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public enum SAnimObjectType
{
    TextMeshPro,
    Image,
    CanvasGroup,
    Transform,
    Material,
    Unknown
}

public class SAnimObject : MonoBehaviour
{
    [SerializeField] private SAnimObjectType? _objectType;

    private TextMeshProUGUI _textMeshPro;
    private Image _image;
    private CanvasGroup _canvasGroup;
    private Renderer _renderer;

    private void Awake()
    {
        _textMeshPro = GetComponent<TextMeshProUGUI>();
        _image = GetComponent<Image>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _renderer = GetComponent<Renderer>();
        
        _objectType = DetermineObjectType();
    }

    private SAnimObjectType DetermineObjectType()
    {
        if (gameObject == null) return SAnimObjectType.Unknown;
        if (_textMeshPro != null) return SAnimObjectType.TextMeshPro;
        if (_image != null) return SAnimObjectType.Image;
        if (_canvasGroup != null) return SAnimObjectType.CanvasGroup;
        if (_renderer != null) return SAnimObjectType.Material;
        return SAnimObjectType.Transform;
    }

    public SAnimObjectType ObjectType => _objectType ?? DetermineObjectType();

    public Color ObjectColor =>
        _textMeshPro != null ? _textMeshPro.color :
        _image != null ? _image.color :
        _renderer != null && _renderer.material.HasProperty("_Color") ? _renderer.material.color :
        Color.white;

    public Vector3 ObjectScale => transform.localScale;

    public float ObjectAlpha =>
        _canvasGroup != null ? _canvasGroup.alpha :
        _image != null ? _image.color.a :
        _textMeshPro != null ? _textMeshPro.color.a : 1f;

    public float ObjectFillAmount =>
        _image != null && _image.type == Image.Type.Filled ? _image.fillAmount : 1f;

    public void PlayAnimation(SAnimData animData)
    {
        if (animData == null)
        {
            Debug.LogWarning("PlayAnimation 호출 시 animData가 null입니다.");
            return;
        }

        Debug.Log($"{gameObject.name}에서 애니메이션 실행: 색상 {animData.targetColor}, 크기 {animData.targetScale}, 지속 시간 {animData.duration}");

        if (_textMeshPro != null)
        {
            _textMeshPro.DOColor(animData.targetColor, animData.duration).SetEase(animData.easeType);
        }
        else if (_image != null)
        {
            _image.DOColor(animData.targetColor, animData.duration).SetEase(animData.easeType);
            _image.DOFillAmount(animData.targetFillAmount, animData.duration).SetEase(animData.easeType);
        }
        else if (_canvasGroup != null)
        {
            _canvasGroup.DOFade(animData.targetColor.a, animData.duration).SetEase(animData.easeType);
        }
        else if (_renderer != null && _renderer.material.HasProperty("_Color"))
        {
            _renderer.material.DOColor(animData.targetColor, animData.duration).SetEase(animData.easeType);
        }
        else
        {
            transform.DOScale(animData.targetScale, animData.duration).SetEase(animData.easeType);
        }
    }
}
