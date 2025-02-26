using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class Choice : Element
{
    private string _question;
    private List<ChoiceOption> _choiceContents;

    public Choice(string question, List<ChoiceOption> choices)
    {
        _question = question;
        _choiceContents = choices ?? new List<ChoiceOption>();
    }

    public override void ExecuteInstantly()
    {
        // 즉시 실행 로직 (선택된 값 기반으로 동작)
    }
    public override async UniTask ExecuteAsync()
    {
        Debug.Log($"[Choice] 질문 표시: {_question}");

        // UIManager를 통해 선택지 UI 실행
        int selectedChoiceIndex = await ChoiceService.WaitForChoiceWindowResult(this);

        if (selectedChoiceIndex >= 0 && selectedChoiceIndex < _choiceContents.Count)
        {
            Debug.Log($"[Choice] 선택된 인덱스: {selectedChoiceIndex}");
            ChoiceService.CloseCurrentChoice(1f);
            await UniTask.WaitForSeconds(1f);
            await _choiceContents[selectedChoiceIndex].ExecuteAsync();
        }
        else
        {
            Debug.LogWarning("[Choice] 유효하지 않은 선택");
        }
    }

    public string Question => _question;
    public List<ChoiceOption> Choices => _choiceContents;
}
