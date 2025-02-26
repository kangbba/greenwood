using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable]
public class EventResults
{
    [SerializeField, LabelText("이벤트 결과 리스트")]
    private List<EventResult> _eventResults = new List<EventResult>();

    /// <summary>
    /// ✅ 모든 EventResult 실행
    /// </summary>
    public void ExecuteAll()
    {
        if (_eventResults.Count == 0)
        {
            Debug.LogWarning("⚠ [EventResults] 실행할 이벤트가 없음!");
            return;
        }

        Debug.Log("🎬 [EventResults] 모든 이벤트 실행!");
        foreach (var result in _eventResults)
        {
            result.Execute();
        }
    }
}
