using UniRx;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    public ReactiveProperty<int> CurrentDay { get; private set; } = new ReactiveProperty<int>(1); // 1일부터 시작
    public ReactiveProperty<TimePhase> CurrentTimePhase { get; private set; } = new ReactiveProperty<TimePhase>(TimePhase.Morning);

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
    /// 현재 날짜와 시간대를 설정
    /// </summary>
    public void SetTime(int day, TimePhase phase)
    {
        if (day < 1)
        {
            Debug.LogWarning("[TimeManager] Day는 1 이상이어야 합니다.");
            return;
        }

        Debug.Log($"[TimeManager] 시간 변경: Day {day}, Phase {phase}");

        CurrentDay.Value = day;
        CurrentTimePhase.Value = phase;
    }

    /// <summary>
    /// ✅ 하루를 넘겨 다음 날로 이동 (아침으로 초기화)
    /// </summary>
    public void ToTheNextDay()
    {
        int nextDay = CurrentDay.Value + 1;
        SetTime(nextDay, TimePhase.Morning);
        Debug.Log($"[TimeManager] 다음 날로 이동: Day {nextDay}, Phase Morning");
    }
}

/// <summary>
/// 하루의 시간대 Enum (아침, 낮, 저녁, 밤)
/// </summary>
public enum TimePhase
{
    Morning,
    Afternoon,
    Evening,
    Night
}
