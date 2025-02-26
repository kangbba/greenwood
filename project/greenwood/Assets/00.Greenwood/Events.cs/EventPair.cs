using System;
using UniRx;
using UnityEngine;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;

[Serializable]
public class EventPair
{
    [SerializeField, FoldoutGroup("이벤트 조건")]
    private EventConditions _eventConditions; // ✅ 이벤트 조건

    [SerializeField, FoldoutGroup("이벤트 결과")]
    private EventResults _eventResults; // ✅ 이벤트 결과

    /// <summary>
    /// ✅ 외부에서 구독 가능한 `IObservable<bool>`
    /// </summary>
    public IObservable<bool> IsSatisfiedAllStream() => _eventConditions.IsSatisfiedAllStream();

    /// <summary>
    /// ✅ 이벤트 실행 (조건 충족 시 호출)
    /// </summary>
    public void Execute()
    {
        if (_eventResults.IsSequential)
        {
            _eventResults.ExecuteAllSequentiallyAsync().Forget();
        }
        else
        {
            _eventResults.ExecuteAllInParallel();
        }
    }
}
