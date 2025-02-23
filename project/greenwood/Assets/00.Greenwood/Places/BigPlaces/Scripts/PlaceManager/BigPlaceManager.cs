using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public enum EBigPlaceName
{
    Town,
    CafeSeabreezeFront
}

public class BigPlaceManager : MonoBehaviour
{
    public static BigPlaceManager Instance { get; private set; }

    [Header("BigPlace Prefabs")]
    [SerializeField] private List<BigPlace> _bigPlacePrefabs = new List<BigPlace>(); // ✅ BigPlace 프리팹 리스트

    private ReactiveProperty<BigPlace> _currentBigPlaceNotifier = new ReactiveProperty<BigPlace>(null); // ✅ ReactiveProperty 적용
    public IReadOnlyReactiveProperty<BigPlace> CurrentBigPlaceNotifier => _currentBigPlaceNotifier; // ✅ ReadOnly로 외부 노출

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
        Debug.Log("[BigPlaceManager] Initialized.");
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
    public BigPlace CreateBigPlace(EBigPlaceName placeName)
    {
        BigPlace prefab = GetBigPlace(placeName);
        if (prefab == null) return null;

        BigPlace newBigPlace = Instantiate(prefab, UIManager.Instance.GameCanvas.BigPlaceLayer);
        newBigPlace.Init();

        _currentBigPlaceNotifier.Value = newBigPlace; // ✅ ReactiveProperty 값 업데이트

        return newBigPlace;
    }

    public async UniTask MoveBigPlace(EBigPlaceName placeName)
    {   
        if(_currentBigPlaceNotifier.Value != null)
        {
            ExitCurrentBigPlace(.3f);
            await UniTask.WaitForSeconds(.3f);
        }
        BigPlace bigPlace = CreateBigPlace(placeName);
        bigPlace.FadeIn(.5f);
        
        await UniTask.WaitForSeconds(.5f);
    }


    /// <summary>
    /// 현재 BigPlace에서 퇴장
    /// </summary>
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
}
