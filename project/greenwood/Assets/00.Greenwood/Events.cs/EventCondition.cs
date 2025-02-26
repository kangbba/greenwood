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
        HasItem
    }

    [SerializeField] private ConditionType _type;

    [SerializeField, ShowIf(nameof(IsFlagCondition))] private string _flagKey;
    [SerializeField, ShowIf(nameof(IsCurrentSmallPlaceCondition))] private ESmallPlaceName _targetSmallPlace;
    [SerializeField, ShowIf(nameof(IsVisitedBigPlaceCondition))] private EBigPlaceName _visitedBigPlace;
    [SerializeField, ShowIf(nameof(IsVisitedSmallPlaceCondition))] private ESmallPlaceName _visitedSmallPlace;
    [SerializeField, ShowIf(nameof(IsHasItemCondition))] private string _targetItemId;

    private readonly BoolReactiveProperty _isCleared = new BoolReactiveProperty(false);
    public IReadOnlyReactiveProperty<bool> IsCleared => _isCleared;

    public ConditionType Type => _type;

    private bool IsFlagCondition() => _type == ConditionType.Flag;
    private bool IsCurrentSmallPlaceCondition() => _type == ConditionType.CurrentSmallPlace;
    private bool IsVisitedBigPlaceCondition() => _type == ConditionType.VisitedBigPlace;
    private bool IsVisitedSmallPlaceCondition() => _type == ConditionType.VisitedSmallPlace;
    private bool IsHasItemCondition() => _type == ConditionType.HasItem;

    /// <summary>
    /// ✅ 해당 조건이 무엇을 의미하는지 설명을 반환
    /// </summary>
    public string GetConditionDescription()
    {
        switch (_type)
        {
            case ConditionType.Flag:
                return $"플래그 활성화 여부: '{_flagKey}'";
            case ConditionType.CurrentSmallPlace:
                return $"현재 위치: '{_targetSmallPlace}'";
            case ConditionType.VisitedBigPlace:
                return $"방문한 BigPlace: '{_visitedBigPlace}'";
            case ConditionType.VisitedSmallPlace:
                return $"방문한 SmallPlace: '{_visitedSmallPlace}'";
            case ConditionType.HasItem:
                return $"보유 중인 아이템: '{_targetItemId}'";
            default:
                return "알 수 없는 조건";
        }
    }

    public IObservable<bool> IsSatisfiedStream()
    {
        switch (_type)
        {
            case ConditionType.Flag:
                return FlagManager.Instance.GetFlagProperty(_flagKey);
            case ConditionType.CurrentSmallPlace:
                return PlayerManager.Instance.CurrentSmallPlace
                    .Select(smallPlaceObj => smallPlaceObj != null && smallPlaceObj.SmallPlaceName == _targetSmallPlace);
            case ConditionType.VisitedBigPlace:
                return PlayerManager.Instance.VisitedBigPlaces
                    .Select(visited => visited.Contains(_visitedBigPlace));
            case ConditionType.VisitedSmallPlace:
                return PlayerManager.Instance.VisitedSmallPlaces
                    .Select(visited => visited.Contains(_visitedSmallPlace));
            case ConditionType.HasItem:
                return ItemManager.Instance.OwnedItemData
                    .Select(ownedItems => ownedItems.Contains(_targetItemId));
            default:
                return Observable.Return(false);
        }
    }
}
