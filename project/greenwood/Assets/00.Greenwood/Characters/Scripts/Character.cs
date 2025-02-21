using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System;
using DG.Tweening;
using Microsoft.Unity.VisualStudio.Editor;

/// <summary>
/// 캐릭터의 전체적인 상태(Idle, Talking 등)
/// </summary>

public class Character : AnimationImage
{
    [SerializeField] private ECharacterName _characterName;
    public ECharacterName CharacterName => _characterName;

    [SerializeField] private EmotionHandler _emotionHandler;
    [SerializeField] private PoseHandler _poseHandler;
    public EmotionHandler EmotionHandler => _emotionHandler;
    public PoseHandler PoseHandler => _poseHandler;

    private RectTransform _rectTransform;


    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        // 필요 시 Fetch
        _emotionHandler?.FetchEmotions();
        _poseHandler?.FetchPoses();
    }
    public void Init(string initialEmotionID, string initialPoseID, float duration){
        FadeOut(0f);
        FadeIn(duration);
        SetEmotion(initialEmotionID, 0f);
        SetPose(initialPoseID, 0f);
    }


    // 원하는 경우, Character가 직접 pass-through 메서드 제공
    public void SetEmotion(string emotionID, float duration)
    {
        _emotionHandler?.SetEmotion(emotionID, duration);
    }

    public void SetPose(string poseID, float duration)
    {
        _poseHandler?.SetPose(poseID, duration);
    }

    // 원하는 경우, Character가 직접 pass-through 메서드 제공
    public void PlayMouthWithCurrentEmotion(bool b)
    {
        _emotionHandler?.PlayMouthWithCurrentEmotion(b);
    }

    /// <summary>
    /// 캐릭터를 특정 위치(CharacterLocation)로 부드럽게 이동
    /// </summary>
    public void MoveToLocationX(CharacterLocation targetLocation, float duration, Ease easeType = Ease.Linear)
    {
        if (_rectTransform == null)
        {
            Debug.LogError($"[Character] {_rectTransform.gameObject.name} - RectTransform is null!");
            return;
        }

        float screenWidth = UIManager.Instance.GameCanvas.GetComponent<RectTransform>().rect.width;
        float targetX = GetPositionX(targetLocation, screenWidth);

        // ✅ DOTween을 사용하여 X축 이동
        _rectTransform.DOAnchorPosX(targetX, duration).SetEase(easeType);
    }
    /// <summary>
    /// 캐릭터 점프 애니메이션
    /// </summary>
    public void Jump(float jumpHeight = 100f, float duration = 0.5f)
    {
        if (_rectTransform == null)
        {
            Debug.LogError($"[Character] {gameObject.name} - RectTransform is null!");
            return;
        }

        Vector2 originalPosition = _rectTransform.anchoredPosition;

        // DOTween을 사용하여 점프 (상승 → 하강을 하나의 곡선으로 표현)
        _rectTransform.DOAnchorPosY(originalPosition.y + jumpHeight, duration)
            .SetEase(Ease.OutQuad) // ✅ 점프 곡선 적용
            .SetLoops(2, LoopType.Yoyo); // ✅ 한 번 올라갔다가 내려오도록 설정
    }



    /// <summary>
    /// `CharacterLocation` Enum에 따라 X 좌표 계산
    /// </summary>
    private float GetPositionX(CharacterLocation location, float screenWidth)
    {
        return location switch
        {
            CharacterLocation.Left2 => screenWidth * -0.33f,
            CharacterLocation.Left1 => screenWidth * -0.15f,
            CharacterLocation.Center => 0f,
            CharacterLocation.Right1 => screenWidth * 0.15f,
            CharacterLocation.Right2 => screenWidth * 0.33f,
            _ => 0f
        };
    }
}
