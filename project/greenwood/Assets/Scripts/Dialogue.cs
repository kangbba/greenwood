using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;

public class Dialogue : Element
{
    private string _characterID;          // 캡슐화된 캐릭터 ID
    private List<string> _sentences;          // 캡슐화된 대사 목록

    // 읽기 전용 프로퍼티
    public string CharacterID => _characterID;

    public List<string> Sentences { get => _sentences;  }

    /// <summary>
    /// 생성자
    /// </summary>
    public Dialogue(string characterID, List<string> lines)
    {
        _characterID = characterID;
        _sentences = lines;
    }

    /// <summary>
    /// 즉시 스킵(완료) 로직 (필요하다면 추가 구현)
    /// </summary>
    public override void ExecuteInstantly()
    {
        // 예: 즉시 표시, 페이드 스킵 등
    }

    /// <summary>
    /// 실제 재생은 DialogueController에 위임
    /// </summary>
    public override async UniTask ExecuteAsync()
    {
        if (_sentences == null || _sentences.Count == 0)
        {
            Debug.LogWarning("Dialogue :: _sentences is null or empty");
            return;
        }

        await DialogueController.Instance.PlayDialogue(this);
    }
}
