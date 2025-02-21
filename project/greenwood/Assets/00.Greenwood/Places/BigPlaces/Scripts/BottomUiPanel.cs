using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public enum BottomUiButtonType
{
    GoingOut,        // 외출 버튼
    ExitSmallPlace,  // 스몰 플레이스 나가기 버튼
    Rest             // 휴식 버튼
}

[System.Serializable]
public class BottomUiButtonMapping
{
    public BottomUiButtonType buttonType;
    public Button buttonPrefab;
}

public class BottomUiPanel : AnimationImage
{
    [SerializeField] private Image _img;
    [SerializeField] private List<BottomUiButtonMapping> _buttonMappings; // ✅ 버튼 타입과 프리팹을 직접 매핑

    private Dictionary<BottomUiButtonType, Button> _activeButtons = new Dictionary<BottomUiButtonType, Button>();
    private Dictionary<BottomUiButtonType, Button> _buttonPrefabDict = new Dictionary<BottomUiButtonType, Button>(); // ✅ 버튼 타입 → 프리팹 딕셔너리

    private void Awake()
    {
        _buttonPrefabDict = _buttonMappings.ToDictionary(mapping => mapping.buttonType, mapping => mapping.buttonPrefab);

        // ✅ UI가 처음에는 꺼진 상태로 시작
        FadeOut(0f);
    }

    /// <summary>
    /// ✅ 버튼을 한 번에 설정 (기존 버튼은 제거 후 갱신)
    /// </summary>
    public void SetButtons(Dictionary<BottomUiButtonType, System.Action> buttonActions)
    {
        // ✅ 기존 버튼 제거
        ClearButtons();

        // ✅ 딕셔너리에 있는 버튼만 생성
        foreach (var pair in buttonActions)
        {
            AddButton(pair.Key, pair.Value);
        }
    }

    /// <summary>
    /// ✅ 버튼을 추가하고 자동으로 UI를 활성화 (내부 전용)
    /// </summary>
    private void AddButton(BottomUiButtonType buttonType, System.Action onClickAction)
    {

        if (_activeButtons.ContainsKey(buttonType)) return; // ✅ 중복 추가 방지

        if (!_buttonPrefabDict.TryGetValue(buttonType, out Button prefab) || prefab == null)
        {
            Debug.LogWarning($"[BottomUiPanel] No prefab found for button type: {buttonType}");
            return;
        }

        Button newButton = Instantiate(prefab, _img.transform);
        newButton.onClick.AddListener(() => onClickAction.Invoke());
        _activeButtons[buttonType] = newButton;

        AlignButtons();
    }


    /// <summary>
    /// ✅ 모든 버튼 제거
    /// </summary>
    public void ClearButtons()
    {

        foreach (var button in _activeButtons.Values)
        {
            Destroy(button.gameObject);
        }
        _activeButtons.Clear();

        AlignButtons();
    }

    /// <summary>
    /// ✅ 현재 버튼 상태에 따라 UI 자동 페이드 인/아웃 및 버튼 정렬
    /// </summary>
    private void AlignButtons()
    {
        // ✅ 버튼을 우측 정렬하여 배치
        float spacing = 10f; // 버튼 간격
        float buttonWidth = _activeButtons.Values.Count > 0 ? _activeButtons.Values.First().GetComponent<RectTransform>().sizeDelta.x : 100f;
        float totalWidth = (_activeButtons.Count * buttonWidth) + ((_activeButtons.Count - 1) * spacing);

        float startX = _img.rectTransform.rect.width / 2 - totalWidth; // 오른쪽 끝에서 시작

        int index = 0;
        foreach (var button in _activeButtons.Values)
        {
            RectTransform buttonTransform = button.GetComponent<RectTransform>();
            buttonTransform.anchoredPosition = new Vector2(startX + index * (buttonWidth + spacing), buttonTransform.anchoredPosition.y);
            index++;
        }
    }
}
