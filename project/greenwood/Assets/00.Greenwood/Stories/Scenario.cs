using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class Scenario : Element
{
    public abstract List<Element> UpdateElements {get;}

    // 스토리 ID는 자동으로 클래스의 이름을 사용
    public virtual string ScenarioId => GetType().Name; // 클래스의 이름을 ID로 사용


    public override async UniTask ExecuteAsync()
    {
        for(int i = 0 ; i < UpdateElements.Count ; i++)
        {
            await UpdateElements[i].ExecuteAsync();
        }
        await new AllScenarioElementsClear(.3f).ExecuteAsync();
    }

    public override void ExecuteInstantly()
    {
        for(int i = 0 ; i < UpdateElements.Count ; i++){
            Element element = UpdateElements[i];
            Debug.Log($"{i}번째 엘리먼트인 {element.GetType().Name}은 스킵");
            element.ExecuteInstantly();
        }
        new AllScenarioElementsClear(0f).ExecuteInstantly();
    }

    public CharacterEnter GetFirstCharacterEnter()
    {
        foreach (var element in UpdateElements)
        {
            if (element is CharacterEnter characterEnter)
            {
                return characterEnter;
            }
        }
        return null;
    }


    public void ReadyForScenarioStart()
    {
        foreach (Element element in UpdateElements)
        {
            if (element is CharacterEnter || element is EmotionChange)
            {
                element.ExecuteInstantly(); // ✅ 첫 장면에 필요한 요소만 즉시 적용
            }
            else
            {
                Debug.Log($"[Scenario] {element.GetType().Name}은(는) 첫 장면 연출에 필요하지 않아 제외됨.");
            }
        }
    }

}
