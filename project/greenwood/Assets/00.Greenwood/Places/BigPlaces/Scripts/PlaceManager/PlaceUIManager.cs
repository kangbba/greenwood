using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using static SmallPlaceNames;
using static BigPlaceNames;

public class PlaceUIManager : MonoBehaviour
{
    public static PlaceUIManager Instance { get; private set; }

    [SerializeField] private ButtonGroup bottomButtonGroupPrefab;
    [SerializeField] private ButtonGroup leftButtonGroupPrefab;
    [SerializeField] private ButtonGroup centerButtonGroupPrefab; // ✅ CenterButtonGroup 추가
    [SerializeField] private Map _mapPrefab;

    private ButtonGroup _currentBottomButtonGroup;
    private ButtonGroup _currentLeftButtonGroup;
    private ButtonGroup _currentCenterButtonGroup;
    private Map _currentMap;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    /// <summary>
    /// ✅ SmallPlace에 따라 다른 버튼 그룹을 반환
    /// </summary>
    private Dictionary<string, Action> GetSmallPlaceButtonGroup(ESmallPlaceName? smallPlace)
    {
        if (!smallPlace.HasValue) return new Dictionary<string, Action> { { "GoingOut", () => CreateAndShowMap() } };

        switch (smallPlace.Value)
        {
            case ESmallPlaceName.Bakery:
                return new Dictionary<string, Action>
                {
                    { "Talk", () => Debug.Log("Talk at the Bakery") },
                    { "Buy", () => Debug.Log("Buying at the Bakery") },
                    { "Exit", () => SmallPlaceManager.Instance.ExitSmallPlace(.3f) }
                };

            case ESmallPlaceName.Herbshop:
                return new Dictionary<string, Action>
                {
                    { "Talk", () => Debug.Log("Talking in the Herbshop") },
                    { "Buy", () => Debug.Log("Buying herbs") },
                    { "Heal", () => Debug.Log("Healing at the Herbshop") },
                    { "Exit", () => SmallPlaceManager.Instance.ExitSmallPlace(.3f) }
                };

            case ESmallPlaceName.CafeSeabreeze:
                return new Dictionary<string, Action>
                {
                    { "Talk", () => Debug.Log("Talking in the Cafe") },
                    { "Order", () => Debug.Log("Ordering coffee") },
                    { "Exit", () => SmallPlaceManager.Instance.ExitSmallPlace(.3f) }
                };

            default:
                return new Dictionary<string, Action>
                {
                    { "Exit", () => SmallPlaceManager.Instance.ExitSmallPlace(.3f) }
                };
        }
    }
    private void Start()
    {

        InitializeButtonGroups();

        // ✅ BigPlace, SmallPlace의 변경을 감지하여 UI 업데이트
        BigPlaceManager.Instance.CurrentBigPlaceNotifier
            .CombineLatest(SmallPlaceManager.Instance.CurrentSmallPlaceNotifier, (bigPlace, smallPlace) => new { bigPlace, smallPlace })
            .Subscribe(placeState =>
            {
                if (placeState.bigPlace != null)
                {

                    if (placeState.smallPlace != null) // 스몰플레이스 입장
                    {
                        // ✅ CenterButtonGroup
                        _currentCenterButtonGroup.ClearButtons();
                        _currentCenterButtonGroup.FadeOut(.3f);

                        // ✅ LeftButtonGroup
                        Debug.Log("[PlaceUIManager] SmallPlace detected. Showing LeftPanel.");
                        _currentLeftButtonGroup.FadeIn(0.3f);
                        _currentLeftButtonGroup.SetButtonGroup(GetSmallPlaceButtonGroup(placeState.smallPlace.SmallPlaceName));

                        // ✅ BottomButtonGroup
                        Debug.Log("[PlaceUIManager] BigPlace detected. BottomPanel remains visible.");
                        _currentBottomButtonGroup.FadeOut(0.3f);
                    }
                    else // 스몰플레이스 퇴장
                    {
                        // ✅ CenterButtonGroup
                        UpdateCenterButtonGroup(placeState.bigPlace);
                        _currentCenterButtonGroup.FadeIn(.3f);

                        // ✅ LeftButtonGroup
                        Debug.Log("[PlaceUIManager] SmallPlace exited. Hiding LeftPanel.");
                        _currentLeftButtonGroup.FadeOut(0.3f);

                        // ✅ BottomButtonGroup을 표시
                        Debug.Log("[PlaceUIManager] BigPlace detected. BottomPanel remains visible.");
                        _currentBottomButtonGroup.FadeIn(0.3f);

                        // ✅ "이동" 버튼 추가 (null 방지)
                        _currentBottomButtonGroup.SetButtonGroup(new Dictionary<string, Action>
                        {
                            { "GoingOut", () => { 
//                                BigPlaceManager.Instance.MoveBigPlace(.3f);
                                CreateAndShowMap();
                                Debug.Log("GoingOut button clicked"); } 
                            }
                        });
                    }

                }
                else{

                }
            })
            .AddTo(this); // 자동 구독 해제
    }
    private async void CreateAndShowMap()
    {
        // ✅ 기존 맵이 존재하면 제거
        if (_currentMap != null)
        {
            Destroy(_currentMap.gameObject);
        }

        // ✅ 새로운 맵 생성
        _currentMap = Instantiate(_mapPrefab, UIManager.Instance.UICanvas.PlaceUiLayer);
        _currentMap.InitMap(useRestrict: false); // 전체 맵 표시 (제한 없음)

        // ✅ 맵을 표시하고 선택 결과 반환 대기
        EBigPlaceName? selectedPlace = await _currentMap.ShowMap();

        // ✅ 선택된 장소 처리
        if (selectedPlace.HasValue)
        {
            Debug.Log($"[PlaceUIManager] Moving to {selectedPlace.Value}");
            BigPlaceManager.Instance.MoveBigPlace(selectedPlace.Value, .5f);
        }
        else
        {
            Debug.Log("[PlaceUIManager] Map selection canceled.");
        }
    }

