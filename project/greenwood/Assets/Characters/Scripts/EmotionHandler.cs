using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor;

public class EmotionHandler : MonoBehaviour
{
    [FoldoutGroup("🎭 Emotion List")]
    [SerializeField, ListDrawerSettings(ShowFoldout = true)]
    private List<Emotion> _emotions = new();

    [EnumPaging, OnValueChanged(nameof(OnValueChangedCurrentEmotion))]
    [SerializeField]
    private KateEmotionType _previewEmotionType; // 유지

    private string _currentEmotionID; // 실제 감정 ID는 string으로 저장

    private void Awake()
    {
        FetchEmotions();
    }

    /// <summary>
    /// 인스펙터에서 `_currentEmotionType` 값이 바뀔 때마다 호출
    /// </summary>
    private void OnValueChangedCurrentEmotion()
    {
        FetchEmotions();
        SetEmotion(_previewEmotionType.ToString(), 0f);
        
#if UNITY_EDITOR
        EditorApplication.delayCall += () =>
        {
            EditorUtility.SetDirty(this);
            SceneView.RepaintAll();
        };
#endif
    }

    public void FetchEmotions()
    {
        _emotions = new List<Emotion>(GetComponentsInChildren<Emotion>(true));
    }

    public void SetEmotion(string emotionID, float duration)
    {
        if(emotionID == null){
            Debug.LogWarning("emotionID null");
            return;
        }

        Emotion newEmotion = GetEmotion(emotionID);
        if (newEmotion == null)
        {
            Debug.LogWarning($"[EmotionHandler] 감정 `{emotionID}`이(가) 존재하지 않습니다.");
            return;
        }
        if(_currentEmotionID == emotionID){
            Debug.LogWarning("emotionID already same");
            return;
        }

        _currentEmotionID = emotionID;

        foreach (var emo in _emotions)
            emo.gameObject.SetAnimActive(emo == newEmotion, duration);

        Debug.Log($"[EmotionHandler] 감정 변경: `{_currentEmotionID}`");
    }

    public Emotion GetEmotion(string emotionID)
    {
        return _emotions.Find(e => e.EmotionType.ToString() == emotionID);
    }

    public void PlayMouthWithCurrentEmotion(bool b)
    {
        Emotion currentEmotion = GetEmotion(_currentEmotionID);
        if (currentEmotion != null) 
            currentEmotion.PlayMouth(b);
    }

    public void PlayEyesWithCurrentEmotion(bool b)
    {
        Emotion currentEmotion = GetEmotion(_currentEmotionID);
        if (currentEmotion != null) 
            currentEmotion.PlayEyes(b);
    }
}
