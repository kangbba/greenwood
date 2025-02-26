using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

[Serializable]
public class EventResults
{
    [SerializeField] private List<EventResult> _eventResults = new List<EventResult>();
    [SerializeField] private bool _isSequential; // ✅ 기본값: 순차 실행

    public bool IsSequential => _isSequential; // ✅ 실행 방식 외부에서 접근 가능

    /// <summary>
    /// ✅ 모든 이벤트 결과를 순차 실행
    /// </summary>
    public async UniTask ExecuteAllSequentiallyAsync()
    {
        foreach (var eventResult in _eventResults)
        {
            await eventResult.ExecuteAsync();
        }
    }

    /// <summary>
    /// ✅ 모든 이벤트 결과를 동시에 실행
    /// </summary>
    public void ExecuteAllInParallel()
    {
        foreach (var eventResult in _eventResults)
        {
            eventResult.ExecuteAsync().Forget();
        }
    }
}
