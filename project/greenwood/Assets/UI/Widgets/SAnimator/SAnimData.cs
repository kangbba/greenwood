using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class SAnimData
{
    public Color targetColor;
    public Vector3 targetScale;
    public float targetFillAmount;
    public float duration;
    public Ease easeType;

    // 기본 생성자 (초기값을 설정된 기본값으로 초기화)
    public SAnimData()
    {
        targetColor = Color.white;
        targetScale = Vector3.one * 1.15f;
        targetFillAmount = 1f;
        duration = 0.25f;
        easeType = Ease.OutQuad;
    }

    // 사용자 정의 생성자
    public SAnimData(Color color, Vector3 scale, float fillAmount, float duration, Ease ease)
    {
        targetColor = color;
        targetScale = scale;
        targetFillAmount = fillAmount;
        this.duration = duration;
        easeType = ease;
    }
}