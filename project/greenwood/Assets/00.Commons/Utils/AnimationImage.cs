using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AnimationImage : MonoBehaviour
{
    // ✅ RectTransform을 동적으로 가져오기 (Image가 없을 경우 대비)
    private RectTransform _rectTransform => GetComponent<RectTransform>();

    // ✅ Image를 자동으로 추가 (없다면 추가)
    private Image _image;
    private Image ImageComponent
    {
        get
        {
            if (_image == null)
            {
                _image = GetComponent<Image>();
                if (_image == null)
                {
                    _image = gameObject.AddComponent<Image>(); // ✅ 없으면 자동 추가
                    Debug.LogWarning($"[AnimationImage] {gameObject.name} - Image component was missing. Added automatically.");
                }
            }
            return _image;
        }
    }

    // ✅ CanvasGroup을 동적으로 가져오거나 생성
    private CanvasGroup _canvasGroup;
    private CanvasGroup CanvasGroup
    {
        get
        {
            if (_canvasGroup == null)
            {
                _canvasGroup = GetComponent<CanvasGroup>();
                if (_canvasGroup == null)
                {
                    _canvasGroup = gameObject.AddComponent<CanvasGroup>(); // ✅ 없으면 추가
                }
            }
            return _canvasGroup;
        }
    }

    /// <summary>
    /// ✅ CanvasGroup의 `raycastTarget` 및 `blocksRaycasts` 자동 제어
    /// </summary>
    private void AdjustCanvasGroup(bool isActivating, float duration)
    {
        if (isActivating)
        {
            // ✅ 활성화: 애니메이션 완료 후 클릭 가능하게 설정
            CanvasGroup.blocksRaycasts = false;
            CanvasGroup.interactable = false;
            DOVirtual.DelayedCall(duration, () =>
            {
                CanvasGroup.blocksRaycasts = true;
                CanvasGroup.interactable = true;
            });
        }
        else
        {
            // ✅ 비활성화: 즉시 클릭 차단
            CanvasGroup.blocksRaycasts = false;
            CanvasGroup.interactable = false;
        }
    }

    public void Fade(float targetAlpha, float duration, Ease easeType = Ease.OutQuad)
    {
        AdjustCanvasGroup(targetAlpha > 0, duration);
        CanvasGroup.DOFade(targetAlpha, duration).SetEase(easeType);
    }

    public void FadeFrom(float target, float from, float duration, Ease easeType = Ease.OutQuad)
    {
        if(duration == 0){
            CanvasGroup.alpha = target;
            AdjustCanvasGroup(target > 0, duration);
        }
        else{
            CanvasGroup.alpha = from;
            AdjustCanvasGroup(target > 0, duration);
            CanvasGroup.DOFade(target, duration).SetEase(easeType);
        }
    }

    public void Scale(float scaleMultiplier, float duration, Ease easeType = Ease.OutQuad)
    {
        if (_rectTransform == null)
        {
            Debug.LogError($"[AnimationImage] {gameObject.name} - Scale failed: RectTransform not found!");
            return;
        }

        AdjustCanvasGroup(scaleMultiplier > 0, duration);
        _rectTransform.DOScale(scaleMultiplier, duration).SetEase(easeType);
    }

    public void FadeAndDestroy(float duration, Ease easeType = Ease.OutQuad)
    {
        AdjustCanvasGroup(false, 0);
        CanvasGroup.DOFade(0, duration)
            .SetEase(easeType)
            .OnComplete(() => Destroy(gameObject));
    }

    public void FadeIn(float duration, Ease easeType = Ease.OutQuad)
    {
        Fade(1f, duration, easeType);
    }

    public void FadeOut(float duration, Ease easeType = Ease.OutQuad)
    {
        Fade(0f, duration, easeType);
    }

    public void Move(Vector2 target, Vector2? from = null, float duration = 0.5f, Ease easeType = Ease.OutQuad)
    {
        if (_rectTransform == null)
        {
            Debug.LogError($"[AnimationImage] {gameObject.name} - Move failed: RectTransform not found!");
            return;
        }

        if (from.HasValue)
        {
            _rectTransform.anchoredPosition = from.Value;
        }

        _rectTransform.DOAnchorPos(target, duration).SetEase(easeType);
    }

    public void Shake(float strength = 10f, float duration = 0.5f, Ease easeType = Ease.OutQuad)
    {
        if (_rectTransform == null)
        {
            Debug.LogError($"[AnimationImage] {gameObject.name} - Shake failed: RectTransform not found!");
            return;
        }

        _rectTransform.DOShakeAnchorPos(duration, strength, 10, 90f, false, true)
            .SetEase(easeType)
            .OnStart(() => Debug.Log($"[AnimationImage] {gameObject.name} - Shaking started"))
            .OnComplete(() => Debug.Log($"[AnimationImage] {gameObject.name} - Shaking complete"));
    }

    public void SetColor(Color targetColor, float duration, Ease easeType = Ease.OutQuad)
    {
        if (ImageComponent == null)
        {
            Debug.LogError($"[AnimationImage] {gameObject.name} - SetColor failed: Image component not found!");
            return;
        }

        if (duration <= 0)
        {
            ImageComponent.color = targetColor;
        }
        else
        {
            ImageComponent.DOColor(targetColor, duration).SetEase(easeType);
        }
    }
}
