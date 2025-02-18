using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Story
{
    public abstract List<Element> UpdateElements { get; }

    // 스토리 ID는 자동으로 클래스의 이름을 사용
    public virtual string StoryId => GetType().Name; // 클래스의 이름을 ID로 사용

    protected Story _nextStory;

    public Story NextStory
    {
        get => _nextStory;
        protected set => _nextStory = value;
    }

    public void ExecuteInstantlyAll(){
        for(int i = 0 ; i < UpdateElements.Count ; i++){
            Element element = UpdateElements[i];
            Debug.Log($"{i}번째 엘리먼트인 {element.GetType().Name}은 스킵");
            element.ExecuteInstantly();
        }
    }
    public void ExecuteInstantlyTillElementIndex(int count){
        for(int i = 0 ; i < count ; i++){
            Element element = UpdateElements[i];
            Debug.Log($"{i}번째 엘리먼트인 {element.GetType().Name}은 스킵");
            element.ExecuteInstantly();
        }
    }
}
