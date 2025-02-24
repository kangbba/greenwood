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
    public string buttonId;  // 버튼 ID (외부에서 바인딩)

    [Tooltip("개별 버튼 프리팹을 사용할 경우 체크")]
    public bool useCustomPrefab = false; // ✅ 개별 버튼 프리팹 사용 여부

    [ShowIf(nameof(useCustomPrefab))] // ✅ 개별 프리팹을 사용할 때만 노출
    public Button buttonPrefab;

    [HideIf(nameof(useCustomPrefab))] // ✅ 기본 버튼을 사용할 때만 노출
    public string buttonText;
}

public class ButtonGroup : AnimationImage
{
    [SerializeField] private Button _standardBtnPrefab; // ✅ 기본 버튼 프리팹

    [SerializeField] private bool _useAlign = true; // ✅ Align 기능을 사용할지 여부

    [ShowIf(nameof(_useAlign))]
    [SerializeField] private bool _isVertical; // 정렬 방향 (가로/세로)

    [ShowIf(nameof(_useAlign))]
    [SerializeField] private float _spacing = 10f; // 버튼 간격

    [SerializeField] private Transform _groupContainer; // 버튼 그룹의 부모 오브젝트
    [SerializeField] private List<ButtonEntry> _buttonEntries = new List<ButtonEntry>(); // ✅ 버튼 바인딩 리스트

    private Dictionary<string, Button> _activeButtons = new Dictionary<string, Button>(); // 현재 활성화된 버튼들

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
    public void AddButton(string buttonId, Action onClickAction)
    {
        if (_activeButtons.ContainsKey(buttonId)) return; // ✅ 중복 추가 방지

        // ✅ ButtonEntry에서 해당 버튼 ID를 찾아 프리팹 확인
        ButtonEntry entry = _buttonEntries.Find(e => e.buttonId == buttonId);
        Button prefabToUse = (entry != null && entry.useCustomPrefab && entry.buttonPrefab != null) 
            ? entry.buttonPrefab 
            : _standardBtnPrefab;

        if (prefabToUse == null)
        {
            Debug.LogError($"[ButtonGroup] ERROR - No prefab found for button '{buttonId}'!");
            return;
        }

        // ✅ 버튼 인스턴스 생성
        Button newButton = Instantiate(prefabToUse, _groupContainer);

        // ✅ 기본 버튼을 사용하는 경우 텍스트 변경
        if (entry != null && !entry.useCustomPrefab)
        {
            TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = entry.buttonText;
            }
        }

        newButton.onClick.AddListener(() => onClickAction.Invoke());
        _activeButtons[buttonId] = newButton;

        if (_useAlign) AlignButtons(); // ✅ _useAlign이 true일 때만 실행
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
    /// ✅ 버튼 자동 정렬 (_useAlign이 true일 때만 적용)
    /// </summary>
    private void AlignButtons()
    {
        if (!_useAlign) return; // ✅ 정렬을 사용하지 않는 경우 실행 안 함

        float positionOffset = 0f;

        foreach (var button in _activeButtons.Values)
        {
            RectTransform buttonTransform = button.GetComponent<RectTransform>();

            if (_isVertical)
            {
                buttonTransform.anchoredPosition = new Vector2(0, -positionOffset);
                positionOffset += buttonTransform.sizeDelta.y + _spacing;
            }
            else
            {
                buttonTransform.anchoredPosition = new Vector2(positionOffset, 0);
                positionOffset += buttonTransform.sizeDelta.x + _spacing;
            }
        }
    }
}
