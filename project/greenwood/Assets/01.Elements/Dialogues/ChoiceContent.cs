using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public class ChoiceContent
{
    private string _title;
    private List<Element> _elements;

    public string Title => _title;
    public List<Element> Elements => _elements;

    public ChoiceContent(string title, List<Element> elements)
    {
        _title = title;
        _elements = elements;
    }

    public async UniTask ExecuteAsync()
    {
        foreach (var element in _elements)
        {
            await element.ExecuteAsync();
        }
    }
}
