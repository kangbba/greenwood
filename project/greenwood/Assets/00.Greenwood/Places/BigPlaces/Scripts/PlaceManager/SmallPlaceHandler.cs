using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SmallPlaceHandler : MonoBehaviour
{
    [Header("SmallPlace Prefabs")]
    [SerializeField] private List<SmallPlace> _smallPlacePrefabs = new List<SmallPlace>(); // ✅ SmallPlace 프리팹 리스트

    private SmallPlace _currentSmallPlace = null;

    public SmallPlace CurrentSmallPlace => _currentSmallPlace;

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

        _currentSmallPlace = Instantiate(prefab, UIManager.Instance.GameCanvas.SmallPlaceLayer);
        Debug.Log($"[SmallPlaceHandler] Entering SmallPlace: {smallPlaceName}");
        _currentSmallPlace.Init();

        return _currentSmallPlace;
    }

    public void ExitSmallPlace(float duration)
    {
        if (_currentSmallPlace == null)
        {
            Debug.LogWarning("[SmallPlaceHandler] No SmallPlace to exit from.");
            return;
        }
        _currentSmallPlace.FadeAndDestroy(duration);
        _currentSmallPlace = null;
    }
}
