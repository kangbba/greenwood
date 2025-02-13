using UnityEngine;
using DG.Tweening;

public class DialogueArrow : MonoBehaviour
{
    [SerializeField] private RectTransform _graphic; // 그래픽을 따로 분리 (애니메이션 적용 대상)

    private const float _amplitude = 10f; // 위아래 이동 거리
    private const float _duration = 0.5f; // 왕복 시간
    private Tween _moveTween;

    /// <summary>
    /// 화살표를 특정 회전 값으로 초기화하고 애니메이션 실행
    /// </summary>
    public void Initialize()
    {
        AnimateArrow();
    }

    /// <summary>
    /// Graphic만 위아래로 흔들리는 애니메이션 실행
    /// </summary>
    private void AnimateArrow()
    {
        Vector3 startPos = _graphic.localPosition;
        _moveTween = _graphic.DOLocalMoveY(startPos.y + _amplitude, _duration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    /// <summary>
    /// 애니메이션 정지 및 오브젝트 제거
    /// </summary>
    public void DestroyArrow()
    {
        _moveTween?.Kill();
        Destroy(gameObject);
    }
}
