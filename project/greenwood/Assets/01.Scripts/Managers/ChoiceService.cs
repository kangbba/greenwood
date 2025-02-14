using Cysharp.Threading.Tasks;
using UnityEngine;

public static class ChoiceService
{
    private static ChoiceSetWindow _currentChoiceWindow;

    /// <summary>
    /// 선택지 창을 생성하고, 사용자의 선택을 기다림
    /// </summary>
    public static async UniTask<int> WaitForChoicecSetWindowResult(ChoiceSet choiceSet)
    {
        // 이미 열린 선택지가 있으면 제거
        if (_currentChoiceWindow != null)
        {
            Object.Destroy(_currentChoiceWindow.gameObject);
        }

        if (choiceSet.Choices.Count == 2)
        {
            _currentChoiceWindow = Object.Instantiate(
                UIManager.Instance.ChoiceSetWindowDoublePrefab, 
                UIManager.Instance.PopupCanvas.transform
            );
        }
        else
        {
            _currentChoiceWindow = Object.Instantiate(
                UIManager.Instance.ChoiceSetWindowMultiplePrefab, 
                UIManager.Instance.PopupCanvas.transform
            );
        }

        _currentChoiceWindow.Init(choiceSet.Question);
        return await _currentChoiceWindow.ShowChoices(choiceSet.Choices);
    }

    /// <summary>
    /// 현재 선택지를 즉시 종료
    /// </summary>
    public static void CloseCurrentChoiceSet(float duration)
    {
        if (_currentChoiceWindow != null)
        {
            _currentChoiceWindow.gameObject.SetAnimDestroy(duration);
            _currentChoiceWindow = null;
        }
    }
}
