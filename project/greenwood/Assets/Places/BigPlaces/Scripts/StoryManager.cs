using System;
using System.Collections.Generic;
using System.Linq;
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

    private void Start()
    {
        // ✅ BigPlace + SmallPlace + Day + TimePhase을 CombineLatest
        Observable.CombineLatest(
            PlaceManager.Instance.CurrentBigPlace,
            PlaceManager.Instance.CurrentSmallPlace,
            TimeManager.Instance.CurrentDay,
            TimeManager.Instance.CurrentTimePhase,
            (bigPlace, smallPlace, currentDay, currentTimePhase) => (bigPlace, smallPlace, currentDay, currentTimePhase) // ✅ 튜플 변환
        )
        .Subscribe(tuple =>
        {
            if (_isStoryPlayingNotifier.Value) 
            {
                Debug.LogWarning("[StoryManager] Story is already running. Skipping execution.");
                return;
            }

            BigPlace bigPlace = tuple.bigPlace;
            SmallPlace smallPlace = tuple.smallPlace;
            int currentDay = tuple.currentDay;
            TimePhase currentTimePhase = tuple.currentTimePhase;

            Debug.Log($"[StoryManager] Checking Story for Location: {bigPlace?.BigPlaceName}, {smallPlace?.SmallPlaceName}, Day: {currentDay}, Time: {currentTimePhase}");

            string storyName = GetMatchingStoryName(bigPlace, smallPlace, currentDay, currentTimePhase);
            if (!string.IsNullOrEmpty(storyName))
            {
                Debug.Log($"[StoryManager] Executing Story: {storyName}");
                ExecuteStory(storyName).Forget();
            }
        })
        .AddTo(this);
    }

    /// <summary>
    /// 현재 BigPlace, SmallPlace, Day, TimePhase에 맞는 스토리를 찾음
    /// </summary>
    private string GetMatchingStoryName(BigPlace bigPlace, SmallPlace smallPlace, int currentDay, TimePhase currentTimePhase)
    {
        foreach (var mapping in _storyMappings)
        {
            if (mapping.IsMatching(bigPlace, smallPlace, currentDay, currentTimePhase))
            {
                Debug.Log($"[StoryManager] Matched Story: {mapping.storyName}");
                return mapping.storyName;
            }
        }

        Debug.Log("[StoryManager] No matching story found.");
        return null;
    }

    /// <summary>
    /// 스토리 실행 (비동기)
    /// </summary>
    private async UniTask ExecuteStory(string storyName)
    {
        _isStoryPlayingNotifier.Value = true; // ✅ 실행 시작
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

        _isStoryPlayingNotifier.Value = false; // ✅ 실행 종료
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