    private void InitializeButtonGroups()
    {
        // ✅ Button Groups 인스턴스화
        _currentLeftButtonGroup = Instantiate(leftButtonGroupPrefab, UIManager.Instance.UICanvas.PlaceUiLayer);
        _currentCenterButtonGroup = Instantiate(centerButtonGroupPrefab, UIManager.Instance.UICanvas.PlaceUiLayer);
        _currentBottomButtonGroup = Instantiate(bottomButtonGroupPrefab, UIManager.Instance.UICanvas.PlaceUiLayer);

        // ✅ 초기 설정
        _currentLeftButtonGroup.FadeOut(0f);
        _currentCenterButtonGroup.FadeOut(0f);
        _currentBottomButtonGroup.FadeOut(0f);

        // ✅ 버튼 그룹 순서 정렬 (SetSiblingIndex 사용)
        _currentLeftButtonGroup.transform.SetSiblingIndex(0); // 가장 아래 (먼저 렌더링됨)
        _currentCenterButtonGroup.transform.SetSiblingIndex(1); // 그 위
        _currentBottomButtonGroup.transform.SetSiblingIndex(2); // 최상단
    }


    /// <summary>
    /// ✅ BigPlace의 SmallPlaceDoor 정보를 기반으로 CenterButtonGroup에 버튼 추가
    /// </summary>
    private void UpdateCenterButtonGroup(BigPlace bigPlace)
    {
        if (bigPlace == null)
        {
            Debug.LogWarning("[PlaceUIManager] No active BigPlace found.");
            return;
        }

        List<SmallPlaceDoor> doors = bigPlace.SmallPlaceDoors;

        // ✅ 기존 버튼 제거 후 다시 생성
        _currentCenterButtonGroup.ClearButtons();

        if (doors == null || doors.Count == 0)
        {
            Debug.Log($"[PlaceUIManager] No SmallPlaceDoors found in {bigPlace.BigPlaceName}.");
            return;
        }

        foreach (var door in doors)
        {
            string doorId = $"{door.SmallPlaceName}";
            Vector3 doorPosition = door.transform.position; // ✅ Transform 위치 기반 버튼 배치

            _currentCenterButtonGroup.AddButton(doorId, () =>
            {
                Debug.Log($"[PlaceUIManager] Clicked SmallPlaceDoor: {door.SmallPlaceName} at {doorPosition}");
                SmallPlaceManager.Instance.EnterSmallPlace(door.SmallPlaceName, .5f);
            });
        }
    }
}
