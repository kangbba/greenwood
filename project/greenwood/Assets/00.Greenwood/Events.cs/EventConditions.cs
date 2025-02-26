using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

[Serializable]
public class EventConditions
{
    [SerializeField] private List<EventCondition> _conditions = new List<EventCondition>(); // ✅ 다수의 조건 저장

    public IObservable<bool> IsSatisfiedAllStream()
    {
        if (_conditions.Count == 0)
        {
            Debug.Log("✅ [EventConditions] 조건이 없으므로 자동으로 클리어됨");
            return Observable.Return(true); // ✅ 조건이 없으면 자동으로 true 반환
        }

        return Observable.CombineLatest(_conditions.Select(condition => condition.IsSatisfiedStream()))
            .Select(results =>
            {
                bool allSatisfied = results.All(isSatisfied => isSatisfied);

                // ✅ 로그를 한 번만 출력하도록 문자열로 구성
                string logMessage = $"🔎 [EventConditions] 총 {results.Count}개의 조건 감지됨. (현재 만족도: {results.Count(r => r)}/{results.Count})\n";

                for (int i = 0; i < _conditions.Count; i++)
                {
                    logMessage += $"   🔍 조건 {i + 1}: {_conditions[i].GetConditionDescription()} ▶ 결과: {(results[i] ? "✅ 충족됨" : "❌ 미충족")}\n";
                }

                if (allSatisfied)
                {
                    logMessage += "🎉 [EventConditions] 모든 조건 충족! 이벤트 실행 가능!";
                }
                else
                {
                    logMessage += "⏳ [EventConditions] 일부 조건이 충족되지 않음.\n";

                    // ✅ 미충족된 조건만 따로 로그 추가
                    string unmetConditions = _conditions
                        .Where((condition, index) => !results[index])
                        .Select(condition => $"🚨 미충족 조건: {condition.GetConditionDescription()} (❌ 불충족)")
                        .Aggregate("", (current, next) => current + next + "\n");

                    logMessage += unmetConditions;
                }

                Debug.Log(logMessage.TrimEnd()); // ✅ 최종 로그 한 번만 출력
                return allSatisfied;
            });
    }
}
