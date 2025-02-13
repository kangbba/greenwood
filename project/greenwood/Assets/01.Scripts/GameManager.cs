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
            "NPC1", 
            new List<string>
            {
                "안녕하세요! 첫 번째 대사입니다. 첫번째 대사는! 좀 길게 말을 해보겠습니다.",
                "두 번째 문장도, 출력해 봅시다."
            }
        ));

        _elements.Add(new Dialogue(
            "NPC2", 
            new List<string>
            {
                "여기는 두 번째 대화입니다.",
                "조금 더 길게, 말을 해 볼까요?",
                "문장부호도 있습니다!"
            }
        ));

        _elements.Add(new Dialogue(
            "NPC3", 
            new List<string>
            {
                "마지막 대화입니다.",
                "긴 여행이었네요.",
                "이제 곧 끝납니다!"
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
