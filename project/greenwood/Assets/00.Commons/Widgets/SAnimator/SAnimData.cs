using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class SAnimData
{
    public Color targetColor;
    public Vector3 targetLocalScale;
    public Vector3 targetLocalPosition;
    public Quaternion targetLocalRotation;
    public float targetFillAmount;
    public float duration;
    public Ease easeType;

    // 기본 생성자 (초기값을 설정된 기본값으로 초기화)
    public SAnimData()
    {
        targetColor = Color.white;
        targetLocalScale = Vector3.one * 1.15f;
        targetFillAmount = 1f;
        duration = 0.25f;
        easeType = Ease.OutQuad;
    }

    // 사용자 정의 생성자
    public SAnimData(Color color, Vector3 localScale, float fillAmount, float duration, Ease ease)
    {
        targetColor = color;
        targetLocalScale = localScale;
        targetFillAmount = fillAmount;
        this.duration = duration;
        easeType = ease;
    }
}