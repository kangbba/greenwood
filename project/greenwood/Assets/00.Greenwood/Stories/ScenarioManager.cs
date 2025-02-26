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

    public async UniTask ExecuteOneScenarioFromList(
        IReadOnlyList<Scenario> scenarioList,
        Action onBeforeStart = null,
        Action onAfterEnd = null
    )
    {
        if (_isScenarioPlayingNotifier.Value)
        {
            Debug.LogWarning("[ScenarioManager] A scenario is already running. Skipping execution.");
            return;
        }

        if (scenarioList == null || scenarioList.Count == 0)
        {
            Debug.LogWarning("[ScenarioManager] scenarioList is null or empty. No scenario to execute.");
            return;
        }

        // ✅ 랜덤으로 시나리오 선택
        int randomIndex = UnityEngine.Random.Range(0, scenarioList.Count);
        var chosenScenario = scenarioList[randomIndex];

        // 시작 전 콜백
        onBeforeStart?.Invoke();

        _isScenarioPlayingNotifier.Value = true;
        Debug.Log($"[ScenarioManager] Starting Scenario (Random): {chosenScenario.ScenarioId}");

        await chosenScenario.ExecuteAsync(); // 실제 시나리오 실행

        _isScenarioPlayingNotifier.Value = false;
        Debug.Log($"[ScenarioManager] Scenario Finished: {chosenScenario.ScenarioId}");

        // 종료 후 콜백
        onAfterEnd?.Invoke();
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
