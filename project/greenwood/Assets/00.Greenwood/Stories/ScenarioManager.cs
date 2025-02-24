using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
    public static ScenarioManager Instance { get; private set; }

    [Header("스토리 매핑")]
    [SerializeField] private List<ScenarioMapping> _scenarioMappings;

    private ReactiveProperty<bool> _isScenarioPlayingNotifier = new ReactiveProperty<bool>(false);
    public IReadOnlyReactiveProperty<bool> IsScenarioPlayingNotifier => _isScenarioPlayingNotifier;

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
    /// ✅ 외부에서 호출하여 스토리 체크를 실행하는 메서드 (시작 전/후 콜백 추가)
    /// </summary>
    public async UniTask TriggerScenarioIfExist(Action onBeforeStart = null, Action onAfterEnd = null)
    {
        if (_isScenarioPlayingNotifier.Value) 
        {
            Debug.LogWarning("[ScenarioManager] Scenario is already running. Skipping execution.");
            return; // ✅ 실행 중이면 즉시 종료
        }

        BigPlace bigPlace = BigPlaceManager.Instance.CurrentBigPlaceNotifier.Value;
        SmallPlace smallPlace = SmallPlaceManager.Instance.CurrentSmallPlaceNotifier.Value;
        int currentDay = TimeManager.Instance.CurrentDayNotifier.Value;
        TimePhase currentTimePhase = TimeManager.Instance.CurrentTimePhaseNotifier.Value;

        Debug.Log($"[ScenarioManager] Triggering Scenario Check...");
        Debug.Log($"- BigPlace: {(bigPlace != null ? bigPlace.BigPlaceName.ToString() : "None")}");
        Debug.Log($"- SmallPlace: {(smallPlace != null ? smallPlace.SmallPlaceName.ToString() : "None")}");
        Debug.Log($"- Current Day: {currentDay}");
        Debug.Log($"- TimePhase: {currentTimePhase}");

        string scenarioName = GetMatchingScenarioName(bigPlace, smallPlace, currentDay, currentTimePhase);
        if (string.IsNullOrEmpty(scenarioName))
        {
            Debug.LogWarning("[ScenarioManager] scenarioName is null");
            return; // ✅ 실행 중이면 즉시 종료

        }
            
        // ✅ 시작 전 콜백 실행
        onBeforeStart?.Invoke();
        Debug.Log($"[ScenarioManager] Executing Scenario: {scenarioName}");
        await ExecuteScenario(scenarioName); // ✅ 스토리 실행을 기다림
        // ✅ 스토리 종료 후 콜백 실행
        onAfterEnd?.Invoke();
    }


    /// <summary>
    /// 현재 BigPlace, SmallPlace, 날짜, 시간 정보를 기반으로 스토리를 찾음
    /// </summary>
    private string GetMatchingScenarioName(BigPlace bigPlace, SmallPlace smallPlace, int currentDay, TimePhase currentTimePhase)
    {
        foreach (var mapping in _scenarioMappings)
        {
            bool isMatching = mapping.IsMatching(bigPlace, smallPlace, currentDay, currentTimePhase);
            if (isMatching)
            {
                Debug.Log($"[ScenarioManager] ✅ Matched Scenario: {mapping.ScenarioName}");
                return mapping.ScenarioName;
            }
        }

        Debug.Log("[ScenarioManager] ❌ No matching scenario found.");
        return null;
    }

    /// <summary>
    /// 스토리 실행 (비동기)
    /// </summary>
    private async UniTask ExecuteScenario(string scenarioName)
    {
        _isScenarioPlayingNotifier.Value = true;
        Debug.Log($"[ScenarioManager] Starting Scenario: {scenarioName}");

        Scenario scenarioInstance = CreateScenarioInstance(scenarioName);
        if (scenarioInstance != null)
        {
            await scenarioInstance.ExecuteAsync();
        }
        else
        {
            Debug.LogWarning($"[ScenarioManager] Scenario '{scenarioName}' could not be instantiated.");
        }

        _isScenarioPlayingNotifier.Value = false;
        Debug.Log($"[ScenarioManager] Scenario Finished: {scenarioName}");
    }

    /// <summary>
    /// 스토리 이름을 기반으로 동적 생성
    /// </summary>
    private Scenario CreateScenarioInstance(string scenarioName)
    {
        try
        {
            Type scenarioType = Type.GetType(scenarioName);
            if (scenarioType != null && typeof(Scenario).IsAssignableFrom(scenarioType))
            {
                return (Scenario)Activator.CreateInstance(scenarioType);
            }
            else
            {
                Debug.LogWarning($"[ScenarioManager] Scenario class '{scenarioName}' does not exist or is not a valid Scenario type.");
                return null;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"[ScenarioManager] Error creating scenario '{scenarioName}': {ex.Message}");
            return null;
        }
    }

}
