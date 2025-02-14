using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Story
{
    public abstract List<Element> UpdateElements { get; }

    // 스토리 ID는 자동으로 클래스의 이름을 사용
    public virtual string StoryId => GetType().Name; // 클래스의 이름을 ID로 사용

    
    // _nextStory는 protected 필드로 선언하여 상속받은 클래스에서 수정 가능
    protected Story _nextStory;

    // 외부에서 접근할 수 있는 NextStory 프로퍼티 (get은 public, set은 protected)
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
