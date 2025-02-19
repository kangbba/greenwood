using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance { get; private set; }

    private Story _currentStory;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private async void Start()
    {
        Debug.Log("📖 Initializing Story...");
        
        // 최초 Story 지정 (테스트 스토리)
        _currentStory = new TestStory();

        // 스토리 실행
        await ExecuteStorySequence(_currentStory);
    }

    private async UniTask ExecuteStorySequence(Story story)
    {
        while (story != null)
        {
            _currentStory = story;
            Debug.Log($"🚀 Executing Story: {_currentStory.StoryId}");

            await ExecuteElementsSequence(_currentStory.UpdateElements);

            story = story.NextStory;
        }

        Debug.Log("✅ All stories have been executed.");
    }

    private async UniTask ExecuteElementsSequence(List<Element> elements)
    {
        foreach (Element element in elements)
        {
            await element.ExecuteAsync();
        }
    }

    public void SkipCurrentStory()
    {
        if (_currentStory != null)
        {
            Debug.Log($"⏩ Skipping Story: {_currentStory.StoryId}");
            _currentStory.ExecuteInstantlyAll();
        }
    }

    public void SkipToElementIndex(int index)
    {
        if (_currentStory != null)
        {
            Debug.Log($"⏩ Skipping {_currentStory.StoryId} to Element Index {index}");
            _currentStory.ExecuteInstantlyTillElementIndex(index);
        }
    }
}
