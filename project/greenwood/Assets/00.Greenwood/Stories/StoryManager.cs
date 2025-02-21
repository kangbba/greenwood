using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance { get; private set; }

    [Header("스토리 매핑")]
    [SerializeField] private List<StoryMapping> _storyMappings;

    private ReactiveProperty<bool> _isStoryPlayingNotifier = new ReactiveProperty<bool>(false);
    public IReadOnlyReactiveProperty<bool> IsStoryPlayingNotifier => _isStoryPlayingNotifier;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

   /// <summary>
    /// 외부에서 호출하여 스토리 체크를 실행하는 메서드
    /// </summary>
    public async UniTask TriggerStoryIfExist()
    {
        if (_isStoryPlayingNotifier.Value) 
        {
            Debug.LogWarning("[StoryManager] Story is already running. Skipping execution.");
            return; // ✅ 실행 중이면 즉시 종료
        }

        BigPlace bigPlace = PlaceManager.Instance.CurrentBigPlaceNotifier.Value;
        SmallPlace smallPlace = PlaceManager.Instance.CurrentSmallPlaceNotifier.Value;
        int currentDay = TimeManager.Instance.CurrentDay.Value;
        TimePhase currentTimePhase = TimeManager.Instance.CurrentTimePhase.Value;

        Debug.Log($"[StoryManager] Triggering Story Check...");
        Debug.Log($"- BigPlace: {(bigPlace != null ? bigPlace.BigPlaceName.ToString() : "None")}");
        Debug.Log($"- SmallPlace: {(smallPlace != null ? smallPlace.SmallPlaceName.ToString() : "None")}");
        Debug.Log($"- Current Day: {currentDay}");
        Debug.Log($"- TimePhase: {currentTimePhase}");

        string storyName = GetMatchingStoryName(bigPlace, smallPlace, currentDay, currentTimePhase);
        if (!string.IsNullOrEmpty(storyName))
        {
            Debug.Log($"[StoryManager] Executing Story: {storyName}");
            await ExecuteStory(storyName); // ✅ 스토리 실행을 기다림
        }

        await UniTask.WaitUntil(() => !_isStoryPlayingNotifier.Value); // ✅ 스토리 종료 대기

    }


    /// <summary>
    /// 현재 BigPlace, SmallPlace, 날짜, 시간 정보를 기반으로 스토리를 찾음
    /// </summary>
    private string GetMatchingStoryName(BigPlace bigPlace, SmallPlace smallPlace, int currentDay, TimePhase currentTimePhase)
    {
        foreach (var mapping in _storyMappings)
        {
            bool isMatching = mapping.IsMatching(bigPlace, smallPlace, currentDay, currentTimePhase);
            if (isMatching)
            {
                Debug.Log($"[StoryManager] ✅ Matched Story: {mapping.StoryName}");
                return mapping.StoryName;
            }
        }

        Debug.Log("[StoryManager] ❌ No matching story found.");
        return null;
    }

    /// <summary>
    /// 스토리 실행 (비동기)
    /// </summary>
    private async UniTask ExecuteStory(string storyName)
    {
        _isStoryPlayingNotifier.Value = true;
        Debug.Log($"[StoryManager] Starting Story: {storyName}");

        Story storyInstance = CreateStoryInstance(storyName);
        if (storyInstance != null)
        {
            await StoryService.ExecuteStorySequence(storyInstance);
        }
        else
        {
            Debug.LogWarning($"[StoryManager] Story '{storyName}' could not be instantiated.");
        }

        _isStoryPlayingNotifier.Value = false;
        Debug.Log($"[StoryManager] Story Finished: {storyName}");
    }

    /// <summary>
    /// 스토리 이름을 기반으로 동적 생성
    /// </summary>
    private Story CreateStoryInstance(string storyName)
    {
        try
        {
            Type storyType = Type.GetType(storyName);
            if (storyType != null && typeof(Story).IsAssignableFrom(storyType))
            {
                return (Story)Activator.CreateInstance(storyType);
            }
            else
            {
                Debug.LogWarning($"[StoryManager] Story class '{storyName}' does not exist or is not a valid Story type.");
                return null;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"[StoryManager] Error creating story '{storyName}': {ex.Message}");
            return null;
        }
    }
}
