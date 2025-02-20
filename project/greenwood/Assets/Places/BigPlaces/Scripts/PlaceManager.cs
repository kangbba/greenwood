
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public enum EBigPlaceName
{
    Town,
    CafeSeabreezeFront
}
public enum ESmallPlaceName{
    Bakery,
    Herbshop,
    CafeSeabreeze
}

public class PlaceManager : MonoBehaviour
{
    public static PlaceManager Instance { get; private set; }

    [Header("BigPlace Prefabs")]
    [SerializeField] private List<BigPlace> _bigPlacePrefabs;

    private Dictionary<EBigPlaceName, BigPlace> _bigPlaceInstances = new Dictionary<EBigPlaceName, BigPlace>();

    private ReactiveProperty<BigPlace> _currentBigPlace = new ReactiveProperty<BigPlace>(null); // ✅ UniRx 적용
    private ReactiveProperty<SmallPlace> _currentSmallPlace = new ReactiveProperty<SmallPlace>(null); // ✅ UniRx 적용

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        // ✅ BigPlace 변경 시 UI 자동 업데이트 (새로운 BigPlace UI 표시 or 숨김)
        _currentBigPlace
            .Subscribe(bigPlace =>
            {
                if (bigPlace != null)
                {
                    Debug.Log($"[PlaceManager] _currentBigPlace changed to: {bigPlace.BigPlaceName}");
                    PlaceUiManager.Instance.ShowBigPlaceUI(bigPlace).Forget();
                }
                else
                {
                    Debug.Log("[PlaceManager] No active BigPlace, hiding UI.");
                    PlaceUiManager.Instance.HideBigPlaceUI().Forget();
                }
            })
            .AddTo(this);

        // ✅ SmallPlace 상태에 따라 BigPlace & SmallPlace UI 자동 업데이트
        _currentSmallPlace
            .Subscribe(smallPlace =>
            {
                if (_currentBigPlace.Value == null) return;

                if (smallPlace == null)
                {
                    // ✅ SmallPlace가 없는 경우 → BigPlace UI 표시 & SmallPlace UI 숨김
                    Debug.Log("[PlaceManager] SmallPlace is null, showing BigPlace UI.");
                    PlaceUiManager.Instance.ShowBigPlaceUI(_currentBigPlace.Value).Forget();
                    PlaceUiManager.Instance.HideSmallPlaceUI().Forget();
                }
                else
                {
                    // ✅ SmallPlace가 있는 경우 → BigPlace UI 숨김 & SmallPlace UI 표시
                    Debug.Log($"[PlaceManager] Entered SmallPlace: {smallPlace.SmallPlaceName}, hiding BigPlace UI.");
                    PlaceUiManager.Instance.HideBigPlaceUI().Forget();
                    PlaceUiManager.Instance.ShowSmallPlaceUI(smallPlace).Forget();
                }
            })
            .AddTo(this);
    }


    /// <summary>
    /// 새로운 BigPlace로 이동 (기존 BigPlace 제거 후 생성)
    /// </summary>
    public async UniTask MoveBigPlace(EBigPlaceName newPlace)
    {
        BigPlace currentBigPlace = _currentBigPlace.Value;
        if (currentBigPlace != null)
        {
            _currentBigPlace.Value = null;
            await currentBigPlace.HideAndDestroy();
        }

        // ✅ 새로운 BigPlace 생성
        BigPlace instBigPlace = CreateBigPlace(newPlace);
        await instBigPlace.Show();
    }

    /// <summary>
    /// 새로운 BigPlace 생성
    /// </summary>
    private BigPlace CreateBigPlace(EBigPlaceName placeName)
    {
        if (_bigPlaceInstances.ContainsKey(placeName))
        {
            Debug.LogWarning($"[PlaceManager] BigPlace '{placeName}' already exists.");
            return null;
        }

        BigPlace prefab = _bigPlacePrefabs.Find(bp => bp.BigPlaceName == placeName);
        if (prefab == null)
        {
            Debug.LogError($"[PlaceManager] ERROR - BigPlace '{placeName}' not found in prefabs!");
            return null;
        }

        BigPlace instance = Instantiate(prefab, UIManager.Instance.GameCanvas.BigPlaceLayer);
        _bigPlaceInstances[placeName] = instance;
        _currentBigPlace.Value = instance; // ✅ ReactiveProperty 업데이트 → UI 자동 변경

        return instance;
    }

    /// <summary>
    /// SmallPlace 입장
    /// </summary>
    public async void EnterSmallPlace(ESmallPlaceName smallPlaceName)
    {
        if (_currentBigPlace.Value == null)
        {
            Debug.LogError("[PlaceManager] No active BigPlace to enter a SmallPlace from!");
            return;
        }

        Debug.Log($"[PlaceManager] Entering SmallPlace: {smallPlaceName}");

        _currentSmallPlace.Value = _currentBigPlace.Value.CreateSmallPlace(smallPlaceName);
        if (_currentSmallPlace.Value == null)
        {
            Debug.LogError($"[PlaceManager] ERROR - SmallPlace '{smallPlaceName}' could not be instantiated in '{_currentBigPlace.Value.BigPlaceName}'!");
            return;
        }

        await _currentSmallPlace.Value.Show();
    }

    /// <summary>
    /// SmallPlace 퇴장 (BigPlace로 복귀)
    /// </summary>
    public async void ExitSmallPlace()
    {
        SmallPlace currentSmallPlace = _currentSmallPlace.Value;
        if (_currentSmallPlace == null)
        {
            Debug.LogWarning("[PlaceManager] No SmallPlace to exit from.");
            return;
        }

        Debug.Log("[PlaceManager] Exiting SmallPlace");
        _currentSmallPlace.Value = null; // ✅ UniRx 감지 → 자동으로 ShowBigPlaceUI(true) 실행됨
        await currentSmallPlace.Hide();
        Destroy(currentSmallPlace.gameObject);
    }
}
