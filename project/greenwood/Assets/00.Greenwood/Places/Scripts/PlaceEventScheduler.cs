using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using static SmallPlaceNames;

public class PlaceEventScheduler : MonoBehaviour
{
    public static PlaceEventScheduler Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

        /// <summary>
        /// ✅ 시나리오 매니저와 연동해서, OnTalk 같은 액션에서 ExecuteOneScenarioFromList() 호출
        /// </summary>
        public List<SmallPlaceActionPlan> GenerateRandomSchedule()
        {
            // ✅ 결과를 담을 리스트
            var planList = new List<SmallPlaceActionPlan>();

            // Cafe
            var cafePlan = new CafeActionPlan
            {
                OnTalk = () =>
                {
                    // 예시로, 이 안에서 "시나리오 목록"을 만들어서 실행
                    List<Scenario> scenarioList = new List<Scenario>
                    {
                    };
                    // 원하는 시나리오 목록을 넘김
                    ScenarioManager.Instance.ExecuteOneScenarioFromList(scenarioList).Forget();
                },
                OnOrder = () =>
                {
                    // 다른 시나리오 목록 예시
                    List<Scenario> scenarioList = new List<Scenario>
                    {
                    };
                    ScenarioManager.Instance.ExecuteOneScenarioFromList(scenarioList).Forget();
                }
            };
            planList.Add(cafePlan);

            // Bakery
            var bakeryPlan = new BakeryActionPlan
            {
                OnTalk = () =>
                {
                    List<Scenario> scenarioList = new List<Scenario>
                    {
                        new TestStory1()
                    };
                    ScenarioManager.Instance.ExecuteOneScenarioFromList(scenarioList).Forget();
                },
                OnBuy = () =>
                {
                    List<Scenario> scenarioList = new List<Scenario>
                    {
                        new TestStory2()
                    };
                    ScenarioManager.Instance.ExecuteOneScenarioFromList(scenarioList).Forget();
                }
            };
            planList.Add(bakeryPlan);

            // Herbshop
            var herbshopPlan = new HerbshopActionPlan
            {
                OnBuy = () =>
                {
                    List<Scenario> scenarioList = new List<Scenario>
                    {
                    };
                    ScenarioManager.Instance.ExecuteOneScenarioFromList(scenarioList).Forget();
                },
                OnHeal = () =>
                {
                    List<Scenario> scenarioList = new List<Scenario>
                    {
                    };
                    ScenarioManager.Instance.ExecuteOneScenarioFromList(scenarioList).Forget();
                }
            };
            planList.Add(herbshopPlan);

            Debug.Log("[PlaceEventScheduler] 스케줄(List) 생성: RandomFlag 제거 & 시나리오 매니저 호출 방식 적용!");
            return planList;
        }
}
