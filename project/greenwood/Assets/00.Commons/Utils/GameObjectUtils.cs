using UnityEngine;
using DG.Tweening;

public static class GameObjectUtils
{
    /// <summary>
    /// ✅ `delay`(초) 후에 GameObject를 활성화 또는 비활성화
    /// </summary>
    public static void SetActive(this GameObject gameObject, bool isActive, float delay)
    {
        if (gameObject == null)
        {
            Debug.LogWarning("[GameObjectUtils] SetActive failed: GameObject is null!");
            return;
        }

        if (delay <= 0)
        {
            gameObject.SetActive(isActive); // ✅ 즉시 실행
        }
        else
        {
            DOVirtual.DelayedCall(delay, () => gameObject.SetActive(isActive));
        }
    }
}
