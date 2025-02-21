using Cysharp.Threading.Tasks;
using UnityEngine;

public static class ChoiceService
{
    private static ChoiceSetWindowDouble _currentChoiceWindowDouble;
    private static ChoiceSetWindowMultiple _currentChoiceWindowMultiple;

    /// <summary>
    /// 선택지 창을 생성하고, 사용자의 선택을 기다림
    /// </summary>
    public static async UniTask<int> WaitForChoiceSetWindowResult(ChoiceSet choiceSet)
    {
        // 기존 선택지 창이 있으면 제거
        CloseCurrentChoiceSet(0f);

        int choiceCount = choiceSet.Choices.Count;
        
        if (choiceCount == 2)
        {
            _currentChoiceWindowDouble = Object.Instantiate(
                UIManager.Instance.ChoiceSetWindowDoublePrefab, 
                UIManager.Instance.PopupCanvas.transform
            );
            _currentChoiceWindowDouble.Init(choiceSet.Question);
            return await _currentChoiceWindowDouble.ShowChoices(choiceSet.Choices);
        }
        else
        {
            _currentChoiceWindowMultiple = Object.Instantiate(
                UIManager.Instance.ChoiceSetWindowMultiplePrefab, 
                UIManager.Instance.PopupCanvas.transform
            );
            _currentChoiceWindowMultiple.Init(choiceSet.Question);
            return await _currentChoiceWindowMultiple.ShowChoices(choiceSet.Choices);
        }
    }

    /// <summary>
    /// 현재 열린 선택지 창을 즉시 종료
    /// </summary>
    public static void CloseCurrentChoiceSet(float duration)
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
