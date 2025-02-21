using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// SequentialElement 클래스는 주어진 리스트의 Element들을 순차적으로 실행하는 Element입니다.
/// 여러 효과를 연속적으로 실행할 때 사용됩니다.
/// </summary>
public class SequentialElement : Element
{
    private readonly List<Element> _elements;

    // ✅ `List<Element>`를 받도록 변경
    public SequentialElement(List<Element> elements)
    {
        _elements = elements ?? new List<Element>(); // ✅ null 방지
    }

    public List<Element> Elements => _elements; // ✅ 읽기 전용 프로퍼티

    public override void ExecuteInstantly()
    {
        foreach (var element in _elements)
        {
            element.ExecuteInstantly();
        }
    }

    public override async UniTask ExecuteAsync()
    {
        foreach (var element in _elements)
        {
            if (element != null)
            {
                await element.ExecuteAsync(); // ✅ 하나씩 순차 실행
            }
        }
    }
}
