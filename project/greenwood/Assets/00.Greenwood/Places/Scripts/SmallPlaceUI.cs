using UnityEngine;
using TMPro;
using UniRx;
using System.Collections.Generic;
using System;
using static SmallPlaceNames;

public class SmallPlaceUI : AnimationImage
{
    [SerializeField] private ButtonGroup leftButtonGroup;
    [SerializeField] private TextMeshProUGUI placeNameText; // ✅ 장소 이름

    public void Init()
    {
        // ✅ 초기 설정
        FadeOut(0f); // ✅ 기본적으로 숨김

        // ✅ SmallPlace의 변경을 감지하여 UI 업데이트
        PlaceManager.Instance.CurrentSmallPlaceNotifier
            .Subscribe(smallPlace =>
            {
                if (smallPlace != null) // 스몰플레이스 입장
                {
                    Debug.Log("[SmallPlaceUI] SmallPlace detected. Showing LeftPanel.");
                    FadeIn(0.3f);
                    leftButtonGroup.SetButtonGroup(GetSmallPlaceButtonGroup(smallPlace.SmallPlaceName));
                    placeNameText.text = smallPlace.SmallPlaceName.ToString(); // ✅ UI 업데이트
                }
                else // 스몰플레이스 퇴장
                {
                    Debug.Log("[SmallPlaceUI] SmallPlace exited. Hiding LeftPanel.");
                    FadeOut(0.3f);
                    placeNameText.text = ""; // ✅ 장소 이름 초기화
                }
            })
            .AddTo(this); // 자동 구독 해제
    }

    private Dictionary<string, Action> GetSmallPlaceButtonGroup(ESmallPlaceName? smallPlace)
    {
        switch (smallPlace.Value)
        {
            case ESmallPlaceName.Bakery:
                return new Dictionary<string, Action>
                {
                    { "Talk", () => Debug.Log("Talk at the Bakery") },
                    { "Buy", () => Debug.Log("Buying at the Bakery") },
                    { "Exit", () => PlaceManager.Instance.ExitSmallPlace(.3f) }
                };

            case ESmallPlaceName.Herbshop:
                return new Dictionary<string, Action>
                {
                    { "Talk", () => Debug.Log("Talking in the Herbshop") },
                    { "Buy", () => Debug.Log("Buying herbs") },
                    { "Heal", () => Debug.Log("Healing at the Herbshop") },
                    { "Exit", () => PlaceManager.Instance.ExitSmallPlace(.3f) }
                };

            case ESmallPlaceName.CafeSeabreeze:
                return new Dictionary<string, Action>
                {
                    { "Talk", () => Debug.Log("Talking in the Cafe") },
                    { "Order", () => Debug.Log("Ordering coffee") },
                    { "Exit", () => PlaceManager.Instance.ExitSmallPlace(.3f) }
                };

            default:
                return new Dictionary<string, Action>
                {
                    { "Exit", () => PlaceManager.Instance.ExitSmallPlace(.3f) }
                };
        }
    }
}
