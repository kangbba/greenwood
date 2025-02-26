using System;
using System.Collections.Generic;
using System.Linq; // ✅ Linq 추가
using UniRx;
using UnityEngine;

[Serializable]
public class EventConditions
{
    [SerializeField] private List<EventCondition> _conditions = new List<EventCondition>(); // ✅ 다수의 조건 저장

    private BoolReactiveProperty _allCleared = new BoolReactiveProperty(false);
    public IReadOnlyReactiveProperty<bool> IsCleared => _allCleared;

    private CompositeDisposable _subscriptions = new CompositeDisposable();

    public void Initialize()
    {
        Dispose(); // ✅ 기존 구독 해제

        if (_conditions.Count == 0)
        {
            _allCleared.Value = true;
            Debug.Log("✅ [EventConditions] 조건이 없으므로 자동 클리어됨");
            return;
        }

        // ✅ 모든 조건을 감시하고, 하나라도 false이면 전체 false로 설정
        Observable.CombineLatest(_conditions.ConvertAll(condition =>
        {
            condition.Initialize();
            return condition.IsCleared;
        }))
        .Subscribe(results =>
        {
            bool allSatisfied = results.All(isCleared => isCleared);

            // ✅ 개별 조건 값 디버깅 로그 추가
            for (int i = 0; i < _conditions.Count; i++)
            {
                Debug.Log($"🔍 [EventConditions] 조건 {i}({_conditions[i].Type}) 결과: {results[i]}");
            }

            if (allSatisfied)
            {
                Debug.Log("🔥 [EventConditions] 조건 관찰중, 조건 충족!");
            }
            else
            {
                Debug.Log("⏳ [EventConditions] 조건 관찰중이지만 모두 충족하지 않음...");

                // ✅ 충족되지 않은 조건만 로그 출력
                for (int i = 0; i < _conditions.Count; i++)
                {
                    if (!results[i])
                    {
                        Debug.LogWarning($"🚨 [EventConditions] 충족되지 않은 조건: {_conditions[i].Type}, 값: {results[i]}");
                    }
                }
            }

            _allCleared.Value = allSatisfied;
        })
        .AddTo(_subscriptions);
    }


    public void Dispose()
    {
        _subscriptions.Clear();
        foreach (var condition in _conditions)
        {
            condition.Dispose();
        }
    }
}
