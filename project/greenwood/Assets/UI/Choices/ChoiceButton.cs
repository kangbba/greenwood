using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ChoiceButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _choiceText;
    [SerializeField] private Button _button;

    private int _choiceIndex;
    private Action<int> _onClickAction;

    public void Init(string text, int index, Action<int> onClick)
    {
        _choiceIndex = index;
        _choiceText.text = text;
        _onClickAction = onClick;

        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() => _onClickAction?.Invoke(_choiceIndex));
    }
}
