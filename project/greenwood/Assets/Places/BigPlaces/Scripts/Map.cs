using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;

public class Map : MonoBehaviour
{
    [SerializeField] private List<MoveBigPlaceBtn> _movePlaceButtons; // ✅ 사전 바인딩된 버튼 목록
    [SerializeField] private Button _confirmButton; // ✅ 확정 버튼
    [SerializeField] private Button _cancelButton; // ✅ 취소 버튼 추가

    private ReactiveProperty<EBigPlaceName?> _selectedPlace = new ReactiveProperty<EBigPlaceName?>(null);
    private UniTaskCompletionSource<EBigPlaceName?> _mapCompletionSource; // ✅ 맵 결과를 반환할 Task

    private void Awake()
    {
        foreach (var btn in _movePlaceButtons)
        {
            btn.Init(placeName => _selectedPlace.Value = placeName);
        }

        _selectedPlace.Subscribe(place =>
        {
            _confirmButton.interactable = place.HasValue;
        })
        .AddTo(this);

        _confirmButton.onClick.AddListener(() => ConfirmSelection());
        _cancelButton.onClick.AddListener(() => CancelSelection());
    }

    /// <summary>
    /// ✅ 맵 UI를 초기화하고 선택 가능한 버튼 활성화
    /// </summary>
    public void InitMap(List<EBigPlaceName> activePlaces)
    {
        foreach (var btn in _movePlaceButtons)
        {
            bool isEnabled = activePlaces.Contains(btn.BigPlaceName);
            btn.SetEnable(isEnabled);
        }

        _selectedPlace.Value = null;
        _confirmButton.interactable = false;
    }


    /// <summary>
    /// ✅ 맵을 띄우고 선택된 장소를 반환
    /// </summary>
    public async UniTask<EBigPlaceName?> ShowMap()
    {
        _mapCompletionSource = new UniTaskCompletionSource<EBigPlaceName?>();

        gameObject.SetAnimActive(true, 0.3f);
        return await _mapCompletionSource.Task;
    }

    private void ConfirmSelection()
    {
        if (_selectedPlace.Value.HasValue)
        {
            _mapCompletionSource.TrySetResult(_selectedPlace.Value.Value);
            CloseMap();
        }
    }

    private void CancelSelection()
    {
        _mapCompletionSource.TrySetResult(null);
        CloseMap();
    }

    private void CloseMap()
    {
        gameObject.SetAnimActive(false, 0.3f);
    }
}
