using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class BigPlace : MonoBehaviour
{
    [Header("BigPlace Settings")]
    [SerializeField] private EBigPlaceName _bigPlaceName;
    [SerializeField] private Transform _smallPlaceSpawnParent;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Transform _btnsParent; // ✅ 버튼 UI 관리

    [Header("SmallPlace Button Mappings")]
    [SerializeField] private List<SmallPlaceButtonMapping> _smallPlaceButtons;

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
                mapping.button.onClick.AddListener(() => EnterSmallPlace(smallPlaceName));
            }
        }
    }

    /// <summary>
    /// UI 버튼 활성화 및 비활성화 처리
    /// </summary>
    private async UniTask SetButtonsUIActive(bool isActive, float duration)
    {
        _btnsParent.gameObject.SetAnimActive(isActive, duration);
        await UniTask.WaitForSeconds(duration);
    }

    /// <summary>
    /// Exit 버튼 활성화 및 비활성화 처리
    /// </summary>
    private async UniTask SetExitButtonActive(bool isActive, float duration)
    {
        _exitButton.gameObject.SetAnimActive(isActive, duration);
        await UniTask.WaitForSeconds(duration);
    }

    private async void EnterSmallPlace(ESmallPlaceName smallPlaceName)
    {
        Debug.Log($"[BigPlace] ({_bigPlaceName}) Entering SmallPlace: {smallPlaceName}");

        if (!_smallPlaces.TryGetValue(smallPlaceName, out SmallPlace smallPlacePrefab))
        {
            Debug.LogError($"[BigPlace] ERROR - SmallPlace '{smallPlaceName}' not found!");
            return;
        }

        // ✅ UI 버튼 비활성화 (클릭 방지)
        await SetButtonsUIActive(false, 0.3f);

        // ✅ 항상 BigPlace → SmallPlace 전환이므로 기존 SmallPlace 제거 로직 필요 없음
        _currentSmallPlace = Instantiate(smallPlacePrefab, _smallPlaceSpawnParent);
        await _currentSmallPlace.Show();

        // ✅ 특정 조건을 추가하여 실행 가능 (현재는 항상 실행)
        if (true) 
        {
            Debug.Log($"[BigPlace] Starting Story after entering {smallPlaceName}.");
            await StoryService.ExecuteStorySequence(new TestStory());
        }
        await SetExitButtonActive(true, 0.3f);
    }

    private async void ExitSmallPlace()
    {
        // ✅ Exit 버튼 비활성화
        await SetExitButtonActive(false, 0.5f);

        if (_currentSmallPlace != null)
        {
            await _currentSmallPlace.Hide();
            Destroy(_currentSmallPlace.gameObject);
            _currentSmallPlace = null;
        }

        // ✅ UI 버튼 다시 활성화
        await SetButtonsUIActive(true, 0.3f);
    }
}
