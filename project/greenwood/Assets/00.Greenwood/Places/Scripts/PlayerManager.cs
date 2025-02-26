using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using static BigPlaceNames;
using static SmallPlaceNames;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    private ReactiveProperty<BigPlace> _currentBigPlace = new ReactiveProperty<BigPlace>(null);
    private ReactiveProperty<SmallPlace> _currentSmallPlace = new ReactiveProperty<SmallPlace>(null);

    private ReactiveProperty<HashSet<EBigPlaceName>> _visitedBigPlaces = new(new HashSet<EBigPlaceName>()); // ✅ 방문한 BigPlace 감지
    private ReactiveProperty<HashSet<ESmallPlaceName>> _visitedSmallPlaces = new(new HashSet<ESmallPlaceName>()); // ✅ 방문한 SmallPlace 감지

    public IReadOnlyReactiveProperty<BigPlace> CurrentBigPlace => _currentBigPlace;
    public IReadOnlyReactiveProperty<SmallPlace> CurrentSmallPlace => _currentSmallPlace;

    public IReadOnlyReactiveProperty<HashSet<EBigPlaceName>> VisitedBigPlaces => _visitedBigPlaces;
    public IReadOnlyReactiveProperty<HashSet<ESmallPlaceName>> VisitedSmallPlaces => _visitedSmallPlaces;

    public EBigPlaceName? CurrentBigPlaceName => _currentBigPlace.Value?.BigPlaceName;
    public ESmallPlaceName? CurrentSmallPlaceName => _currentSmallPlace.Value?.SmallPlaceName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void MoveBigPlace(EBigPlaceName placeName)
    {
        var place = PlaceManager.Instance.GetBigPlace(placeName);
        if (place != null)
        {
            _currentBigPlace.Value = place;
            _currentSmallPlace.Value = null;

            if (!_visitedBigPlaces.Value.Contains(placeName))
            {
                _visitedBigPlaces.Value = new HashSet<EBigPlaceName>(_visitedBigPlaces.Value) { placeName };
                Debug.Log($"🚀 [PlayerManager] BigPlace '{placeName}' 방문 기록 추가됨!");
            }
        }
        else
        {
            Debug.LogWarning($"BigPlace [{placeName}]를 찾을 수 없습니다.");
        }
    }

    public void EnterSmallPlace(ESmallPlaceName smallPlaceName)
    {
        var smallPlace = PlaceManager.Instance.GetSmallPlace(smallPlaceName);
        if (smallPlace != null)
        {
            _currentSmallPlace.Value = smallPlace;

            if (!_visitedSmallPlaces.Value.Contains(smallPlaceName))
            {
                _visitedSmallPlaces.Value = new HashSet<ESmallPlaceName>(_visitedSmallPlaces.Value) { smallPlaceName };
                Debug.Log($"🚀 [PlayerManager] SmallPlace '{smallPlaceName}' 방문 기록 추가됨!");
            }
        }
        else
        {
            Debug.LogWarning($"SmallPlace [{smallPlaceName}]를 찾을 수 없습니다.");
        }
    }


    /// <summary>
    /// ✅ 현재 BigPlace에서 나가기
    /// </summary>
    public void ExitCurrentBigPlace()
    {
        _currentBigPlace.Value = null;
        _currentSmallPlace.Value = null;
    }

    /// <summary>
    /// ✅ 현재 SmallPlace에서 나가기
    /// </summary>
    public void ExitSmallPlace()
    {
        _currentSmallPlace.Value = null;
    }
    public void ClearVisitedPlaces()
    {
        _visitedBigPlaces.Value = new HashSet<EBigPlaceName>(); // ✅ 방문 기록 초기화
        _visitedSmallPlaces.Value = new HashSet<ESmallPlaceName>(); // ✅ 방문 기록 초기화
        Debug.Log("🧹 [PlayerManager] 모든 방문 기록이 초기화됨!");
    }

}
