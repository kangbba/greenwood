using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class PlaceUiManager : MonoBehaviour
{
    public static PlaceUiManager Instance { get; private set; }

    [SerializeField] private BottomUiPanel _placeBottomUI;
    [SerializeField] private Button _enterSmallPlaceBtnPrefab;
    [SerializeField] private Map _mapPrefab; // ✅ 맵 UI 프리팹 추가

    private List<Button> _enterPlaceBtns = new List<Button>();
    private Map _currentMapInstance; // ✅ 현재 활성화된 맵 인스턴스

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        Observable.CombineLatest(
            PlaceManager.Instance.CurrentBigPlace,
            PlaceManager.Instance.CurrentSmallPlace,
            (bigPlace, smallPlace) => (bigPlace, smallPlace)
        )
        .Subscribe(tuple =>
        {
            BigPlace bigPlace = tuple.bigPlace;
            SmallPlace smallPlace = tuple.smallPlace;

            Debug.Log($"[PlaceManager] UI Update - BigPlace: {bigPlace?.BigPlaceName}, SmallPlace: {smallPlace?.SmallPlaceName}");

            if (bigPlace == null && smallPlace == null)
            {
                Debug.Log("[PlaceManager] No active places, hiding all UI.");
                DestroyEnterPlaceBtns().Forget();
                _placeBottomUI.ClearButtons();
            }
            else if (bigPlace != null && smallPlace == null)
            {
                Debug.Log("[PlaceManager] Showing BigPlace UI.");
                CreateEnterPlaceBtns(bigPlace).Forget();

                _placeBottomUI.SetButtons(new Dictionary<BottomUiButtonType, System.Action>
                {
                    { BottomUiButtonType.GoingOut, async () => 
                        {
                            EBigPlaceName selectedPlace = await ShowMap();
                            if (selectedPlace != bigPlace.BigPlaceName)
                            {
                                await PlaceManager.Instance.MoveBigPlace(selectedPlace);
                            }
                        }
                    },
                    { BottomUiButtonType.Rest, () => TimeManager.Instance.ToTheNextDay() },
                });
            }
            else if (bigPlace != null && smallPlace != null)
            {
                _placeBottomUI.SetButtons(new Dictionary<BottomUiButtonType, System.Action>
                {
                    { BottomUiButtonType.ExitSmallPlace, () => PlaceManager.Instance.ExitSmallPlace() }
                });

                DestroyEnterPlaceBtns().Forget();
            }
        })
        .AddTo(this);
    }

    public async UniTask<EBigPlaceName> ShowMap()
    {
        if (_currentMapInstance != null)
        {
            Debug.LogWarning("[PlaceUiManager] Map is already active.");
            return default;
        }

        _currentMapInstance = Instantiate(_mapPrefab, UIManager.Instance.UICanvas.MapLayer);
        Debug.Log("[PlaceUiManager] Map UI opened.");

        // ✅ 맵 활성화 및 사용 가능한 장소 표시
        List<EBigPlaceName> availablePlaces = new List<EBigPlaceName>();
        foreach (var bigPlace in PlaceManager.Instance.BigPlacePrefabs)
        {
            availablePlaces.Add(bigPlace.BigPlaceName);
        }
        _currentMapInstance.InitMap(availablePlaces);

        // ✅ 선택이 완료될 때까지 대기
        EBigPlaceName selectedPlace = await _currentMapInstance.WaitForPlaceSelection();

        // ✅ 맵 닫기
        HideMap();
        return selectedPlace;
    }


    /// <summary>
    /// ✅ 맵을 숨기고 제거
    /// </summary>
    private void HideMap()
    {
        if (_currentMapInstance != null)
        {
            Destroy(_currentMapInstance.gameObject);
            _currentMapInstance = null;
            Debug.Log("[PlaceUiManager] Map UI closed.");
        }
    }

    private async UniTask CreateEnterPlaceBtns(BigPlace bigPlace)
    {
        foreach (var location in bigPlace.SmallPlaceLocations)
        {
            Debug.Log($"[PlaceUiManager] Creating Button for SmallPlace: {location.SmallPlaceName}");

            Button newButton = Instantiate(_enterSmallPlaceBtnPrefab, location.transform);
            newButton.transform.localPosition = Vector2.zero;
            newButton.gameObject.SetAnimToFrom(true, false, 1f);
            newButton.onClick.AddListener(() =>
            {
                Debug.Log($"[PlaceUiManager] EnterSmallPlace Button Clicked: {location.SmallPlaceName}");
                PlaceManager.Instance.EnterSmallPlace(location.SmallPlaceName);
            });
            _enterPlaceBtns.Add(newButton);
        }

        await UniTask.WaitForSeconds(1f);
    }

    private async UniTask DestroyEnterPlaceBtns()
    {
        Debug.Log("[PlaceUiManager] Destroying all EnterPlace Buttons...");

        foreach (var button in _enterPlaceBtns)
        {
            Debug.Log($"[PlaceUiManager] Destroying Button: {button.name}");
            button.gameObject.SetAnimDestroy(1f);
        }
        _enterPlaceBtns.Clear();

        await UniTask.WaitForSeconds(1f);
    }
}
