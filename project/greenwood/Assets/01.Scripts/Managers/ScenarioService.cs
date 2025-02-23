using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public static class ScenarioService
{
    public static async UniTask ExecuteScenarioSequence(Scenario scenario)
    {
        while (scenario != null)
        {
            Debug.Log($"🚀 Executing Scenario: {scenario.ScenarioId}");

            await ExecuteElementsSequence(scenario.UpdateElements);

            scenario = scenario.NextScenario;
        }

        Debug.Log("✅ All stories have been executed.");
    }

    private static async UniTask ExecuteElementsSequence(List<Element> elements)
    {
        foreach (Element element in elements)
        {
            await element.ExecuteAsync();
        }
    }

    public static void SkipScenario(Scenario scenario)
    {
        if (scenario != null)
        {
            Debug.Log($"⏩ Skipping Scenario: {scenario.ScenarioId}");
            scenario.ExecuteInstantlyAll();
        }
    }

    public static void SkipToElementIndex(Scenario scenario, int index)
    {
        if (scenario != null)
        {
            Debug.Log($"⏩ Skipping {scenario.ScenarioId} to Element Index {index}");
            scenario.ExecuteInstantlyTillElementIndex(index);
        }
    }
}
