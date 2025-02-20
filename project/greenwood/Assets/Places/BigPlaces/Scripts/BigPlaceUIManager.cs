// using UnityEngine;

// public class BigPlaceUIManager : MonoBehaviour
// {
//     public static BigPlaceUIManager Instance { get; private set; }

//     [Header("UI Prefabs")]
//     [SerializeField] private BigPlaceBottomPanel _bottomPanelPrefab;  // ✅ 패널 프리팹

//     private BigPlaceBottomPanel _currentBottomPanel;

//     private void Awake()
//     {
//         if (Instance == null) Instance = this;
//         else
//         {
//             Destroy(gameObject);
//             return;
//         }
//     }

//     public void Show(EBigPlaceName placeName, bool showReturnButton, bool showLeaveButton, float duration)
//     {
//         // ✅ 기존 패널 제거
//         if (_currentBottomPanel != null)
//         {
//             _currentBottomPanel.DestroyPanel();
//             _currentBottomPanel = null;
//         }

//         // ✅ 새로운 패널 생성
//         _currentBottomPanel = Instantiate(_bottomPanelPrefab, transform);
//         _currentBottomPanel.SetupPanel(showReturnButton, showLeaveButton, duration);

//         // ✅ 버튼 동작 설정 (CurrentBigPlace 직접 사용)
//         _currentBottomPanel.SetReturnAction(() => BigPlaceManager.Instance.CurrentBigPlace?.ExitSmallPlace());
//         _currentBottomPanel.SetLeaveAction(() => BigPlaceManager.Instance.MoveBigPlace(EBigPlaceName.Sea));
//     }

// }
