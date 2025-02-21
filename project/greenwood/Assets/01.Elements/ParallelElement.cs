using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ParallelElement : Element
{
    private readonly List<Element> _elements;

    // ✅ 생성자를 통해 List<Element>를 받도록 변경
    public ParallelElement(List<Element> elements)
    {
        _elements = elements ?? new List<Element>(); // ✅ null 방지
        _elements.RemoveAll(element => element == null); // ✅ null 요소 제거
    }

    public override void ExecuteInstantly()
    {
        // ✅ 각 Element의 ExecuteInstantly 실행
        foreach (var element in _elements)
        {
            element.ExecuteInstantly();
        }
    }

    public override async UniTask ExecuteAsync()
    {
        if (_elements.Count == 0)
        {
            Debug.LogWarning("[ParallelElement] 실행할 요소가 없습니다.");
            return;
        }

        // ✅ 모든 요소를 비동기 실행 (병렬 실행)
        List<UniTask> tasks = new List<UniTask>();
        foreach (var element in _elements)
        {
            tasks.Add(element.ExecuteAsync()); // ✅ 각 요소의 ExecuteAsync 실행
        }

        await UniTask.WhenAll(tasks); // ✅ 모든 작업이 완료될 때까지 대기
    }
}
