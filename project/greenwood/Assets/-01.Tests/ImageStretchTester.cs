using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class ImageStretchTester : MonoBehaviour
{
    [Title("이미지 설정")]
    [SerializeField] private Image _targetImage; // ✅ 적용할 Image

    [Button("📌 가로 Stretch + 세로 비율 유지 적용")]
    private void ApplyStretch()
    {
        if (_targetImage == null)
        {
            Debug.LogError("[ImageStretchTester] 대상 Image가 없습니다! 할당해주세요.");
            return;
        }

        _targetImage.ApplyStretchWithAspectRatio(); // ✅ 이미지에 Stretch 적용
        Debug.Log($"[ImageStretchTester] '{_targetImage.gameObject.name}' - Stretch 적용 완료!");
    }
}
