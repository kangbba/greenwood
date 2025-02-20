
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
    public ReactiveProperty<BigPlace> CurrentBigPlace { get => _currentBigPlace; }
    public ReactiveProperty<SmallPlace> CurrentSmallPlace { get => _currentSmallPlace; }
    public List<BigPlace> BigPlacePrefabs { get => _bigPlacePrefabs; }

    [Header("BigPlace Prefabs")]
    [SerializeField] private List<BigPlace> _bigPlacePrefabs;

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
    }


    /// <summary>
    /// 새로운 BigPlace로 이동 (기존 BigPlace 제거 후 생성)
    /// </summary>
    public async UniTask MoveBigPlace(EBigPlaceName newPlace)
    {
        BigPlace currentBigPlace = _currentBigPlace.Value;

        // ✅ 동일한 BigPlace로 이동하려고 하면 경고 로그 출력 후 중단
        if (currentBigPlace != null && currentBigPlace.BigPlaceName == newPlace)
        {
            Debug.LogWarning($"[PlaceManager] MoveBigPlace aborted: Already in '{newPlace}'!");
            return;
        }

        if (currentBigPlace != null)
        {
            _currentBigPlace.Value = null;
            await currentBigPlace.HideAndDestroy();
        }

        // ✅ 새로운 BigPlace 생성
        BigPlace instBigPlace = CreateBigPlace(newPlace);
        if(instBigPlace != null){

            await instBigPlace.Show();
        }
        else{
            Debug.LogWarning("inst big place is null");
        }
    }

    private BigPlace CreateBigPlace(EBigPlaceName placeName)
    {
        // ✅ 프리팹 리스트에서 해당 BigPlace 찾기
        BigPlace prefab = _bigPlacePrefabs.Find(bp => bp.BigPlaceName == placeName);

        // ✅ 프리팹이 존재하지 않으면 오류 로그 출력 후 null 반환
        if (prefab == null)
        {
            Debug.LogError($"[PlaceManager] ERROR - BigPlace '{placeName}' not found in prefabs! Check if it's assigned in the inspector.");
            return null;
        }

        // ✅ BigPlace 생성 및 딕셔너리에 추가
        BigPlace instance = Instantiate(prefab, UIManager.Instance.GameCanvas.BigPlaceLayer);
        _currentBigPlace.Value = instance; // ✅ ReactiveProperty 업데이트 → UI 자동 변경

        Debug.Log($"[PlaceManager] Successfully created BigPlace: {placeName}");
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
