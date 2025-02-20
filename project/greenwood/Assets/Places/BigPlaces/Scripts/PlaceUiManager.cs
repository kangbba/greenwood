using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class PlaceUiManager : MonoBehaviour
{
    public static PlaceUiManager Instance { get; private set; }

    [SerializeField] private Image _bottomUiPanel;
    [SerializeField] private Button _enterSmallPlaceBtnPrefab;
    [SerializeField] private Button _exitSmallPlaceBtnPrefab;

    private List<Button> _enterPlaceBtns = new List<Button>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // ✅ SmallPlace UI 표시
    public async UniTask ShowSmallPlaceUI(SmallPlace smallPlace)
    {
        Debug.Log($"[PlaceUiManager] Showing SmallPlace UI: {smallPlace.SmallPlaceName}");
        // ✅ Exit 버튼 생성 후 _bottomUiPanel의 자식으로 추가
        _bottomUiPanel.gameObject.SetAnim(false, 0f);
        Button exitButton = Instantiate(_exitSmallPlaceBtnPrefab, _bottomUiPanel.transform);
        exitButton.onClick.AddListener(() =>
        {
            Debug.Log("[PlaceUiManager] ExitSmallPlace button clicked.");
            PlaceManager.Instance.ExitSmallPlace();
        });
        _bottomUiPanel.gameObject.SetAnim(true, 1f);
        await UniTask.WaitForSeconds(1f);
    }

    // ✅ SmallPlace UI 숨김
    public async UniTask HideSmallPlaceUI()
    {
        Debug.Log("[PlaceUiManager] Hiding SmallPlace UI");
        _bottomUiPanel.gameObject.SetAnim(false, 1f);
        await UniTask.WaitForSeconds(1f);
        _bottomUiPanel.transform.DestroyAllChildren();
    }

    // ✅ BigPlace UI 표시 (SmallPlace 버튼 생성)
    public async UniTask ShowBigPlaceUI(BigPlace bigPlace)
    {
        Debug.Log($"[PlaceUiManager] Showing BigPlace UI: {bigPlace.BigPlaceName}");
        await CreateEnterPlaceBtns(bigPlace, PlaceManager.Instance.EnterSmallPlace);
    }

    // ✅ BigPlace UI 숨김 (SmallPlace 버튼 제거)
    public async UniTask HideBigPlaceUI()
    {
        Debug.Log("[PlaceUiManager] Hiding BigPlace UI");
        await DestroyEnterPlaceBtns();
    }

    // ✅ SmallPlace 이동 버튼 동적 생성
    private async UniTask CreateEnterPlaceBtns(BigPlace bigPlace, System.Action<ESmallPlaceName> enterSmallPlaceAction)
    {
        foreach (var location in bigPlace.SmallPlaceLocations)
        {
            Debug.Log($"[PlaceUiManager] Creating Button for SmallPlace: {location.SmallPlaceName}");

            Button newButton = Instantiate(_enterSmallPlaceBtnPrefab, location.transform);
            newButton.transform.localPosition = Vector2.zero;
            newButton.gameObject.SetAnimToFrom(true, false, 1f); // ✅ 애니메이션 적용
            newButton.onClick.AddListener(() =>
            {
                Debug.Log($"[PlaceUiManager] EnterSmallPlace Button Clicked: {location.SmallPlaceName}");
                enterSmallPlaceAction.Invoke(location.SmallPlaceName);
            });
            _enterPlaceBtns.Add(newButton);
        }

        await UniTask.WaitForSeconds(1f);
    }

    // ✅ SmallPlace 버튼 제거
    private async UniTask DestroyEnterPlaceBtns()
    {
        Debug.Log("[PlaceUiManager] Destroying all EnterPlace Buttons...");

        foreach (var button in _enterPlaceBtns)
        {
            Debug.Log($"[PlaceUiManager] Destroying Button: {button.name}");
            button.gameObject.SetAnimDestroy(1f);
        }
        _enterPlaceBtns.Clear();

        await UniTask.WaitForSeconds(1f);
    }

}
