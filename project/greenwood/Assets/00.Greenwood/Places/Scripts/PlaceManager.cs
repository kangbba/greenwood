using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using static BigPlaceNames;
using static SmallPlaceNames;

public class PlaceManager : MonoBehaviour
{
    public static PlaceManager Instance { get; private set; }

    private Dictionary<EBigPlaceName, BigPlace> _bigPlacePrefabs = new();
    private Dictionary<ESmallPlaceName, SmallPlace> _smallPlacePrefabs = new();

    private List<BigPlace> _activeBigPlaces = new();
    private List<SmallPlace> _activeSmallPlaces = new();

    [Header("UI Prefabs")]
    [SerializeField] private BigPlaceUI _bigPlaceUiPrefab;
    [SerializeField] private SmallPlaceUI _smallPlaceUiPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadPlacePrefabs();
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

    private void LoadPlacePrefabs()
    {
        // ✅ BigPlace 프리팹 로드
        BigPlace[] bigPlaceArray = Resources.LoadAll<BigPlace>("Places/BigPlaces");
        foreach (var place in bigPlaceArray)
        {
            _bigPlacePrefabs[place.BigPlaceName] = place;
        }

        // ✅ SmallPlace 프리팹 로드
        SmallPlace[] smallPlaceArray = Resources.LoadAll<SmallPlace>("Places/SmallPlaces");
        foreach (var place in smallPlaceArray)
        {
            _smallPlacePrefabs[place.SmallPlaceName] = place;
        }
    }

    public void Init()
    {
        foreach (EBigPlaceName bigPlaceName in Enum.GetValues(typeof(EBigPlaceName)))
        {
            BigPlace prefab = GetBigPlacePrefab(bigPlaceName);
            if (prefab == null) continue;

            BigPlace newBigPlace = Instantiate(prefab, UIManager.Instance.GameCanvas.BigPlaceLayer);
            newBigPlace.Init();
            newBigPlace.FadeOut(0f);
            _activeBigPlaces.Add(newBigPlace);
        }

        SubscribeToPlayerNotifications();
    }

    private void SubscribeToPlayerNotifications()
    {
        Debug.Log("주의 : 구독시작");
        PlayerManager.Instance.CurrentBigPlace
            .Subscribe(OnBigPlaceChanged)
            .AddTo(this);

        PlayerManager.Instance.CurrentSmallPlace
            .Subscribe(OnSmallPlaceChanged)
            .AddTo(this);
    }

    private void OnBigPlaceChanged(BigPlace newBigPlace)
    {
        if (newBigPlace == null)
        {
            ExitCurrentBigPlace(0.5f);
            return;
        }

        MoveBigPlace(newBigPlace.BigPlaceName, 0.5f);
    }

    private void OnSmallPlaceChanged(SmallPlace newSmallPlace)
    {
        if (newSmallPlace == null)
        {
            ExitSmallPlace(0.5f);
            return;
        }

        EnterSmallPlace(newSmallPlace.SmallPlaceName, 0.5f);
    }

    public BigPlace GetBigPlace(EBigPlaceName placeName)
    {
        return _activeBigPlaces.FirstOrDefault(bp => bp.BigPlaceName == placeName);
    }

    public SmallPlace GetSmallPlace(ESmallPlaceName smallPlaceName)
    {
        return _activeSmallPlaces.FirstOrDefault(sp => sp.SmallPlaceName == smallPlaceName);
    }

    public BigPlace GetBigPlacePrefab(EBigPlaceName placeName)
    {
        return _bigPlacePrefabs.TryGetValue(placeName, out var place) ? place : null;
    }

    public SmallPlace GetSmallPlacePrefab(ESmallPlaceName smallPlaceName)
    {
        return _smallPlacePrefabs.TryGetValue(smallPlaceName, out var place) ? place : null;
    }

    private void MoveBigPlace(EBigPlaceName placeName, float duration)
    {
        foreach (var bigPlace in _activeBigPlaces)
        {
            bigPlace.FadeOut(duration);
        }

        DestroyAllActiveSmallPlaces();

        BigPlace newBigPlace = GetBigPlace(placeName);
        if (newBigPlace != null)
        {
            newBigPlace.FadeIn(duration);

            foreach (var door in newBigPlace.SmallPlaceDoors)
            {
                SmallPlace smallPlace = CreateSmallPlace(door.SmallPlaceName);
                if (smallPlace != null)
                {
                    _activeSmallPlaces.Add(smallPlace);
                    smallPlace.FadeOut(0f);
                }
            }
        }
        else
        {
            Debug.LogError($"[PlaceManager] `{placeName}` BigPlace 인스턴스 없음!");
        }
    }

    private SmallPlace CreateSmallPlace(ESmallPlaceName smallPlaceName)
    {
        SmallPlace prefab = GetSmallPlacePrefab(smallPlaceName);
        if (prefab == null)
        {
            Debug.LogError($"[PlaceManager] `{smallPlaceName}` SmallPlace 프리팹 없음!");
            return null;
        }

        SmallPlace newSmallPlace = Instantiate(prefab, UIManager.Instance.GameCanvas.SmallPlaceLayer);
        newSmallPlace.Init();
        return newSmallPlace;
    }

    private void ExitCurrentBigPlace(float duration)
    {
        foreach (var bigPlace in _activeBigPlaces)
        {
            bigPlace.FadeOut(duration);
        }

        DestroyAllActiveSmallPlaces();
    }

    private void EnterSmallPlace(ESmallPlaceName smallPlaceName, float duration)
    {
        SmallPlace smallPlace = GetSmallPlace(smallPlaceName);
        if (smallPlace == null)
        {
            Debug.LogError($"[PlaceManager] `{smallPlaceName}` SmallPlace 인스턴스 없음!");
            return;
        }

        smallPlace.FadeIn(duration);
    }

    private void ExitSmallPlace(float duration)
    {
        foreach (var smallPlace in _activeSmallPlaces)
        {
            smallPlace.FadeOut(duration);
        }
    }

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
}
