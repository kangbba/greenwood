using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private Story _currentStory; // 현재 실행 중인 스토리

    private void Awake()
    {
        // 싱글톤 초기화
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private async void Start()
    {
        // 최초 Story 지정 (테스트 스토리)
        _currentStory = new TestStory();
        
        // 스토리 실행
        await ExecuteStorySequence(_currentStory);
    }

    /// <summary>
    /// 현재 Story를 실행하고, NextStory가 있다면 자동 진행
    /// </summary>
    private async UniTask ExecuteStorySequence(Story story)
    {
        while (story != null)
        {
            _currentStory = story;
            Debug.Log($"🚀 Executing Story: {_currentStory.StoryId}");

            await ExecuteElementsSequence(_currentStory.UpdateElements);

            // 다음 스토리로 이동
            story = story.NextStory;
        }

        Debug.Log("✅ All stories have been executed.");
    }

    /// <summary>
    /// Story 내부의 Elements 리스트를 순서대로 실행
    /// </summary>
    private async UniTask ExecuteElementsSequence(List<Element> elements)
    {
        foreach (Element element in elements)
        {
            await element.ExecuteAsync();
        }
    }

    /// <summary>
    /// 현재 실행 중인 스토리를 즉시 완료
    /// </summary>
    public void SkipCurrentStory()
    {
        if (_currentStory != null)
        {
            Debug.Log($"⏩ Skipping Story: {_currentStory.StoryId}");
            _currentStory.ExecuteInstantlyAll();
        }
    }

    /// <summary>
    /// 현재 Story의 특정 요소까지 즉시 완료
    /// </summary>
    public void SkipToElementIndex(int index)
    {
        if (_currentStory != null)
        {
            Debug.Log($"⏩ Skipping {_currentStory.StoryId} to Element Index {index}");
            _currentStory.ExecuteInstantlyTillElementIndex(index);
        }
    }
}
