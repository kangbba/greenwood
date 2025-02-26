using UnityEngine;
using DG.Tweening;

public class HighestCanvas : MonoBehaviour
{
    [SerializeField] private AnimationImage _blackPanel; // ✅ AnimationImage를 통한 Black Panel 바인딩

    /// <summary>
    /// BlackPanel을 활성화 또는 비활성화 (페이드 효과 적용)
    /// </summary>
    /// <param name="isOn">true: 활성화 (검은 화면), false: 비활성화 (투명)</param>
    /// <param name="duration">애니메이션 지속 시간</param>
    public void SetBlackPanel(bool isOn, float duration)
    {
        if (_blackPanel == null)
        {
            Debug.LogError("[HighestCanvas] _blackPanel이 바인딩되지 않았습니다!");
            return;
        }

        Color targetColor = isOn ? Color.black : Color.black.ModifiedAlpha(0f); // 검은색 or 투명
        _blackPanel.SetColor(targetColor, duration);
    }
}
