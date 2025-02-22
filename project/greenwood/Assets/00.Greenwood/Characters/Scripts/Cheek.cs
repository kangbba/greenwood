using UnityEngine;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using DG.Tweening;
using UnityEditor;

public class Cheek : MonoBehaviour
{
    [SerializeField] private Image _cheekImg; // 볼터치 효과 이미지

    private bool _isFlushActive = false; // 현재 Flush 상태

    /// <summary>
    /// 뺨(볼터치 등) 활성화 (Alpha 조절)
    /// </summary>
    public void SetFlush(bool isActive, float duration)
    {
        if (_cheekImg == null)
        {
            Debug.LogWarning("[Cheek] _cheekImg가 할당되지 않았습니다.");
            return;
        }

        float targetAlpha = isActive ? 0.6f : 0f; // 볼터치 활성화 시 0.6, 비활성화 시 0
        if (duration <= 0f)
        {
            Color instantColor = _cheekImg.color;
            instantColor.a = targetAlpha;
            _cheekImg.color = instantColor; // 즉시 적용
        }
        else
        {
            _cheekImg.DOFade(targetAlpha, duration); // Tween으로 부드러운 변화
        }
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
    /// Sirenix Odin Inspector 버튼을 활용한 Flush 효과 토글
    /// </summary>
    [Button("🔄 Toggle Flush")]
    private void ToggleFlush()
    {
        _isFlushActive = !_isFlushActive;
        SetFlush(_isFlushActive, 0f);
        Debug.Log($"[Cheek] Flush 상태 변경: {_isFlushActive}");
    }
}
