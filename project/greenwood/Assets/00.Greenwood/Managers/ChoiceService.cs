using Cysharp.Threading.Tasks;
using UnityEngine;

public static class ChoiceService
{
    private static ChoiceWindowDouble _currentChoiceWindowDouble;
    private static ChoiceWindowMultiple _currentChoiceWindowMultiple;

    /// <summary>
    /// 선택지 창을 생성하고, 사용자의 선택을 기다림
    /// </summary>
    public static async UniTask<int> WaitForChoiceWindowResult(Choice Choice)
    {
        // 기존 선택지 창이 있으면 제거
        CloseCurrentChoice(0f);

        int choiceCount = Choice.Choices.Count;
        
        if (choiceCount == 2)
        {
            _currentChoiceWindowDouble = Object.Instantiate(
                UIManager.Instance.ChoiceWindowDoublePrefab, 
                UIManager.Instance.PopupCanvas.transform
            );
            _currentChoiceWindowDouble.Init(Choice.Question);
            return await _currentChoiceWindowDouble.ShowChoices(Choice.Choices);
        }
        else
        {
            _currentChoiceWindowMultiple = Object.Instantiate(
                UIManager.Instance.ChoiceWindowMultiplePrefab, 
                UIManager.Instance.PopupCanvas.transform
            );
            _currentChoiceWindowMultiple.Init(Choice.Question);
            return await _currentChoiceWindowMultiple.ShowChoices(Choice.Choices);
        }
    }

    /// <summary>
    /// 현재 열린 선택지 창을 즉시 종료
    /// </summary>
    public static void CloseCurrentChoice(float duration)
    {
        if (_currentChoiceWindowDouble != null)
        {
            _currentChoiceWindowDouble.FadeAndDestroy(duration);
            _currentChoiceWindowDouble = null;
        }

        if (_currentChoiceWindowMultiple != null)
        {
            _currentChoiceWindowMultiple.FadeAndDestroy(duration);
            _currentChoiceWindowMultiple = null;
        }
    }
}
