using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;

public class ChoiceSetWindowDouble : ChoiceSetWindow
{
    [SerializeField] private ChoiceButton _leftChoiceButton;
    [SerializeField] private ChoiceButton _rightChoiceButton;

    public override void Init(string question)
    {
        _background.gameObject.SetAnim(false, 0f);
        _background.gameObject.SetAnim(true, .3f);
        _questionText.text = question;
    }

    public override async UniTask<int> ShowChoices(List<ChoiceContent> choices)
    {
        if (choices.Count != 2)
        {
            Debug.LogError("❌ ChoiceSetWindowDouble requires exactly 2 choices.");
            return -1;
        }

        _choiceCompletionSource = new UniTaskCompletionSource<int>();

        // 🔥 버튼에 클릭 이벤트 추가
        _leftChoiceButton.Init(choices[0].Title, 0, SelectChoice);
        _rightChoiceButton.Init(choices[1].Title, 1, SelectChoice);

        return await _choiceCompletionSource.Task;
    }
    
    /// <summary>
    /// 선택한 버튼의 인덱스를 반환하고 창을 닫음
    /// </summary>
    private void SelectChoice(int index)
    {
        Debug.Log($"✅ 선택된 버튼 Index: {index}");
        _choiceCompletionSource?.TrySetResult(index);
    }
}
