using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public static class StoryService
{
    public static async UniTask ExecuteStorySequence(Story story)
    {
        while (story != null)
        {
            Debug.Log($"🚀 Executing Story: {story.StoryId}");

            await ExecuteElementsSequence(story.UpdateElements);

            story = story.NextStory;
        }

        Debug.Log("✅ All stories have been executed.");
    }

    private static async UniTask ExecuteElementsSequence(List<Element> elements)
    {
        foreach (Element element in elements)
        {
            await element.ExecuteAsync();
        }
    }

    public static void SkipStory(Story story)
    {
        if (story != null)
        {
            Debug.Log($"⏩ Skipping Story: {story.StoryId}");
            story.ExecuteInstantlyAll();
        }
    }

    public static void SkipToElementIndex(Story story, int index)
    {
        if (story != null)
        {
            Debug.Log($"⏩ Skipping {story.StoryId} to Element Index {index}");
            story.ExecuteInstantlyTillElementIndex(index);
        }
    }
}
