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
    [SerializeField] private List<SmallPlace> _smallPlacePrefabs = new List<SmallPlace>(); // ✅ SmallPlace 프리팹 리스트
    [SerializeField] private BigPlaceUI _bigPlaceUiPrefab; // ✅ SmallPlace 프리팹 리스트
    [SerializeField] private SmallPlaceUI _smallPlaceUiPrefab; // ✅ SmallPlace 프리팹 리스트

    private List<SmallPlace> _activeSmallPlaces = new List<SmallPlace>();

    private ReactiveProperty<BigPlace> _currentBigPlaceNotifier = new ReactiveProperty<BigPlace>(null); // ✅ ReactiveProperty 적용
    public IReadOnlyReactiveProperty<BigPlace> CurrentBigPlaceNotifier => _currentBigPlaceNotifier; // ✅ ReadOnly로 외부 노출

    private ReactiveProperty<SmallPlace> _currentSmallPlaceNotifier = new ReactiveProperty<SmallPlace>(null); // ✅ ReactiveProperty 적용
    public IReadOnlyReactiveProperty<SmallPlace> CurrentSmallPlaceNotifier => _currentSmallPlaceNotifier; // ✅ ReadOnly로 외부 노출

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

    public void MoveBigPlace(EBigPlaceName placeName, float duration)
    {   
        if (_currentBigPlaceNotifier.Value != null)
        {
            ExitCurrentBigPlace(duration);
        }

        // ✅ 기존 SmallPlaces 완전 제거
        DestroyAllActiveSmallPlaces();

        BigPlace bigPlace = InstantiateBigPlace(placeName);
        bigPlace.FadeIn(duration);

        // ✅ 새로운 BigPlace에 연결된 SmallPlace 미리 생성
        foreach (var door in bigPlace.SmallPlaceDoors)
        {
            ESmallPlaceName smallPlaceName = door.SmallPlaceName;
            SmallPlace smallPlace = InstantiateSmallPlace(smallPlaceName);

            _activeSmallPlaces.Add(smallPlace);
            smallPlace.FadeOut(0f); // 기본적으로 숨김

            // ✅ SmallPlace에 실행될 Scenario 미리 설정
            Scenario assignedScenario = null; 
            smallPlace.SetScenario(assignedScenario);
        }

        _currentBigPlaceNotifier.Value = bigPlace; // ✅ ReactiveProperty 값 업데이트
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

        // ✅ 기존에 생성된 SmallPlace 찾기
        SmallPlace smallPlace = _activeSmallPlaces.FirstOrDefault(sp => sp.SmallPlaceName == smallPlaceName);
        if (smallPlace == null)
        {
            Debug.LogError($"[BigPlaceManager] ERROR - SmallPlace '{smallPlaceName}' not found in _activeSmallPlaces!");
            return;
        }

        // ✅ SmallPlace를 활성화하고 시각적으로 표시
        smallPlace.FadeIn(duration);

        // ✅ 첫 장면을 준비
        smallPlace.ReadyForScenarioStart();

        _currentSmallPlaceNotifier.Value = smallPlace;
    }

    public void ExitSmallPlace(float duration)
    {
        if (_currentSmallPlaceNotifier.Value == null)
        {
            Debug.LogWarning("[BigPlaceManager] No SmallPlace to exit from.");
            return;
        }

        // ✅ SmallPlace를 비활성화 (삭제 X, 단순히 숨김 처리)
        _currentSmallPlaceNotifier.Value.FadeOut(duration);
        _currentSmallPlaceNotifier.Value = null; // ✅ ReactiveProperty 값 초기화
    }
    
    /// <summary>
    /// ✅ 현재 활성화된 모든 SmallPlace 오브젝트를 제거
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
    /// 특정 SmallPlace 프리팹을 가져옴
    /// </summary>
    public SmallPlace GetSmallPlacePrefab(ESmallPlaceName smallPlaceName)
    {
        SmallPlace smallPlace = _smallPlacePrefabs.FirstOrDefault(sp => sp.SmallPlaceName == smallPlaceName);

        if (smallPlace == null)
            Debug.LogError($"[SmallPlaceManager] ERROR - SmallPlace '{smallPlaceName}' not found in prefabs!");

        return smallPlace;
    }

    /// <summary>
    /// SmallPlace를 생성하고 현재 장소로 설정
    /// </summary>
    public SmallPlace InstantiateSmallPlace(ESmallPlaceName smallPlaceName)
    {
        SmallPlace prefab = GetSmallPlacePrefab(smallPlaceName);
        if (prefab == null) return null;

        SmallPlace newSmallPlace = Instantiate(prefab, UIManager.Instance.GameCanvas.SmallPlaceLayer);
        Debug.Log($"[SmallPlaceManager] Entering SmallPlace: {smallPlaceName}");
        newSmallPlace.Init();

        return newSmallPlace;
    }
}
