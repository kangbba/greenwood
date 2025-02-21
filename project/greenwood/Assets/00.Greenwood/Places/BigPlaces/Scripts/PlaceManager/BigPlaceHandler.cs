using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BigPlaceHandler : MonoBehaviour
{
    [Header("BigPlace Prefabs")]
    [SerializeField] private List<BigPlace> _bigPlacePrefabs = new List<BigPlace>(); // ✅ BigPlace 프리팹 리스트

    private BigPlace _currentBigPlace = null;

    public BigPlace CurrentBigPlace => _currentBigPlace; // ✅ 프로퍼티 Get만 허용

    public void Init()
    {
        Debug.Log("[BigPlaceHandler] Initialized.");
    }

    /// <summary>
    /// 특정 BigPlace 프리팹을 가져옴
    /// </summary>
    public BigPlace GetBigPlace(EBigPlaceName placeName)
    {
        BigPlace bigPlace = _bigPlacePrefabs.FirstOrDefault(bp => bp.BigPlaceName == placeName);

        if (bigPlace == null)
            Debug.LogError($"[BigPlaceHandler] ERROR - BigPlace '{placeName}' not found in prefabs!");

        return bigPlace;
    }

    /// <summary>
    /// BigPlace를 생성하고 현재 장소로 설정
    /// </summary>
    public BigPlace CreateBigPlace(EBigPlaceName placeName)
    {
        BigPlace prefab = GetBigPlace(placeName);
        if (prefab == null) return null;

        _currentBigPlace = Instantiate(prefab, UIManager.Instance.GameCanvas.BigPlaceLayer);
        _currentBigPlace.Init();
        Debug.Log($"[BigPlaceHandler] Entering BigPlace: {placeName}");

        return _currentBigPlace;
    }

    /// <summary>
    /// 현재 BigPlace에서 퇴장
    /// </summary>
    public void ExitCurrentBigPlace(float duration)
    {
        if (_currentBigPlace == null)
        {
            Debug.LogWarning("[BigPlaceHandler] No BigPlace to exit from.");
            return;
        }

        BigPlace exitingBigPlace = _currentBigPlace;
        _currentBigPlace = null;

        exitingBigPlace.FadeAndDestroy(duration);
    }
}
