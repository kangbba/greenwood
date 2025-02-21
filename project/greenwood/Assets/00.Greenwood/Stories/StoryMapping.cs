using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class StoryMapping
{
    [Title("스토리 설정")]
    public string storyName; // ✅ 스토리 이름

    [Title("조건 활성화 여부")]
    [ToggleLeft] public bool usePlaceState = true;  // ✅ 장소 상태 조건 사용 여부
    [ToggleLeft] public bool useTargetDay = true;   // ✅ 특정 날짜 조건 사용 여부
    [ToggleLeft] public bool useTargetTimePhase = false; // ✅ 특정 시간대 조건 사용 여부

    [Title("조건 값")]
    [ShowIf("usePlaceState")] public EPlaceState targetPlaceState = EPlaceState.InSmallPlace; // ✅ 특정 장소 상태
    [ShowIf("@targetPlaceState == EPlaceState.InBigPlace")] public EBigPlaceName bigPlaceName;
    [ShowIf("@targetPlaceState == EPlaceState.InSmallPlace")] public ESmallPlaceName smallPlaceName;
    [ShowIf("useTargetDay"), MinValue(1)] public int targetDay; // ✅ 1일 이상
    [ShowIf("useTargetTimePhase")] public TimePhase targetTimePhase;

    /// <summary>
    /// 현재 장소 상태와 시간에 대해 이 스토리가 실행 가능한지 확인
    /// </summary>
    public bool IsMatching(EPlaceState placeState, BigPlace bigPlace, SmallPlace smallPlace, int currentDay, TimePhase currentTimePhase)
    {
        bool placeStateMatch = !usePlaceState || targetPlaceState == placeState;
        bool placeMatch = true;

        if (placeState == EPlaceState.InBigPlace && targetPlaceState == EPlaceState.InBigPlace)
        {
            placeMatch = bigPlace?.BigPlaceName == bigPlaceName;
        }
        else if (placeState == EPlaceState.InSmallPlace && targetPlaceState == EPlaceState.InSmallPlace)
        {
            placeMatch = smallPlace?.SmallPlaceName == smallPlaceName;
        }

        bool dayMatch = !useTargetDay || targetDay == currentDay;
        bool timeMatch = !useTargetTimePhase || targetTimePhase == currentTimePhase;

        return placeStateMatch && placeMatch && dayMatch && timeMatch;
    }
}
