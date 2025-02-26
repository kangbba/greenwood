using System;
using UniRx;
using UnityEngine;
using Sirenix.OdinInspector;
using static SmallPlaceNames;
using static BigPlaceNames;

[Serializable]
public class EventCondition
{
    public enum ConditionType
    {
        Flag,
        CurrentSmallPlace,
        VisitedBigPlace,
        VisitedSmallPlace,
        HasItem,
        SmallPlaceFirstVisit,
    }

    [SerializeField] private ConditionType _type;

    [SerializeField, ShowIf(nameof(IsFlagCondition))] private string _flagKey;
    [SerializeField, ShowIf(nameof(IsCurrentSmallPlaceCondition))] private ESmallPlaceName _targetSmallPlace;
    [SerializeField, ShowIf(nameof(IsVisitedBigPlaceCondition))] private EBigPlaceName _visitedBigPlace;
    [SerializeField, ShowIf(nameof(IsVisitedSmallPlaceCondition))] private ESmallPlaceName _visitedSmallPlace;
    [SerializeField, ShowIf(nameof(IsHasItemCondition))] private string _targetItemId;
    [SerializeField, ShowIf(nameof(IsSmallPlaceFirstVisitCondition))] private ESmallPlaceName _firstVisitSmallPlace;

    [SerializeField, Tooltip("조건을 반전하여 적용 (예: 방문하지 않았을 때 true)")]
    private bool _invertCondition = false; // ✅ 조건 반전 여부

    private readonly BoolReactiveProperty _isCleared = new BoolReactiveProperty(false);
    public IReadOnlyReactiveProperty<bool> IsCleared => _isCleared;

    public ConditionType Type => _type;

    private bool IsFlagCondition() => _type == ConditionType.Flag;
    private bool IsCurrentSmallPlaceCondition() => _type == ConditionType.CurrentSmallPlace;
    private bool IsVisitedBigPlaceCondition() => _type == ConditionType.VisitedBigPlace;
    private bool IsVisitedSmallPlaceCondition() => _type == ConditionType.VisitedSmallPlace;
    private bool IsHasItemCondition() => _type == ConditionType.HasItem;
    private bool IsSmallPlaceFirstVisitCondition() => _type == ConditionType.SmallPlaceFirstVisit;

    /// <summary>
    /// ✅ 해당 조건이 무엇을 의미하는지 설명을 반환
    /// </summary>
    public string GetConditionDescription()
    {
        string description = _type switch
        {
            ConditionType.Flag => $"플래그 활성화 여부: '{_flagKey}'",
            ConditionType.CurrentSmallPlace => $"현재 위치: '{_targetSmallPlace}'",
            ConditionType.VisitedBigPlace => $"방문한 BigPlace: '{_visitedBigPlace}'",
            ConditionType.VisitedSmallPlace => $"방문한 SmallPlace: '{_visitedSmallPlace}'",
            ConditionType.HasItem => $"보유 중인 아이템: '{_targetItemId}'",
            ConditionType.SmallPlaceFirstVisit => $"SmallPlace 첫 방문 여부: '{_firstVisitSmallPlace}'",
            _ => "알 수 없는 조건"
        };

        return _invertCondition ? $"(반전됨) {description}" : description;
    }

   public IObservable<bool> IsSatisfiedStream()
    {
        IObservable<bool> conditionStream = _type switch
        {
            ConditionType.Flag => FlagManager.Instance.GetFlagProperty(_flagKey),
            ConditionType.CurrentSmallPlace => PlayerManager.Instance.CurrentSmallPlace
                .Select(smallPlaceObj => smallPlaceObj != null && smallPlaceObj.SmallPlaceName == _targetSmallPlace),
            ConditionType.VisitedBigPlace => PlayerManager.Instance.VisitedBigPlaces
                .Select(visited => visited.Contains(_visitedBigPlace)),
            ConditionType.VisitedSmallPlace => PlayerManager.Instance.VisitedSmallPlaces
                .Select(visited => visited.Contains(_visitedSmallPlace)),
            ConditionType.HasItem => ItemManager.Instance.OwnedItemData
                .Select(ownedItems => ownedItems.Contains(_targetItemId)),

            // ✅ SmallPlaceFirstVisit: 방문한 적이 없는 상태에서 처음으로 방문할 때만 true
            ConditionType.SmallPlaceFirstVisit => PlayerManager.Instance.CurrentSmallPlace
                .Where(current => current != null && current.SmallPlaceName == _firstVisitSmallPlace)
                .Take(1) // ✅ 첫 번째 값만 반응하도록 제한 (이후 변화 무시)
                .CombineLatest(PlayerManager.Instance.VisitedSmallPlaces,
                    (current, visitedPlaces) => !visitedPlaces.Contains(_firstVisitSmallPlace))
                .DistinctUntilChanged(), // ✅ 중복 값 무시 (한 번만 true 반환)

            _ => Observable.Return(false)
        };

        // ✅ 반전 옵션 적용 (예: 방문한 적이 없을 경우 만족)
        return conditionStream.Select(result => _invertCondition ? !result : result);
    }


}
