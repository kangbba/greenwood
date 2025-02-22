using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public class MonoDialogue : Element
{
    private List<string> _sentences;

    public MonoDialogue(List<string> sentences)
    {
        _sentences = sentences;
    }

    public MonoDialogue(string sentence)
    {
        _sentences = new List<string> { sentence };
    }

    public override void ExecuteInstantly()
    {
    }

    public override async UniTask ExecuteAsync()
    {
        if (_sentences == null || _sentences.Count == 0)
        {
            UnityEngine.Debug.LogWarning("[MonoDialogue] No sentences provided.");
            return;
        }

        // ✅ `ECharacterName.Mono`를 사용하여 Dialogue 실행
        await new Dialogue(ECharacterName.Mono, _sentences).ExecuteAsync();
    }
}
