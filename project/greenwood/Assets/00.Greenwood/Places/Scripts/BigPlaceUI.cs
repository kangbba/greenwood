using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using UniRx;
using System;
using static BigPlaceNames;

public class BigPlaceUI : AnimationImage
{
    [SerializeField] private ButtonGroup bottomButtonGroup;
    [SerializeField] private ButtonGroup centerButtonGroup; // ✅ CenterButtonGroup 추가
    [SerializeField] private Map _mapPrefab;
    private Map _currentMap;

    public void Init()
    {
        FadeOut(0f);

        // ✅ 플레이어의 현재 위치 상태 감지하여 UI 업데이트
        PlayerManager.Instance.CurrentBigPlace
            .CombineLatest(PlayerManager.Instance.CurrentSmallPlace, (bigPlace, smallPlace) => (bigPlace, smallPlace))
            .Subscribe(tuple =>
            {
                (BigPlace bigPlace, SmallPlace smallPlace) = tuple;

                if (bigPlace != null && smallPlace == null) // ✅ BigPlace만 존재할 때만 UI 업데이트
                {
                  //  Debug.Log($"[BigPlaceUI] 현재 BigPlace: {bigPlace.BigPlaceName}");

                    // ✅ CenterButtonGroup 업데이트
                    UpdateCenterButtonGroup(bigPlace);
                    FadeIn(.3f);

                    // ✅ "이동" 버튼 추가
                    bottomButtonGroup.SetButtonGroup(new Dictionary<string, Action>
                    {
                        { "GoOut", () => { CreateAndShowMap(); Debug.Log("GoOut button clicked"); } },
                        { "GoHome", () => { Debug.Log("GoHome button clicked"); } }
                    });
                }
                else // ✅ SmallPlace가 활성화된 경우 UI 숨김
                {
                    FadeOut(0.3f);
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
        EBigPlaceName? selectedPlaceName = await _currentMap.ShowMap();

        // ✅ 선택된 장소 처리
        if (selectedPlaceName.HasValue)
        {
            Debug.Log($"[BigPlaceUI] Moving to {selectedPlaceName.Value}");
            PlayerManager.Instance.MoveBigPlace(selectedPlaceName.Value);
        }
        else
        {
            Debug.Log("[BigPlaceUI] Map selection canceled.");
        }
    }

    /// <summary>
    /// ✅ BigPlace의 SmallPlaceDoor 정보를 기반으로 CenterButtonGroup에 버튼 추가
    /// </summary>
    private void UpdateCenterButtonGroup(BigPlace bigPlace)
    {
        if (bigPlace == null)
        {
            Debug.LogWarning("[BigPlaceUI] No active BigPlace found.");
            return;
        }

        List<SmallPlaceDoor> doors = bigPlace.SmallPlaceDoors;

        // ✅ 기존 버튼 제거 후 다시 생성
        centerButtonGroup.ClearButtons();

        if (doors == null || doors.Count == 0)
        {
            Debug.Log($"[BigPlaceUI] No SmallPlaceDoors found in {bigPlace.BigPlaceName}.");
            return;
        }

        foreach (var door in doors)
        {
            string doorId = $"{door.SmallPlaceName}";
            Vector3 doorTrPosition = door.transform.position; // ✅ Transform 위치 기반 버튼 배치

            Button doorBtn = centerButtonGroup.AddButton(doorId, () =>
            {
                PlayerManager.Instance.EnterSmallPlace(door.SmallPlaceName);
            });

            doorBtn.transform.position = doorTrPosition;
        }
    }
}
