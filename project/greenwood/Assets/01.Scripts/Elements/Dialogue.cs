using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;

public class Dialogue : Element
{
    private CharacterName _characterName;  // 캡슐화된 캐릭터 ID
    private List<string> _sentences;       // 캡슐화된 대사 목록
    private float _speed;

    // 읽기 전용 프로퍼티
    public CharacterName CharacterName => _characterName;
    public List<string> Sentences => _sentences;
    public float Speed => _speed;

    /// <summary>
    /// 생성자
    /// </summary>
    public Dialogue(CharacterName characterName, List<string> lines, float speedMultiplier = 1f)
    {
        _characterName = characterName;
        _sentences = lines;
        _speed = 1500 * speedMultiplier;
    }
    public Dialogue(CharacterName characterName, string line, float speedMultiplier = 1f)
    {
        _characterName = characterName;
        _sentences = new List<string>(){line};
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
