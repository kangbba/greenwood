using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public enum ESmallPlaceName
{
    Bakery,
    Herbshop,
    CafeSeabreeze
}

public class SmallPlaceManager : MonoBehaviour
{
    public static SmallPlaceManager Instance { get; private set; }

    [Header("SmallPlace Prefabs")]
    [SerializeField] private List<SmallPlace> _smallPlacePrefabs = new List<SmallPlace>(); // ✅ SmallPlace 프리팹 리스트

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

    public void Init()
    {
        Debug.Log("[SmallPlaceManager] Initialized.");
    }

    /// <summary>
    /// 특정 SmallPlace 프리팹을 가져옴
    /// </summary>
    public SmallPlace GetSmallPlace(ESmallPlaceName smallPlaceName)
    {
        SmallPlace smallPlace = _smallPlacePrefabs.FirstOrDefault(sp => sp.SmallPlaceName == smallPlaceName);

        if (smallPlace == null)
            Debug.LogError($"[SmallPlaceManager] ERROR - SmallPlace '{smallPlaceName}' not found in prefabs!");

        return smallPlace;
    }

    /// <summary>
    /// SmallPlace를 생성하고 현재 장소로 설정
    /// </summary>
    public SmallPlace CreateSmallPlace(ESmallPlaceName smallPlaceName)
    {
        SmallPlace prefab = GetSmallPlace(smallPlaceName);
        if (prefab == null) return null;

        SmallPlace newSmallPlace = Instantiate(prefab, UIManager.Instance.GameCanvas.SmallPlaceLayer);
        Debug.Log($"[SmallPlaceManager] Entering SmallPlace: {smallPlaceName}");
        newSmallPlace.Init();

        _currentSmallPlaceNotifier.Value = newSmallPlace; // ✅ ReactiveProperty 값 업데이트

        return newSmallPlace;
    }

    /// <summary>
    /// 현재 SmallPlace에서 퇴장
    /// </summary>
    public void ExitSmallPlace(float duration)
    {
        if (_currentSmallPlaceNotifier.Value == null)
        {
            Debug.LogWarning("[SmallPlaceManager] No SmallPlace to exit from.");
            return;
        }

        _currentSmallPlaceNotifier.Value.FadeAndDestroy(duration);
        _currentSmallPlaceNotifier.Value = null; // ✅ ReactiveProperty 값 초기화 (구독자 감지 가능)
    }
}
