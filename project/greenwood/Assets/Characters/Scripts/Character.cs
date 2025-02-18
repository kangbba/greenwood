using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

/// <summary>
/// 캐릭터의 전체적인 상태(Idle, Talking 등)
/// </summary>

public class Character : MonoBehaviour
{
    [SerializeField] private ECharacterName _characterName;
    public ECharacterName CharacterName => _characterName;

    [SerializeField] private EmotionHandler _emotionHandler;
    [SerializeField] private PoseHandler _poseHandler;
    public EmotionHandler EmotionHandler => _emotionHandler;
    public PoseHandler PoseHandler => _poseHandler;



    private void Awake()
    {
        // 필요 시 Fetch
        _emotionHandler?.FetchEmotions();
        _poseHandler?.FetchPoses();
    }
    public void Init(string initialEmotionID, string initialPoseID, float duration){
        gameObject.SetAnim(false, 0f);
        SetEmotion(initialEmotionID, 0f);
        SetPose(initialPoseID, 0f);
        PlayEyesWithCurrentEmotion(true);
        gameObject.SetAnim(true, duration);
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
    // 원하는 경우, Character가 직접 pass-through 메서드 제공
    public void PlayEyesWithCurrentEmotion(bool b)
    {
        _emotionHandler?.PlayEyesWithCurrentEmotion(b);
    }

}
