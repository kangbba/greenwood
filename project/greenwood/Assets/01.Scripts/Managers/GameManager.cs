using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // 테스트용 Elements 리스트
    private List<Element> _elements = new List<Element>();

    private void Awake()
    {
        // 싱글톤 초기화
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 후에도 유지하고 싶다면
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private async void Start()
    {
        // 테스트용 Dialogue 3개를 _elements에 추가
        _elements.Add(new Dialogue(
            CharacterName.Kate, 
            new List<string>
            {
                "어이, 여기 봐! 나 완전 신나는 소식 가져왔어!",
                "들어봐~! 오늘 진짜 재밌는 일이 있었거든!",
                "근데 말이야, 너 진짜 이런 거 관심 없지? 하하!"
            }
        ));

        _elements.Add(new Dialogue(
            CharacterName.Mono, 
            new List<string>
            {
                "조용하다... 이런 적막은 오랜만이네.",
                "그러니까, 내가 여기 온 이유는...",
                "흠, 대체 어디서부터 잘못된 걸까..."
            }
        ));

        _elements.Add(new Dialogue(
            CharacterName.Lisa, 
            new List<string>
            {
                "시간 낭비야. 대체 뭘 기대하는 거지?",
                "흥, 이런 일엔 관심 없어.",
                "됐어. 더 이상 말할 필요 없잖아."
            }
        ));


        // 순서대로 Elements를 실행
        await ExecuteElementsSequence();
    }

    /// <summary>
    /// _elements 리스트를 순서대로 실행
    /// </summary>
    private async UniTask ExecuteElementsSequence()
    {
        foreach (Element element in _elements)
        {
            // Element가 가진 async 실행 메서드 호출
            await element.ExecuteAsync();
        }

        // 모든 요소 실행이 끝났을 때 처리할 로직
        Debug.Log("All elements have been executed.");
    }
}
