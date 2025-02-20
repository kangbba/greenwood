using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class StoryMapping
{
    [Title("스토리 설정")]
    public string storyName; // ✅ 스토리 이름

    [Title("조건 활성화 여부")]
    [ToggleLeft] public bool useBigPlace = true;     // ✅ BigPlace 조건 사용 여부
    [ToggleLeft] public bool useSmallPlace = false;  // ✅ SmallPlace 조건 사용 여부
    [ToggleLeft] public bool useTargetDay = true;    // ✅ 특정 날짜 조건 사용 여부
    [ToggleLeft] public bool useTargetTimePhase = true; // ✅ 특정 시간대 조건 사용 여부

    [Title("조건 값")]
    [ShowIf("useBigPlace")] public EBigPlaceName bigPlaceName;
    [ShowIf("useSmallPlace")] public ESmallPlaceName smallPlaceName;
    [ShowIf("useTargetDay"), MinValue(1)] public int targetDay; // ✅ 1일 이상
    [ShowIf("useTargetTimePhase")] public TimePhase targetTimePhase;

    /// <summary>
    /// 현재 장소와 시간에 대해 이 스토리가 실행 가능한지 확인
    /// </summary>
    public bool IsMatching(BigPlace bigPlace, SmallPlace smallPlace, int currentDay, TimePhase currentTimePhase)
    {
        bool bigPlaceMatch = !useBigPlace || (bigPlace != null && bigPlace.BigPlaceName == bigPlaceName);
        bool smallPlaceMatch = !useSmallPlace || (smallPlace != null && smallPlace.SmallPlaceName == smallPlaceName);
        bool dayMatch = !useTargetDay || targetDay == currentDay;
        bool timeMatch = !useTargetTimePhase || targetTimePhase == currentTimePhase;

        return bigPlaceMatch && smallPlaceMatch && dayMatch && timeMatch;
    }
}
