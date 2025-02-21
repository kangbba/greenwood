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

    private void Start()
    {
        // ✅ 장소 상태(PlaceState) + 날짜(Day) + 시간대(TimePhase)를 CombineLatest
        Observable.CombineLatest(
            PlaceManager.Instance.PlaceStateNotifier,  // ✅ 장소 상태 변경 감지
            TimeManager.Instance.CurrentDay,
            TimeManager.Instance.CurrentTimePhase,
            (placeState, currentDay, currentTimePhase) => (placeState, currentDay, currentTimePhase)
        )
        .Subscribe(tuple =>
        {
            if (_isStoryPlayingNotifier.Value) 
            {
                Debug.LogWarning("[StoryManager] Story is already running. Skipping execution.");
                return;
            }

            EPlaceState placeState = tuple.placeState;
            int currentDay = tuple.currentDay;
            TimePhase currentTimePhase = tuple.currentTimePhase;

            string storyName = GetMatchingStoryName(placeState, currentDay, currentTimePhase);
            if (!string.IsNullOrEmpty(storyName))
            {
                Debug.Log($"[StoryManager] Executing Story: {storyName}");
                ExecuteStory(storyName).Forget();
            }
        })
        .AddTo(this);
    }

    /// <summary>
    /// 현재 장소 상태와 시간 정보를 기반으로 스토리를 찾음
    /// </summary>
    private string GetMatchingStoryName(EPlaceState placeState, int currentDay, TimePhase currentTimePhase)
    {
        BigPlace bigPlace = PlaceManager.Instance.CurrentBigPlace; // 현재 BigPlace 가져오기
        SmallPlace smallPlace = (placeState == EPlaceState.InSmallPlace) ? PlaceManager.Instance.CurrentSmallPlace : null; // SmallPlace는 상태가 InSmallPlace일 때만 가져옴

        foreach (var mapping in _storyMappings)
        {
            if (mapping.IsMatching(placeState, bigPlace, smallPlace, currentDay, currentTimePhase))
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
