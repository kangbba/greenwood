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
                    _canvasGroup.interactable = ImageComponent.raycastTarget;
                    _canvasGroup.blocksRaycasts = true;
                }
            }
            return _canvasGroup;
        }
    }
    public void Fade(float targetAlpha, float duration, Ease easeType = Ease.OutQuad)
    {
        CanvasGroup.DOFade(targetAlpha, duration)
            .SetEase(easeType);
    }

    public void FadeFrom(float target, float from, float duration, Ease easeType = Ease.OutQuad)
    {
        CanvasGroup.alpha = from; // ✅ 초기 알파 값 설정
        CanvasGroup.DOFade(target, duration)
            .SetEase(easeType);
    }


    public void Scale(float scaleMultiplier, float duration, Ease easeType = Ease.OutQuad)
    {
        if (_rectTransform == null)
        {
            Debug.LogError($"[AnimationImage] {gameObject.name} - Scale failed: RectTransform not found!");
            return;
        }

        _rectTransform.DOScale(scaleMultiplier, duration)
            .SetEase(easeType);
    }

    /// <summary>
    /// ✅ FadeOut 후 오브젝트 제거 (CanvasGroup 활용)
    /// </summary>
    public void FadeAndDestroy(float duration, Ease easeType = Ease.OutQuad)
    {
        CanvasGroup.DOFade(0, duration)
            .SetEase(easeType)
            .OnComplete(() =>{
                if(gameObject != null){
                    Destroy(gameObject);
                }
            });
    }

    /// <summary>
    /// ✅ Fade In 애니메이션 (CanvasGroup 사용)
    /// </summary>
    public void FadeIn(float duration, Ease easeType = Ease.OutQuad)
    {
        CanvasGroup.DOFade(1, duration)
            .SetEase(easeType);
    }

    /// <summary>
    /// ✅ Fade Out 애니메이션 (CanvasGroup 사용)
    /// </summary>
    public void FadeOut(float duration, Ease easeType = Ease.OutQuad)
    {
        CanvasGroup.DOFade(0, duration)
            .SetEase(easeType);
    }

   /// <summary>
    /// ✅ **지정된 목표 위치로 이동 (필요 시 시작 위치 지정)**
    /// </summary>
    /// <param name="target">목표 위치</param>
    /// <param name="from">시작 위치 (선택적, null일 경우 현재 위치에서 이동)</param>
    /// <param name="duration">애니메이션 지속 시간</param>
    /// <param name="easeType">Ease 타입 (기본값: Ease.OutQuad)</param>
    public void Move(Vector2 target, Vector2? from = null, float duration = 0.5f, Ease easeType = Ease.OutQuad)
    {
        if (_rectTransform == null)
        {
            Debug.LogError($"[AnimationImage] {gameObject.name} - Move failed: RectTransform not found!");
            return;
        }

        // ✅ `from` 값이 있으면 시작 위치 설정
        if (from.HasValue)
        {
            _rectTransform.anchoredPosition = from.Value;
        }

        // ✅ 목표 위치로 이동 애니메이션 적용
        _rectTransform.DOAnchorPos(target, duration).SetEase(easeType);
    }


    /// <summary>
    /// ✅ **UI 요소를 흔들리게 만드는 효과**
    /// </summary>
    public void Shake(float strength = 10f, float duration = 0.5f, Ease easeType = Ease.OutQuad)
    {
        if (_rectTransform == null)
        {
            Debug.LogError($"[AnimationImage] {gameObject.name} - Shake failed: RectTransform not found!");
            return;
        }

        // ✅ 멋진 흔들림 효과 적용 (DOTween 기본 추천 수치)
        _rectTransform.DOShakeAnchorPos(duration, strength, 10, 90f, false, true)
            .SetEase(easeType)
            .OnStart(() => Debug.Log($"[AnimationImage] {gameObject.name} - Shaking started"))
            .OnComplete(() => Debug.Log($"[AnimationImage] {gameObject.name} - Shaking complete"));
    }
}
