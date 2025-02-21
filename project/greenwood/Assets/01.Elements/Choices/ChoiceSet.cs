using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceSet : Element
{
    private string _question;
    private List<ChoiceContent> _choiceContents;

    public ChoiceSet(string question, List<ChoiceContent> choices)
    {
        _question = question;
        _choiceContents = choices ?? new List<ChoiceContent>();
    }

    public override void ExecuteInstantly()
    {
        // 즉시 실행 로직 (선택된 값 기반으로 동작)
    }
    public override async UniTask ExecuteAsync()
    {
        Debug.Log($"[ChoiceSet] 질문 표시: {_question}");

        // UIManager를 통해 선택지 UI 실행
        int selectedChoiceIndex = await ChoiceService.WaitForChoicecSetWindowResult(this);

        if (selectedChoiceIndex >= 0 && selectedChoiceIndex < _choiceContents.Count)
        {
            Debug.Log($"[ChoiceSet] 선택된 인덱스: {selectedChoiceIndex}");
            ChoiceService.CloseCurrentChoiceSet(1f);
            await UniTask.WaitForSeconds(1f);
            await _choiceContents[selectedChoiceIndex].ExecuteAsync();
        }
        else
        {
            Debug.LogWarning("[ChoiceSet] 유효하지 않은 선택");
        }
    }

    public string Question => _question;
    public List<ChoiceContent> Choices => _choiceContents;
}
