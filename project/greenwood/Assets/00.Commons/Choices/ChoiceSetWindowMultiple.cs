using Cysharp.Threading.Tasks;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class ChoiceWindowMultiple : AnimationImage
{
    [SerializeField] private TextMeshProUGUI _questionText;
    [SerializeField] private Transform _choiceContainer;
    [SerializeField] private ChoiceButton _choiceButtonPrefab;

    private List<ChoiceButton> _choiceButtons = new List<ChoiceButton>();
    private UniTaskCompletionSource<int> _choiceCompletionSource;

    /// <summary>
    /// ✅ UI 초기화 (질문 설정)
    /// </summary>
    public void Init(string question)
    {
        FadeIn(0.3f); // ✅ Fade In 애니메이션 적용
        _questionText.text = question;
    }

    /// <summary>
    /// ✅ 선택지를 설정하고 사용자의 선택을 기다림
    /// </summary>
    public async UniTask<int> ShowChoices(List<ChoiceOption> choices)
    {
        if (choices.Count < 3)
        {
            Debug.LogError("❌ ChoiceWindowMultiple requires at least 3 choices.");
            return -1;
        }

        _choiceCompletionSource = new UniTaskCompletionSource<int>();

        // 🔥 기존 버튼 제거
        foreach (var button in _choiceButtons)
        {
            Destroy(button.gameObject);
        }
        _choiceButtons.Clear();

        // 🔥 버튼 생성 및 배치
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

        // ✅ UI 닫기
        FadeOut(0.3f);

        return selectedIndex; // 🚀 선택된 인덱스를 반환
    }

    /// <summary>
    /// ✅ 선택된 결과를 저장하고 대기 중인 `UniTask`를 완료
    /// </summary>
    private void SelectChoice(int index)
    {
        Debug.Log($"✅ 선택된 버튼 Index: {index}");
        _choiceCompletionSource?.TrySetResult(index);
    }
}
