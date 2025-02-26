using System;
using UnityEngine;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;

[Serializable]
public class EventResult
{
    [SerializeField, LabelText("실행할 시나리오 이름")]
    private string _scenarioName; // ✅ 실행할 시나리오 이름

    public void Execute()
    {
        if (!string.IsNullOrEmpty(_scenarioName))
        {
            Debug.Log($"▶ [EventResult] Scenario 실행: {_scenarioName}");
            ScenarioManager.Instance.ExecuteScenario(_scenarioName).Forget();
        }
        else
        {
            Debug.LogWarning($"⚠ [EventResult] 실행할 Scenario가 설정되지 않음!");
        }
    }
}
