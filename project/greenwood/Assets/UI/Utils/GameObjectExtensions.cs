using UnityEngine;
using DG.Tweening;

public static class GameObjectExtensions
{
    /// <summary>
    /// GameObject의 활성화/비활성화 애니메이션 (Alpha Fade In/Out 후 SetActive 적용)
    /// </summary>
    public static void SetAnim(this GameObject obj, bool isActive, float duration)
    {
        if (obj == null) return;

        CanvasGroup canvasGroup = obj.GetOrAddCanvasGroup();

        if (duration == 0)
        {
            obj.SetActive(isActive);
            canvasGroup.alpha = isActive ? 1 : 0;
            canvasGroup.SetInteractable(isActive);
            return;
        }

        if (isActive)
        {
            obj.SetActive(true);
            canvasGroup.alpha = 0;
            canvasGroup.SetInteractable(false);

            canvasGroup.DOFade(1, duration).OnComplete(() =>
            {
                canvasGroup.SetInteractable(true);
            });
        }
        else
        {
            canvasGroup.SetInteractable(false);

            canvasGroup.DOFade(0, duration).OnComplete(() =>
            {
            });
        }
    }
    public static void SetAnimActive(this GameObject obj, bool isActive, float duration)
    {
        if (obj == null) return;

        CanvasGroup canvasGroup = obj.GetOrAddCanvasGroup();

        if (duration == 0)
        {
            obj.SetActive(isActive);
            canvasGroup.alpha = isActive ? 1 : 0;
            canvasGroup.SetInteractable(isActive);
            return;
        }

        if (isActive)
        {
            obj.SetActive(true);
            canvasGroup.alpha = 0;
            canvasGroup.SetInteractable(false);

            canvasGroup.DOFade(1, duration).OnComplete(() =>
            {
                canvasGroup.SetInteractable(true);
            });
        }
        else
        {
            canvasGroup.SetInteractable(false);

            canvasGroup.DOFade(0, duration).OnComplete(() =>
            {
                obj.SetActive(false);
            });
        }
    }

    /// <summary>
    /// GameObject의 페이드 아웃 후 파괴 애니메이션
    /// </summary>
    public static void SetAnimDestroy(this GameObject obj, float duration)
    {
        if (obj == null) return;

        CanvasGroup canvasGroup = obj.GetOrAddCanvasGroup();

        if (duration == 0)
        {
            canvasGroup.alpha = 0;
            Object.Destroy(obj);
            return;
        }

        canvasGroup.SetInteractable(false);

        canvasGroup.DOFade(0, duration).OnComplete(() =>
        {
            Object.Destroy(obj);
        });
    }

    /// <summary>
    /// GameObject의 스케일 기반 활성화 애니메이션 (Scale 0 -> 1 후 SetActive 적용)
    /// </summary>
    public static void SetAnimScaleActive(this GameObject obj, bool isActive, float duration)
    {
        if (obj == null) return;

        CanvasGroup canvasGroup = obj.GetOrAddCanvasGroup();

        if (duration == 0)
        {
            obj.SetActive(isActive);
            obj.transform.localScale = isActive ? Vector3.one : Vector3.zero;
            canvasGroup.SetInteractable(isActive);
            return;
        }

        if (isActive)
        {
            obj.SetActive(true);
            obj.transform.localScale = Vector3.zero;
            canvasGroup.SetInteractable(false);

            obj.transform.DOScale(Vector3.one, duration).SetEase(Ease.OutBack).OnComplete(() =>
            {
                canvasGroup.SetInteractable(true);
            });
        }
        else
        {
            canvasGroup.SetInteractable(false);

            obj.transform.DOScale(Vector3.zero, duration).SetEase(Ease.InBack).OnComplete(() =>
            {
                obj.SetActive(false);
            });
        }
    }

    /// <summary>
    /// GameObject의 스케일 기반 파괴 애니메이션 (Scale 1 -> 0 후 Destroy)
    /// </summary>
    public static void SetAnimScaleDestroy(this GameObject obj, float duration)
    {
        if (obj == null) return;

        CanvasGroup canvasGroup = obj.GetOrAddCanvasGroup();

        if (duration == 0)
        {
            obj.transform.localScale = Vector3.zero;
            Object.Destroy(obj);
            return;
        }

        canvasGroup.SetInteractable(false);

        obj.transform.DOScale(Vector3.zero, duration).SetEase(Ease.InBack).OnComplete(() =>
        {
            Object.Destroy(obj);
        });
    }

    /// <summary>
    /// GameObject에 CanvasGroup이 없으면 추가하고 반환
    /// </summary>
    private static CanvasGroup GetOrAddCanvasGroup(this GameObject obj)
    {
        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = obj.AddComponent<CanvasGroup>();
        }

        return canvasGroup;
    }

    /// <summary>
    /// 입력 차단 및 복원 설정
    /// </summary>
    private static void SetInteractable(this CanvasGroup canvasGroup, bool isInteractable)
    {
        canvasGroup.interactable = isInteractable;
        canvasGroup.blocksRaycasts = isInteractable;
    }
}
