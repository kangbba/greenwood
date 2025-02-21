using Cysharp.Threading.Tasks;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class ChoiceSetWindowDouble : AnimationImage
{
    [SerializeField] private TextMeshProUGUI _questionText;
    [SerializeField] private ChoiceButton _leftChoiceButton;
    [SerializeField] private ChoiceButton _rightChoiceButton;
    [SerializeField] private Image _background;

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
    /// ✅ 두 개의 선택지를 설정하고 사용자의 선택을 기다림
    /// </summary>
    public async UniTask<int> ShowChoices(List<ChoiceContent> choices)
    {
        if (choices == null || choices.Count != 2)
        {
            Debug.LogError("❌ ChoiceSetWindowDouble requires exactly 2 choices.");
            return -1;
        }

        _choiceCompletionSource = new UniTaskCompletionSource<int>();

        // 🔥 버튼에 클릭 이벤트 추가
        _leftChoiceButton.Init(choices[0].Title, 0, SelectChoice);
        _rightChoiceButton.Init(choices[1].Title, 1, SelectChoice);

        // 선택 대기
        int selectedIndex = await _choiceCompletionSource.Task;

        // ✅ 선택 후 UI 닫기
        FadeOut(0.3f);

        return selectedIndex;
    }
    
    /// <summary>
    /// ✅ 선택한 버튼의 인덱스를 반환하고 창을 닫음
    /// </summary>
    private void SelectChoice(int index)
    {
        _choiceCompletionSource.TrySetResult(index);
    }
}
