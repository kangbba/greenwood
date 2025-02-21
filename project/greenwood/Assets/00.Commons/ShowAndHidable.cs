using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class ShowAndHidable : MonoBehaviour
{
    private Vector2 initialAnchoredPosition = Vector2.zero; // ✅ 기본값 유지
    private RectTransform _rectTransform;
    private bool _isInitialized = false; // ✅ 초기화 여부 체크용

    protected virtual void Awake()
    {
        _rectTransform = GetComponent<Image>()?.rectTransform;
        
        if (_rectTransform == null)
        {
            Debug.LogError($"[ShowAndHidable] {gameObject.name} - Image component with RectTransform not found!");
            return;
        }

        initialAnchoredPosition = _rectTransform.anchoredPosition; // ✅ 초기 위치 캐싱
        _isInitialized = true; // ✅ 초기화 완료 플래그 설정

     //   Debug.Log($"[ShowAndHidable] {gameObject.name} - Initial Anchored Position Cached: {initialAnchoredPosition}");
    }

    public virtual void FadeAndDestroy(float duration)
    {
        gameObject.SetAnimDestroy(duration);
    }

    public void SlideIn(float duration)
    {
        if (!_isInitialized)
        {
            Debug.LogError($"[ShowAndHidable] {gameObject.name} - SlideIn failed: Not initialized!");
            return;
        }

        _rectTransform.DOAnchorPos(initialAnchoredPosition, duration)
            .SetEase(Ease.OutQuad);
    }

    public void SlideOut(Vector2 offset, float duration)
    {
        if (!_isInitialized)
        {
            Debug.LogError($"[ShowAndHidable] {gameObject.name} - SlideOut failed: Not initialized!");
            return;
        }

        _rectTransform.DOAnchorPos(initialAnchoredPosition + offset, duration)
            .SetEase(Ease.InQuad);
    }

    public void FadeIn(float duration)
    {
        gameObject.SetAnim(true, duration);
    }

    public void FadeOut(float duration)
    {
        gameObject.SetAnim(false, duration);
    }
}
