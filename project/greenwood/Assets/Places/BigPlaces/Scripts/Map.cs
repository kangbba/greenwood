using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using UniRx;

public class Map : MonoBehaviour
{
    [SerializeField] private List<MoveBigPlaceBtn> _movePlaceButtons; // ✅ 사전 바인딩된 버튼 목록
    [SerializeField] private Button _confirmButton; // ✅ 결정 버튼

    private ReactiveProperty<EBigPlaceName> _selectedPlace = new ReactiveProperty<EBigPlaceName>(default);

    private void Awake()
    {
        // ✅ 각 버튼을 초기화하면서 선택 이벤트 설정
        foreach (var btn in _movePlaceButtons)
        {
            btn.Init(placeName => _selectedPlace.Value = placeName);
        }
    }

    /// <summary>
    /// ✅ 선택 가능한 BigPlace 버튼 활성화
    /// </summary>
    public void InitMap(List<EBigPlaceName> activePlaces)
    {
        foreach (var btn in _movePlaceButtons)
        {
            bool isEnabled = activePlaces.Contains(btn.BigPlaceName);
            btn.SetEnable(isEnabled);
        }
    }

    /// <summary>
    /// ✅ 플레이어가 선택할 때까지 대기
    /// </summary>
    public async UniTask<EBigPlaceName> WaitForPlaceSelection()
    {
        _selectedPlace.Value = default;

        // ✅ 결정 버튼을 누를 때까지 대기
        await _confirmButton.OnClickAsObservable()
            .Where(_ => _selectedPlace.Value != default)
            .First()
            .ToUniTask();

        return _selectedPlace.Value;
    }
}
