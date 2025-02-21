using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;

public class ChoiceSetWindowMultiple : ChoiceSetWindow
{
    [SerializeField] private Transform _choiceContainer;
    [SerializeField] private ChoiceButton _choiceButtonPrefab;
    
    private List<ChoiceButton> _choiceButtons = new List<ChoiceButton>();

    public override void Init(string question)
    {
        gameObject.SetAnim(false, 0f);
        gameObject.SetAnim(true, .3f);
        _questionText.text = question;
    }

    public override async UniTask<int> ShowChoices(List<ChoiceContent> choices)
    {
        if (choices.Count < 3)
        {
            Debug.LogError("❌ ChoiceSetWindowMultiple requires at least 3 choices.");
            return -1;
        }

        _choiceCompletionSource = new UniTaskCompletionSource<int>();

        // 🔥 버튼 생성 및 배치
        _choiceButtons.Clear();
        float totalHeight = _choiceContainer.GetComponent<RectTransform>().rect.height;
        float buttonHeight = _choiceButtonPrefab.GetComponent<RectTransform>().sizeDelta.y;
        float spacing = (totalHeight - (buttonHeight * choices.Count)) / (choices.Count - 1);

        for (int i = 0; i < choices.Count; i++)
        {
            ChoiceButton button = Instantiate(_choiceButtonPrefab, _choiceContainer);
            button.Init(choices[i].Title, i, SelectChoice);
            _choiceButtons.Add(button);

            // 🔥 위치 조정
            RectTransform buttonRect = button.GetComponent<RectTransform>();
            buttonRect.anchorMin = new Vector2(0.5f, 1);
            buttonRect.anchorMax = new Vector2(0.5f, 1);
            buttonRect.pivot = new Vector2(0.5f, 1);
            buttonRect.anchoredPosition = new Vector2(0, -i * (buttonHeight + spacing));
        }

        // 선택 대기
        int selectedIndex = await _choiceCompletionSource.Task;

        return selectedIndex; // 🚀 선택된 인덱스를 반환 (창 닫기는 외부에서 수행)
    }

    /// <summary>
    /// 선택된 결과를 저장하고 대기 중인 `UniTask`를 완료 (내부적으로 UI 변경 없음)
    /// </summary>
    private void SelectChoice(int index)
    {
        Debug.Log($"✅ 선택된 버튼 Index: {index}");
        _choiceCompletionSource?.TrySetResult(index);
    }
}
