using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Sirenix.OdinInspector;

[Serializable]
public class ButtonEntry
{
    [SerializeField] private string _buttonId;  // 버튼 ID (ECharacterName.ToString())
    [SerializeField] private string _buttonText; // ✅ UI에 표시될 버튼 텍스트 (DisplayName)
    [SerializeField] private bool _useCustomPrefab = false; // ✅ 개별 버튼 프리팹 사용 여부

    [ShowIf(nameof(_useCustomPrefab))]
    [SerializeField] private Button _buttonPrefab; // ✅ 개별 프리팹 (선택적)

    public string ButtonId => _buttonId;
    public string ButtonText => _buttonText;
    public bool UseCustomPrefab => _useCustomPrefab;
    public Button ButtonPrefab => _buttonPrefab;

    public ButtonEntry(string buttonId, string buttonText, bool useCustomPrefab = false, Button buttonPrefab = null)
    {
        _buttonId = buttonId;
        _buttonText = buttonText;
        _useCustomPrefab = useCustomPrefab;
        _buttonPrefab = buttonPrefab;
    }
}


public class ButtonGroup : AnimationImage
{
    [SerializeField] private Button _standardBtnPrefab; // ✅ 기본 버튼 프리팹

    [SerializeField] private bool _useAlign; // ✅ Align 기능을 사용할지 여부

    [ShowIf(nameof(_useAlign))]
    [SerializeField] private bool _isVertical; // 정렬 방향 (가로/세로)

    [SerializeField] private Transform _groupContainer; // 버튼 그룹의 부모 오브젝트
    [SerializeField] private float spaceMultiplier = 1f; // 버튼 그룹의 부모 오브젝트
    [SerializeField] private List<ButtonEntry> _buttonEntries = new List<ButtonEntry>(); // ✅ 버튼 바인딩 리스트

    private Dictionary<string, Button> _activeButtons = new Dictionary<string, Button>(); // 현재 활성화된 버튼들

    public List<ButtonEntry> ButtonEntries { get => _buttonEntries; set => _buttonEntries = value; }

    /// <summary>
    /// ✅ 버튼 그룹을 한 번에 설정 (기존 버튼 제거 후 갱신)
    /// </summary>
    public void SetButtonGroup(Dictionary<string, Action> buttonActions)
    {
        ClearButtons();
        foreach (var pair in buttonActions)
        {
            AddButton(pair.Key, pair.Value);
        }

        if (_useAlign) AlignButtons(); // ✅ _useAlign이 true일 때만 실행
    }

    /// <summary>
    /// ✅ 개별 버튼 추가
    /// </summary>
    public Button AddButton(string buttonId, Action onClickAction)
    {
        if (_activeButtons.ContainsKey(buttonId)) return null; // ✅ 중복 추가 방지

        // ✅ ButtonEntry에서 해당 버튼 ID를 찾아 프리팹 확인
        ButtonEntry entry = _buttonEntries.Find(e => e.ButtonId == buttonId);
        Button prefabToUse = (entry != null && entry.UseCustomPrefab && entry.ButtonPrefab != null) 
            ? entry.ButtonPrefab 
            : _standardBtnPrefab;

        if (prefabToUse == null)
        {
            Debug.LogError($"[ButtonGroup] ERROR - No prefab found for button '{buttonId}'!");
            return null;
        }

        // ✅ 버튼 인스턴스 생성
        Button newButton = Instantiate(prefabToUse, _groupContainer);

        // ✅ 기본 버튼을 사용하는 경우 텍스트 변경
        if (entry != null && !entry.UseCustomPrefab)
        {
            TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = entry.ButtonText;
            }
        }

        newButton.onClick.AddListener(() => onClickAction.Invoke());
        _activeButtons[buttonId] = newButton;

        if (_useAlign) AlignButtons(); // ✅ _useAlign이 true일 때만 실행

        return newButton;
    }

    /// <summary>
    /// ✅ 개별 버튼 제거
    /// </summary>
    public void RemoveButton(string buttonId)
    {
        if (_activeButtons.TryGetValue(buttonId, out Button button))
        {
            Destroy(button.gameObject);
            _activeButtons.Remove(buttonId);
        }

        if (_useAlign) AlignButtons(); // ✅ _useAlign이 true일 때만 실행
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
    }

    /// <summary>
    /// ✅ 버튼을 **완벽하게 중앙 정렬** (_useAlign이 true일 때만 적용)
    /// </summary>
    private void AlignButtons()
    {
        if (!_useAlign) return; // ✅ 정렬을 사용하지 않는 경우 실행 안 함

        List<RectTransform> buttonTransforms = new List<RectTransform>();
        float totalSize = 0f;
        int buttonCount = _activeButtons.Count;

        foreach (var button in _activeButtons.Values)
        {
            RectTransform buttonTransform = button.GetComponent<RectTransform>();
            buttonTransforms.Add(buttonTransform);
            totalSize += _isVertical ? buttonTransform.sizeDelta.y : buttonTransform.sizeDelta.x;
        }

        float centerOffset = totalSize / 2f; // ✅ 전체 크기의 반을 구함 (중심점 계산)
        float positionOffset = -centerOffset; // ✅ 시작점을 중심에서 조정

        for (int i = 0; i < buttonCount; i++)
        {
            RectTransform buttonTransform = buttonTransforms[i];
            float buttonSize = _isVertical ? buttonTransform.sizeDelta.y : buttonTransform.sizeDelta.x;

            if (_isVertical)
            {
                buttonTransform.anchoredPosition = new Vector2(0, -positionOffset + (buttonSize / 2));
                positionOffset += buttonSize * spaceMultiplier;
            }
            else
            {
                buttonTransform.anchoredPosition = new Vector2(positionOffset + (buttonSize / 2), 0);
                positionOffset += buttonSize * spaceMultiplier;
            }
        }
    }
}
