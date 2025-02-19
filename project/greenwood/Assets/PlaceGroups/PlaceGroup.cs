using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlaceGroup : MonoBehaviour
{
    [Header("Place Group Settings")]
    [SerializeField] private EPlaceGroupName _placeGroupName;
    [SerializeField] private Transform _placeSpawnParent;
    [SerializeField] private Button _exitButton; // ExitButton 미리 바인딩됨
    [SerializeField] private List<PlaceButtonMapping> _placeButtons = new List<PlaceButtonMapping>();

    [Header("Place Prefabs")]
    [SerializeField] private List<Place> _placePrefabs;

    private Dictionary<EPlaceName, Place> _places = new Dictionary<EPlaceName, Place>();
    private Place _currentPlace;


    public EPlaceGroupName PlaceGroupName => _placeGroupName;

    private void Awake()
    {
        LoadPlaces();
        SetupButtonListeners();
        _exitButton.onClick.AddListener(ExitPlace);
    }

    private void LoadPlaces()
    {
        foreach (var place in _placePrefabs)
        {
            if (!_places.ContainsKey(place.PlaceName))
            {
                _places[place.PlaceName] = place;
            }
            else
            {
                Debug.LogError($"[PlaceGroup] Duplicate Place Name detected: {place.PlaceName}");
            }
        }
    }

    private void SetupButtonListeners()
    {
        foreach (var mapping in _placeButtons)
        {
            if (mapping.button != null)
            {
                EPlaceName placeName = mapping.placeName;
                mapping.button.onClick.AddListener(() => OnPlaceButtonClicked(placeName));
            }
        }
    }

    private void OnPlaceButtonClicked(EPlaceName placeName)
    {
        Debug.Log($"[PlaceGroup] ({_placeGroupName}) Button clicked: Moving to {placeName}");

        if (_places.TryGetValue(placeName, out Place placePrefab))
        {
            ShowPlace(placePrefab);
        }
        else
        {
            Debug.LogError($"[PlaceGroup] ERROR - Place '{placeName}' not found!");
        }
    }

    private void ShowPlace(Place placePrefab)
    {
        if (_currentPlace != null)
        {
            Destroy(_currentPlace.gameObject);
        }

        Place newPlace = Instantiate(placePrefab, _placeSpawnParent);
        _currentPlace = newPlace;
        _currentPlace.Show(0.5f);

        _exitButton.gameObject.SetAnimActive(true, 0.5f); // Place 진입 시 ExitButton 활성화
    }

    private void ExitPlace()
    {
        if (_currentPlace != null)
        {
            _currentPlace.Hide(0.5f);
            Destroy(_currentPlace.gameObject, 0.5f);
            _currentPlace = null;
        }

        _exitButton.gameObject.SetAnimActive(false, 0.5f); // Place 나갈 때 ExitButton 비활성화
    }
}
