using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public enum SAnimObjectType
{
    Unknown,
    TextMeshPro,
    Image,
    CanvasGroup,
    Transform,
}

public class SAnimObject : MonoBehaviour
{
    [SerializeField] private SAnimObjectType _objectType = SAnimObjectType.Unknown;

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
        return SAnimObjectType.Transform;
    }

    public SAnimObjectType ObjectType => DetermineObjectType();

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

     //   Debug.Log($"{gameObject.name}에서 애니메이션 실행: 색상 {animData.targetColor}, 크기 {animData.targetLocalScale}, 지속 시간 {animData.duration}");

        switch (_objectType)
        {
            case SAnimObjectType.TextMeshPro:
                _textMeshPro.DOColor(animData.targetColor, animData.duration).SetEase(animData.easeType);
                _textMeshPro.transform.DOScale(animData.targetLocalScale, animData.duration).SetEase(animData.easeType);
                break;

            case SAnimObjectType.Image:
                _image.DOColor(animData.targetColor, animData.duration).SetEase(animData.easeType);
                _image.transform.DOScale(animData.targetLocalScale, animData.duration).SetEase(animData.easeType);
                if (_image.type == Image.Type.Filled)
                {
                    _image.DOFillAmount(animData.targetFillAmount, animData.duration).SetEase(animData.easeType);
                }
                break;

            case SAnimObjectType.CanvasGroup:
                _canvasGroup.DOFade(animData.targetColor.a, animData.duration).SetEase(animData.easeType);
                _canvasGroup.transform.DOScale(animData.targetLocalScale, animData.duration).SetEase(animData.easeType);
                break;

            case SAnimObjectType.Transform:
                transform.DOScale(animData.targetLocalScale, animData.duration).SetEase(animData.easeType);
                transform.DOLocalMove(animData.targetLocalPosition, animData.duration).SetEase(animData.easeType);
                transform.DOLocalRotateQuaternion(animData.targetLocalRotation, animData.duration).SetEase(animData.easeType);
                break;

            default:
                Debug.LogWarning($"{gameObject.name}의 애니메이션을 실행할 수 없습니다.");
                break;
        }
    }
}
