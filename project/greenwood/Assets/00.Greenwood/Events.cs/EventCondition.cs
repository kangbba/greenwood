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
        Flag,              // 특정 플래그 값이 true인지 확인
        CurrentSmallPlace, // 현재 플레이어가 특정 SmallPlace에 있는지 확인
        VisitedBigPlace,   // 특정 BigPlace를 방문한 적이 있는지 확인
        VisitedSmallPlace,
        HasItem            // 특정 아이템을 보유하고 있는지 확인
    }

    [SerializeField] private ConditionType _type;

    [SerializeField, ShowIf(nameof(IsFlagCondition))] 
    private string _flagKey;

    [SerializeField, ShowIf(nameof(IsCurrentSmallPlaceCondition))] 
    private ESmallPlaceName _targetSmallPlace;

    [SerializeField, ShowIf(nameof(IsVisitedBigPlaceCondition))] 
    private EBigPlaceName _visitedBigPlace;

    [SerializeField, ShowIf(nameof(IsVisitedSmallPlaceCondition))] 
    private ESmallPlaceName _visitedSmallPlace;

    [SerializeField, ShowIf(nameof(IsHasItemCondition))] 
    private string _targetItemId;
    private BoolReactiveProperty _isCleared = new BoolReactiveProperty(false);
    public IReadOnlyReactiveProperty<bool> IsCleared => _isCleared;

    public ConditionType Type { get => _type; }

    private IDisposable _subscription;

    private bool IsFlagCondition() => _type == ConditionType.Flag;
    private bool IsCurrentSmallPlaceCondition() => _type == ConditionType.CurrentSmallPlace;
    private bool IsVisitedBigPlaceCondition() => _type == ConditionType.VisitedBigPlace;
    private bool IsVisitedSmallPlaceCondition() => _type == ConditionType.VisitedSmallPlace;
    private bool IsHasItemCondition() => _type == ConditionType.HasItem;

    public void Initialize()
    {
        Dispose(); // ✅ 기존 구독 해제

        switch (_type)
        {
            case ConditionType.Flag:
                if (!string.IsNullOrEmpty(_flagKey))
                {
                    _subscription = FlagManager.Instance.GetFlagProperty(_flagKey)
                        .Subscribe(value =>
                        {
                            Debug.Log($"🔍 [EventCondition] Flag({_flagKey}) 값 변경: {value}");
                            _isCleared.Value = value;
                        });
                }
                break;
            case ConditionType.CurrentSmallPlace:
                _subscription = PlayerManager.Instance.CurrentSmallPlace
                    .Select(smallPlaceObj => smallPlaceObj != null && smallPlaceObj.SmallPlaceName == _targetSmallPlace)
                    .Subscribe(isInTargetPlace =>
                    {
                        Debug.Log($"🔍 [EventCondition] CurrentSmallPlace({_targetSmallPlace}) 값 변경: {isInTargetPlace}");
                        _isCleared.Value = isInTargetPlace;
                    });
                break;

            case ConditionType.VisitedBigPlace:
                _subscription = PlayerManager.Instance.VisitedBigPlaces
                    .Select(visited => visited.Contains(_visitedBigPlace))
                    .Subscribe(isVisited =>
                    {
                        Debug.Log($"🔍 [EventCondition] VisitedBigPlace({_visitedBigPlace}) 값 변경: {isVisited}");
                        _isCleared.Value = isVisited;
                    });
                break;

            case ConditionType.VisitedSmallPlace:
                _subscription = PlayerManager.Instance.VisitedSmallPlaces
                    .Select(visited => visited.Contains(_visitedSmallPlace))
                    .Subscribe(isVisited =>
                    {
                        Debug.Log($"🔍 [EventCondition] VisitedSmallPlace({_visitedSmallPlace}) 값 변경: {isVisited}");
                        _isCleared.Value = isVisited;
                    });
                break;

        case ConditionType.HasItem:
            _subscription = ItemManager.Instance.OwnedItemData
                .Select(ownedItems => ownedItems.Contains(_targetItemId))
                .Subscribe(hasItem =>
                {
                    Debug.Log($"🔍 [EventCondition] HasItem({_targetItemId}) 값 변경: {hasItem}");
                    _isCleared.Value = hasItem;
                });
            break;

        }
    }


    public void Dispose()
    {
        _subscription?.Dispose();
    }
}
