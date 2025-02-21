using TMPro;
using UniRx;
using UnityEngine;

public class LogText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _logText; // ✅ UI 표시용 TextMeshProUGUI

    private void Start()
    {
        if (_logText == null)
        {
            Debug.LogError("[LogText] UI TextMeshProUGUI is not assigned!");
            return;
        }

        // ✅ 초기 UI 텍스트 설정
        UpdateLogText(PlaceManager.Instance.CurrentBigPlaceNotifier.Value, PlaceManager.Instance.CurrentSmallPlaceNotifier.Value);

        // ✅ BigPlaceNotifier & SmallPlaceNotifier 변경 감지
        Observable.CombineLatest(
            PlaceManager.Instance.CurrentBigPlaceNotifier,
            PlaceManager.Instance.CurrentSmallPlaceNotifier,
            (bigPlace, smallPlace) => (bigPlace, smallPlace)
        )
        .Subscribe(tuple =>
        {
            UpdateLogText(tuple.bigPlace, tuple.smallPlace);
        })
        .AddTo(this); // ✅ UniRx 자동 해제
    }

    /// <summary>
    /// UI 텍스트 업데이트
    /// </summary>
    private void UpdateLogText(BigPlace bigPlace, SmallPlace smallPlace)
    {
        _logText.text = $"BigPlace: {(bigPlace != null ? bigPlace.BigPlaceName.ToString() : "None")}\n" +
                        $"SmallPlace: {(smallPlace != null ? smallPlace.SmallPlaceName.ToString() : "None")}";

        Debug.Log($"[LogText] Updated Log: {_logText.text}");
    }
}
