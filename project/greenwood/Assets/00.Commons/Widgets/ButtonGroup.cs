using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[Serializable]
public class ButtonEntry
{
    public string buttonId;    // 버튼 ID (외부에서 바인딩)
    public Button buttonPrefab; // 버튼 프리팹 (외부에서 바인딩)
}

public class ButtonGroup : MonoBehaviour
{
    [SerializeField] private bool _isHorizontal = true; // 기본적으로 가로 정렬
    [SerializeField] private float _spacing = 10f; // 버튼 간격
    [SerializeField] private Transform _groupContainer; // 버튼 그룹의 부모 오브젝트
    [SerializeField] private List<ButtonEntry> _buttonEntries = new List<ButtonEntry>(); // ✅ 버튼 바인딩 리스트

    private Dictionary<string, Button> _activeButtons = new Dictionary<string, Button>(); // 현재 활성화된 버튼들
    private Dictionary<string, Button> _buttonPrefabs = new Dictionary<string, Button>(); // 바인딩된 버튼 프리팹

    private void Awake()
    {
        // ✅ 바인딩된 버튼 프리팹을 딕셔너리로 변환
        foreach (var entry in _buttonEntries)
        {
            if (!_buttonPrefabs.ContainsKey(entry.buttonId))
            {
                _buttonPrefabs[entry.buttonId] = entry.buttonPrefab;
            }
        }
    }

    public void Init(bool isHorizontal, float spacing = 10f)
    {
        _isHorizontal = isHorizontal;
        _spacing = spacing;
    }

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
        AlignButtons();
    }

    /// <summary>
    /// ✅ 개별 버튼 추가
    /// </summary>
    public void AddButton(string buttonId, Action onClickAction)
    {
        if (_activeButtons.ContainsKey(buttonId)) return; // ✅ 중복 추가 방지

        // ✅ _buttonPrefabs 에 등록되지 않은 ID일 경우 강제 차단
        if (!_buttonPrefabs.ContainsKey(buttonId))
        {
            Debug.LogError($"[ButtonGroup] ERROR - Button '{buttonId}' is not registered in _buttonEntries! Cannot add.");
            return;
        }

        Button prefab = _buttonPrefabs[buttonId];
        Button newButton = Instantiate(prefab, _groupContainer);
        newButton.onClick.AddListener(() => onClickAction.Invoke());
        _activeButtons[buttonId] = newButton;

        AlignButtons();
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
    }

    /// <summary>
    /// ✅ 버튼 자동 정렬
    /// </summary>
    private void AlignButtons()
    {
        float positionOffset = 0f;

        foreach (var button in _activeButtons.Values)
        {
            RectTransform buttonTransform = button.GetComponent<RectTransform>();

            if (_isHorizontal)
            {
                buttonTransform.anchoredPosition = new Vector2(positionOffset, 0);
                positionOffset += buttonTransform.sizeDelta.x + _spacing;
            }
            else
            {
                buttonTransform.anchoredPosition = new Vector2(0, -positionOffset);
                positionOffset += buttonTransform.sizeDelta.y + _spacing;
            }
        }
    }
}
