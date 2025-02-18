using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;

public class Dialogue : Element
{
    private ECharacterName _characterName;  // 캡슐화된 캐릭터 ID
    private List<string> _sentences;       // 캡슐화된 대사 목록
    private List<string> _importantWords;       // 캡슐화된 대사 목록
    private float _speed;

    // 읽기 전용 프로퍼티
    public ECharacterName CharacterName => _characterName;
    public List<string> Sentences => _sentences;
    public float Speed => _speed;
    public List<string> ImportantWords { get => _importantWords; }

    /// <summary>
    /// 생성자
    /// </summary>
    public Dialogue(ECharacterName characterName, List<string> sentences, List<string> importantWords = null, float speedMultiplier = 1f)
    {
        _characterName = characterName;
        _sentences = sentences;
        _importantWords = importantWords;
        _speed = 1500 * speedMultiplier;
    }
    public Dialogue(ECharacterName characterName, string sentence, string importantWord = null, float speedMultiplier = 1f)
    {
        _characterName = characterName;
        _sentences = new List<string>(){sentence};
        _importantWords = new List<string>(){importantWord};
        _speed = 1500 * speedMultiplier;
    }

    /// <summary>
    /// 즉시 스킵(완료) 로직 (필요하다면 추가 구현)
    /// </summary>
    public override void ExecuteInstantly()
    {
        DialogueService.SkipCurrentDialogue();
    }

    /// <summary>
    /// 다이얼로그 실행 → DialogueService로 위임
    /// </summary>
    public override async UniTask ExecuteAsync()
    {
        if (_sentences == null || _sentences.Count == 0)
        {
            Debug.LogWarning("Dialogue :: _sentences is null or empty");
            return;
        }

        await DialogueService.ShowDialogue(this);
    }
}
