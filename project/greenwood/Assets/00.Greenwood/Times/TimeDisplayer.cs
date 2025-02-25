using TMPro;
using UniRx;
using UnityEngine;
using System;

public class TimeDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timeText; // ✅ UI 텍스트 (TMP)

    private static readonly DateTime StartDate = new DateTime(1991, 12, 1); // ✅ 시작일: 1991년 12월 1일

    private void Start()
    {
        // ✅ 현재 날짜와 시간대 정보를 구독하여 UI에 업데이트
        Observable.CombineLatest(
            TimeManager.Instance.CurrentDayNotifier,
            TimeManager.Instance.CurrentTimePhaseNotifier,
            (day, timePhase) =>
            {
                DateTime currentDate = StartDate.AddDays(day - 1); // ✅ 1991년 12월 1일 기준으로 날짜 계산
                return $"{currentDate:yyyy년 MM월 dd일}, {timePhase}"; // ✅ 원하는 형식으로 출력
            }
        )
        .Subscribe(timeText =>
        {
            _timeText.text = timeText;
            Debug.Log($"[TimeDisplayer] Updated Time Display: {timeText}");
        })
        .AddTo(this);
    }
}
