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
    [SerializeField] private Map _mapPrefab;

    private List<Button> _enterPlaceBtns = new List<Button>();
    private Map _currentMapInstance;

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
    {   // ✅ StoryManager의 IsStoryPlaying 값을 구독하여 자동으로 UI 표시/숨김 처리
        StoryManager.Instance.IsStoryPlayingNotifier
            .Subscribe(isPlaying =>
            {
                if (isPlaying)
                {
                    Debug.Log("[PlaceUiManager] Story started → Hiding UI");
                    _placeBottomUI.Hide();
                }
                else
                {
                    Debug.Log("[PlaceUiManager] Story ended → Showing UI");
                    _placeBottomUI.Show();
                }
            })
            .AddTo(this);

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
                Debug.Log("");
                DestroyEnterPlaceBtns().Forget();
                _placeBottomUI.ClearButtons();
            }
            else if (bigPlace != null && smallPlace == null)
            {
                Debug.Log("빅 플레이스 입장");
                CreateEnterPlaceBtns(bigPlace).Forget();

                var btnDictionary = new Dictionary<BottomUiButtonType, System.Action>
                {
                    { BottomUiButtonType.GoingOut, async () =>
                        {
                            _placeBottomUI.Hide();
                            await CreateMapAndShow(bigPlace); // ✅ 맵 선택 결과 대기
                            _placeBottomUI.Show();
                        }
                    },
                    { BottomUiButtonType.Rest, () => TimeManager.Instance.ToTheNextDay() },
                };
                
               _placeBottomUI.SetButtons(btnDictionary);

            }
            else if (bigPlace != null && smallPlace != null)
            {
                Debug.Log("스몰 플레이스 입장");
                _placeBottomUI.SetButtons(new Dictionary<BottomUiButtonType, System.Action>
                {
                    { BottomUiButtonType.ExitSmallPlace, () => PlaceManager.Instance.ExitSmallPlace() }
                });

                DestroyEnterPlaceBtns().Forget();
            }
        })
        .AddTo(this);
    }
   
   private async UniTask CreateMapAndShow(BigPlace currentBigPlace)
    {
        _currentMapInstance = Instantiate(_mapPrefab, UIManager.Instance.UICanvas.MapLayer);

        List<EBigPlaceName> availablePlaces = new List<EBigPlaceName>();
        foreach (var bigPlace in PlaceManager.Instance.BigPlacePrefabs)
        {
            availablePlaces.Add(bigPlace.BigPlaceName);
        }
        _currentMapInstance.InitMap(availablePlaces);

        // ✅ `ShowMap()`이 단순히 장소만 반환하도록 유지
        EBigPlaceName? selectedPlace = await _currentMapInstance.ShowMap();

        // ✅ 맵 닫기
        Destroy(_currentMapInstance.gameObject);
        _currentMapInstance = null;


        if (selectedPlace.HasValue && selectedPlace.Value != currentBigPlace.BigPlaceName)
        {
            Debug.Log($"[PlaceUiManager] Moving to {selectedPlace.Value}");
            await PlaceManager.Instance.MoveBigPlace(selectedPlace.Value);
        }
        else
        {
            Debug.Log("[PlaceUiManager] Map selection cancelled");
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
            button.gameObject.SetAnimDestroy(1f);
        }
        _enterPlaceBtns.Clear();

        await UniTask.WaitForSeconds(1f);
    }
}
