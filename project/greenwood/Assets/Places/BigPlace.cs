using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BigPlace : MonoBehaviour
{
    [Header("BigPlace Settings")]
    [SerializeField] private EBigPlaceName _bigPlaceName;
    [SerializeField] private Transform _smallPlaceSpawnParent;
    [SerializeField] private Button _exitButton; // ExitButton 미리 바인딩됨
    [SerializeField] private List<SmallPlaceButtonMapping> _smallPlaceButtons = new List<SmallPlaceButtonMapping>();

    [Header("SmallPlace Prefabs")]
    [SerializeField] private List<SmallPlace> _smallPlacePrefabs;

    private Dictionary<ESmallPlaceName, SmallPlace> _smallPlaces = new Dictionary<ESmallPlaceName, SmallPlace>();
    private SmallPlace _currentSmallPlace;

    public EBigPlaceName BigPlaceName => _bigPlaceName;

    private void Awake()
    {
        LoadSmallPlaces();
        SetupButtonListeners();
        _exitButton.onClick.AddListener(ExitSmallPlace);
    }

    private void LoadSmallPlaces()
    {
        foreach (var smallPlace in _smallPlacePrefabs)
        {
            if (!_smallPlaces.ContainsKey(smallPlace.SmallPlaceName))
            {
                _smallPlaces[smallPlace.SmallPlaceName] = smallPlace;
            }
            else
            {
                Debug.LogError($"[BigPlace] Duplicate SmallPlace Name detected: {smallPlace.SmallPlaceName}");
            }
        }
    }

    private void SetupButtonListeners()
    {
        foreach (var mapping in _smallPlaceButtons)
        {
            if (mapping.button != null)
            {
                ESmallPlaceName smallPlaceName = mapping.smallPlaceName;
                mapping.button.onClick.AddListener(() => OnSmallPlaceButtonClicked(smallPlaceName));
            }
        }
    }

    private void OnSmallPlaceButtonClicked(ESmallPlaceName smallPlaceName)
    {
        Debug.Log($"[BigPlace] ({_bigPlaceName}) Button clicked: Moving to {smallPlaceName}");

        if (_smallPlaces.TryGetValue(smallPlaceName, out SmallPlace smallPlacePrefab))
        {
            ShowSmallPlace(smallPlacePrefab);
        }
        else
        {
            Debug.LogError($"[BigPlace] ERROR - SmallPlace '{smallPlaceName}' not found!");
        }
    }

    private void ShowSmallPlace(SmallPlace smallPlacePrefab)
    {
        if (_currentSmallPlace != null)
        {
            Destroy(_currentSmallPlace.gameObject);
        }

        SmallPlace newSmallPlace = Instantiate(smallPlacePrefab, _smallPlaceSpawnParent);
        _currentSmallPlace = newSmallPlace;
        _currentSmallPlace.Show(0.5f);

        _exitButton.gameObject.SetAnimActive(true, 0.5f); // SmallPlace 진입 시 ExitButton 활성화
    }

    private void ExitSmallPlace()
    {
        if (_currentSmallPlace != null)
        {
            _currentSmallPlace.Hide(0.5f);
            Destroy(_currentSmallPlace.gameObject, 0.5f);
            _currentSmallPlace = null;
        }

        _exitButton.gameObject.SetAnimActive(false, 0.5f); // SmallPlace 나갈 때 ExitButton 비활성화
    }
}
