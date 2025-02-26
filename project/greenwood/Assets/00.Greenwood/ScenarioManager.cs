using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
    public static ScenarioManager Instance { get; private set; }

    private ReactiveProperty<bool> _isScenarioPlaying = new ReactiveProperty<bool>(false);
    public IReadOnlyReactiveProperty<bool> IsScenarioPlaying => _isScenarioPlaying;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    public async UniTask ExecuteOneScenario(
        Scenario scenario,
        Action onBeforeStart = null,
        Action onAfterEnd = null
    )
    {
        if (_isScenarioPlaying.Value)
        {
            Debug.LogWarning("[ScenarioManager] A scenario is already running. Skipping execution.");
            return;
        }

        if (scenario == null)
        {
            Debug.LogWarning("[ScenarioManager] scenario is null. No scenario to execute.");
            return;
        }

        // 시작 전 콜백
        onBeforeStart?.Invoke();

        _isScenarioPlaying.Value = true;
        Debug.Log($"[ScenarioManager] Starting Scenario: {scenario.ScenarioId}");

        await scenario.ExecuteAsync(); // ✅ 실제 시나리오 실행

        _isScenarioPlaying.Value = false;
        Debug.Log($"[ScenarioManager] Scenario Finished: {scenario.ScenarioId}");

        // 종료 후 콜백
        onAfterEnd?.Invoke();
    }

    public async UniTask ExecuteOneScenarioFromList(
        IReadOnlyList<Scenario> scenarioList,
        Action onBeforeStart = null,
        Action onAfterEnd = null
    )
    {
        if (_isScenarioPlaying.Value)
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

        _isScenarioPlaying.Value = true;
        Debug.Log($"[ScenarioManager] Starting Scenario (Random): {chosenScenario.ScenarioId}");

        await chosenScenario.ExecuteAsync(); // 실제 시나리오 실행

        _isScenarioPlaying.Value = false;
        Debug.Log($"[ScenarioManager] Scenario Finished: {chosenScenario.ScenarioId}");

        // 종료 후 콜백
        onAfterEnd?.Invoke();
    }


    /// <summary>
    /// 스토리 실행 (비동기)
    /// </summary>
    public async UniTask ExecuteScenario(string scenarioName)
    {
        _isScenarioPlaying.Value = true;
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

        _isScenarioPlaying.Value = false;
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
