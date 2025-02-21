using System;
using TMPro;
using UniRx;
using UnityEngine;

public class LogText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _logText; // ✅ UI 표시용 TextMeshProUGUI
    private IDisposable _placeStateSubscription;

    private void Start()
    {
        if (_logText == null)
        {
            Debug.LogError("[LogText] UI TextMeshProUGUI is not assigned!");
            return;
        }

        // ✅ PlaceManager의 PlaceStateNotifier 구독
        _placeStateSubscription = PlaceManager.Instance.PlaceStateNotifier
            .Subscribe(newState =>
            {
                _logText.text = $"PlaceState: {newState}"; // ✅ UI 업데이트
                Debug.Log($"[LogText] PlaceState changed to: {newState}");
            })
            .AddTo(this); // ✅ UniRx 자동 해제
    }

    private void OnDestroy()
    {
        _placeStateSubscription?.Dispose(); // ✅ 메모리 누수 방지
    }
}
