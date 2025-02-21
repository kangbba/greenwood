using TMPro;
using UniRx;
using UnityEngine;

public class TimeDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timeText; // ✅ UI 텍스트 (TMP)

    private void Start()
    {
        // ✅ 현재 날짜와 시간대 정보를 구독하여 UI에 업데이트
        Observable.CombineLatest(
            TimeManager.Instance.CurrentDay,
            TimeManager.Instance.CurrentTimePhase,
            (day, timePhase) => $"Day {day}, {timePhase}"
        )
        .Subscribe(timeText =>
        {
            _timeText.text = timeText;
            Debug.Log($"[TimeDisplayer] Updated Time Display: {timeText}");
        })
        .AddTo(this);
    }
}
