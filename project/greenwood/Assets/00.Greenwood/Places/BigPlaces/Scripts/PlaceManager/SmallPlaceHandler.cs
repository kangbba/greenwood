using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class SmallPlaceHandler : MonoBehaviour
{
    [Header("SmallPlace Prefabs")]
    [SerializeField] private List<SmallPlace> _smallPlacePrefabs = new List<SmallPlace>(); // ✅ SmallPlace 프리팹 리스트

    private ReactiveProperty<SmallPlace> _currentSmallPlaceNotifier = new ReactiveProperty<SmallPlace>(null); // ✅ ReactiveProperty 적용

    public IReadOnlyReactiveProperty<SmallPlace> CurrentSmallPlaceNotifier => _currentSmallPlaceNotifier; // ✅ ReadOnly로 외부 노출

    public void Init()
    {
        Debug.Log("[SmallPlaceHandler] Initialized.");
    }

    public SmallPlace GetSmallPlace(ESmallPlaceName smallPlaceName)
    {
        SmallPlace smallPlace = _smallPlacePrefabs.FirstOrDefault(sp => sp.SmallPlaceName == smallPlaceName);

        if (smallPlace == null)
            Debug.LogError($"[SmallPlaceHandler] ERROR - SmallPlace '{smallPlaceName}' not found in prefabs!");

        return smallPlace;
    }

    public SmallPlace CreateSmallPlace(ESmallPlaceName smallPlaceName)
    {
        SmallPlace prefab = GetSmallPlace(smallPlaceName);
        if (prefab == null) return null;

        SmallPlace newSmallPlace = Instantiate(prefab, UIManager.Instance.GameCanvas.SmallPlaceLayer);
        Debug.Log($"[SmallPlaceHandler] Entering SmallPlace: {smallPlaceName}");
        newSmallPlace.Init();

        _currentSmallPlaceNotifier.Value = newSmallPlace; // ✅ ReactiveProperty 값 업데이트

        return newSmallPlace;
    }

    public void ExitSmallPlace(float duration)
    {
        if (_currentSmallPlaceNotifier.Value == null)
        {
            Debug.LogWarning("[SmallPlaceHandler] No SmallPlace to exit from.");
            return;
        }

        _currentSmallPlaceNotifier.Value.FadeAndDestroy(duration);
        _currentSmallPlaceNotifier.Value = null; // ✅ ReactiveProperty 값 초기화 (구독자 감지 가능)
    }
}
