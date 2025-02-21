using System;
using UnityEngine;
using static Character;
using Sirenix.OdinInspector;

public class Emotion : AnimationImage
{
    [EnumPaging, OnValueChanged(nameof(OnValueChangedCurrentEmotion))]
    [SerializeField] private KateEmotionType _emotionType; // Inspector에서 직접 설정
    public KateEmotionType EmotionType => _emotionType;

    [SerializeField] private Eyes _eyes;
    [SerializeField] private Mouth _mouth;
    [SerializeField] private Cheek _cheek;

    private void Awake()
    {
        OnValueChangedCurrentEmotion(); // 초기 실행 시 GameObject 이름 동기화
    }

    public void Init()
    {
        PlayMouth(false);
        PlayEyes(true);

        if (_cheek != null)
        {
            _cheek.SetFlush(false, 0f);
            _cheek.SetFlush(true, 2f);
        }
    }

    public void PlayMouth(bool isActive)
    {
        if (isActive)
        {
            _mouth.Play().Forget();
        }
        else
        {
            _mouth.Stop();
        }
    }

    private void PlayEyes(bool isActive)
    {
        if (isActive)
        {
            _eyes.Play().Forget();
        }
        else
        {
            _eyes.Stop();
        }
    }

    /// <summary>
    /// Inspector에서 EmotionType 변경 시 GameObject 이름 자동 변경
    /// </summary>
    private void OnValueChangedCurrentEmotion()
    {
        gameObject.name = $"{_emotionType}";
    }
}
