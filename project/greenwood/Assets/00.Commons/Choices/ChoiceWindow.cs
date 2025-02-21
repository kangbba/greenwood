using Cysharp.Threading.Tasks;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public abstract class ChoiceSetWindow : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI _questionText;
    [SerializeField] protected Image _background;
    protected UniTaskCompletionSource<int> _choiceCompletionSource;

    /// <summary>
    /// UI 초기화 (질문 설정) - 모든 서브 클래스에서 필수 구현
    /// </summary>
    public abstract void Init(string question);

    /// <summary>
    /// 선택지를 설정하고 사용자의 선택을 기다림 - 모든 서브 클래스에서 필수 구현
    /// </summary>
    public abstract UniTask<int> ShowChoices(List<ChoiceContent> choices);

}
