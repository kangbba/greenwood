using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using static BigPlaceNames;
using static SmallPlaceNames;

public class PlaceManager : MonoBehaviour
{
    public static PlaceManager Instance { get; private set; }

    [Header("BigPlace Prefabs")]
    [SerializeField] private List<BigPlace> _bigPlacePrefabs = new List<BigPlace>(); // ✅ BigPlace 프리팹 리스트
    [SerializeField] private List<SmallPlaceBase> _smallPlacePrefabs = new List<SmallPlaceBase>(); // ✅ SmallPlaceBase 프리팹 리스트
    [SerializeField] private BigPlaceUI _bigPlaceUiPrefab; // ✅ SmallPlaceBase 프리팹 리스트
    [SerializeField] private SmallPlaceUI _smallPlaceUiPrefab; // ✅ SmallPlaceBase 프리팹 리스트

    private List<SmallPlaceBase> _activeSmallPlaces = new List<SmallPlaceBase>();

    private ReactiveProperty<BigPlace> _currentBigPlaceNotifier = new ReactiveProperty<BigPlace>(null); // ✅ ReactiveProperty 적용
    public IReadOnlyReactiveProperty<BigPlace> CurrentBigPlaceNotifier => _currentBigPlaceNotifier; // ✅ ReadOnly로 외부 노출

    private ReactiveProperty<SmallPlaceBase> _currentSmallPlaceNotifier = new ReactiveProperty<SmallPlaceBase>(null); // ✅ ReactiveProperty 적용
    public IReadOnlyReactiveProperty<SmallPlaceBase> CurrentSmallPlaceNotifier => _currentSmallPlaceNotifier; // ✅ ReadOnly로 외부 노출

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

    private void Start()
    {
        BigPlaceUI bigPlaceUI = Instantiate(_bigPlaceUiPrefab, UIManager.Instance.UICanvas.PlaceUiLayer);
        bigPlaceUI.Init();
        SmallPlaceUI smallPlaceUI = Instantiate(_smallPlaceUiPrefab, UIManager.Instance.UICanvas.PlaceUiLayer);
        smallPlaceUI.Init();
    }

    /// <summary>
    /// 특정 BigPlace 프리팹을 가져옴
    /// </summary>
    public BigPlace GetBigPlace(EBigPlaceName placeName)
    {
        BigPlace bigPlace = _bigPlacePrefabs.FirstOrDefault(bp => bp.BigPlaceName == placeName);

        if (bigPlace == null)
            Debug.LogError($"[BigPlaceManager] ERROR - BigPlace '{placeName}' not found in prefabs!");

        return bigPlace;
    }

    /// <summary>
    /// BigPlace를 생성하고 현재 장소로 설정
    /// </summary>
    public BigPlace InstantiateBigPlace(EBigPlaceName placeName)
    {
        BigPlace prefab = GetBigPlace(placeName);
        if (prefab == null) return null;

        BigPlace newBigPlace = Instantiate(prefab, UIManager.Instance.GameCanvas.BigPlaceLayer);
        newBigPlace.Init();

        return newBigPlace;
    }

    public void MoveBigPlace(
        EBigPlaceName placeName,
        float duration,
        List<SmallPlaceActionPlan> actionPlans
    )
    {
        if (_currentBigPlaceNotifier.Value != null)
        {
            ExitCurrentBigPlace(duration);
        }

        DestroyAllActiveSmallPlaces();

        BigPlace bigPlace = InstantiateBigPlace(placeName);
        bigPlace.FadeIn(duration);

        foreach (var door in bigPlace.SmallPlaceDoors)
        {
            ESmallPlaceName smallPlaceName = door.SmallPlaceName;
            SmallPlaceBase smallPlace = InstantiateSmallPlace(smallPlaceName);
            _activeSmallPlaces.Add(smallPlace);
            smallPlace.FadeOut(0f);

            // ✅ List 기반으로 ActionPlan 찾기
            SmallPlaceActionPlan plan = actionPlans.Find(
                p => p.SmallPlaceName == smallPlaceName
            );

            if (plan != null)
            {
                plan.ApplyActions(smallPlace);
            }
        }

        _currentBigPlaceNotifier.Value = bigPlace;
    }



    public void ExitCurrentBigPlace(float duration)
    {
        if (_currentBigPlaceNotifier.Value == null)
        {
            Debug.LogWarning("[BigPlaceManager] No BigPlace to exit from.");
            return;
        }

        _currentBigPlaceNotifier.Value.FadeAndDestroy(duration);
        _currentBigPlaceNotifier.Value = null; // ✅ ReactiveProperty 값 초기화 (구독자 감지 가능)
    }

    public void EnterSmallPlace(ESmallPlaceName smallPlaceName, float duration)
    {
        if (_currentSmallPlaceNotifier.Value != null)
        {
            ExitSmallPlace(duration);
        }

        // ✅ 기존에 생성된 SmallPlaceBase 찾기
        SmallPlaceBase smallPlace = _activeSmallPlaces.FirstOrDefault(sp => sp.SmallPlaceName == smallPlaceName);
        if (smallPlace == null)
        {
            Debug.LogError($"[BigPlaceManager] ERROR - SmallPlaceBase '{smallPlaceName}' not found in _activeSmallPlaces!");
            return;
        }

        // ✅ SmallPlace를 활성화하고 시각적으로 표시
        smallPlace.FadeIn(duration);
        

        _currentSmallPlaceNotifier.Value = smallPlace;
    }

    public void ExitSmallPlace(float duration)
    {
        if (_currentSmallPlaceNotifier.Value == null)
        {
            Debug.LogWarning("[BigPlaceManager] No SmallPlaceBase to exit from.");
            return;
        }

        // ✅ SmallPlace를 비활성화 (삭제 X, 단순히 숨김 처리)
        _currentSmallPlaceNotifier.Value.FadeOut(duration);
        _currentSmallPlaceNotifier.Value = null; // ✅ ReactiveProperty 값 초기화
    }
    
    /// <summary>
    /// ✅ 현재 활성화된 모든 SmallPlaceBase 오브젝트를 제거
    /// </summary>
    private void DestroyAllActiveSmallPlaces()
    {
        foreach (var smallPlace in _activeSmallPlaces)
        {
            if (smallPlace != null)
            {
                Destroy(smallPlace.gameObject);
            }
        }

        _activeSmallPlaces.Clear();
    }

    /// <summary>
    /// 특정 SmallPlaceBase 프리팹을 가져옴
    /// </summary>
    public SmallPlaceBase GetSmallPlacePrefab(ESmallPlaceName smallPlaceName)
    {
        SmallPlaceBase smallPlace = _smallPlacePrefabs.FirstOrDefault(sp => sp.SmallPlaceName == smallPlaceName);

        if (smallPlace == null)
            Debug.LogError($"[SmallPlaceManager] ERROR - SmallPlaceBase '{smallPlaceName}' not found in prefabs!");

        return smallPlace;
    }

    /// <summary>
    /// SmallPlace를 생성하고 현재 장소로 설정
    /// </summary>
    public SmallPlaceBase InstantiateSmallPlace(ESmallPlaceName smallPlaceName)
    {
        SmallPlaceBase prefab = GetSmallPlacePrefab(smallPlaceName);
        if (prefab == null) return null;

        SmallPlaceBase newSmallPlace = Instantiate(prefab, UIManager.Instance.GameCanvas.SmallPlaceLayer);
        Debug.Log($"[SmallPlaceManager] Entering SmallPlaceBase: {smallPlaceName}");
        newSmallPlace.Init();

        return newSmallPlace;
    }
}
