using UnityEngine;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using DG.Tweening;
using UnityEditor;

public class Cheek : MonoBehaviour
{
    [SerializeField] private Image[] _flushImgs; // ✅ 볼터치 효과 이미지 배열

    [Title("Alpha Settings")]
    [SerializeField] private float _targetAlpha = 0.6f; // ✅ 활성화 시 목표 알파값
    [SerializeField] private float _disabledAlpha = 0.2f; // ✅ 비활성화 시 목표 알파값 (꺼졌을 때 남길 수치)

    private bool _isFlushActive = false; // 현재 Flush 상태

    private void Awake()
    {
        if (_flushImgs == null || _flushImgs.Length == 0)
        {
            Debug.LogWarning("[Cheek] _flushImgs가 설정되지 않음!");
        }
    }

    /// <summary>
    /// 🔹 뺨(볼터치) 활성화 (Alpha 조절)
    /// </summary>
    public void SetFlush(bool isActive, float duration)
    {
        if (_flushImgs == null || _flushImgs.Length == 0)
        {
            Debug.LogWarning("[Cheek] _flushImgs가 설정되지 않음.");
            return;
        }

        _isFlushActive = isActive; // ✅ 내부 상태를 먼저 변경

        float targetAlpha = _isFlushActive ? _targetAlpha : _disabledAlpha; // ✅ 활성화/비활성화 알파값 설정

        foreach (var img in _flushImgs)
        {
            if (img == null) continue;

            if (duration <= 0f)
            {
                Color instantColor = img.color;
                instantColor.a = targetAlpha;
                img.color = instantColor; // ✅ 즉시 적용
            }
            else
            {
                img.DOKill(); // 🔥 기존 Tween 즉시 중단 (중복 방지)
                img.DOFade(targetAlpha, duration); // ✅ Tween으로 부드러운 변화
            }
        }

        Debug.Log($"[Cheek] 🔄 Flush 상태 변경: {_isFlushActive}, 적용된 Alpha: {targetAlpha}");
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            transform.localPosition = Vector3.zero; // ✅ 항상 (0,0,0) 유지
            transform.localRotation = Quaternion.identity; // ✅ 항상 회전 없음
            transform.localScale = Vector3.one;

            EditorUtility.SetDirty(this);
            SceneView.RepaintAll();
        }
    }
#endif

    /// <summary>
    /// 🔹 Sirenix Odin Inspector 버튼을 활용한 Flush 효과 토글
    /// </summary>
    [Button("🔄 Toggle Flush")]
    private void ToggleFlush()
    {
        SetFlush(!_isFlushActive, 0f); // ✅ 상태 반전 후 적용
    }
}
