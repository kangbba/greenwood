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

    public Dictionary<ESmallPlaceName, List<KeyScenariosPair>> GenerateRandomSchedule()
    {
        var kapsDict = new Dictionary<ESmallPlaceName, List<KeyScenariosPair>>();

        // ✅ Cafe
        var cafeScenarios = new List<KeyScenariosPair>
        {
            new KeyScenariosPair(CafeActionTypes.Talk, ScenarioData.CafeTalk),
            new KeyScenariosPair(CafeActionTypes.Order, ScenarioData.CafeOrder)
        };
        kapsDict[ESmallPlaceName.Cafe] = cafeScenarios;

        Debug.Log($"[GenerateRandomSchedule] ✅ CafeKaps 생성 완료. 총 {cafeScenarios.Count}개.");

        // ✅ Bakery
        var bakeryScenarios = new List<KeyScenariosPair>
        {
            new KeyScenariosPair(BakeryActionTypes.Talk, ScenarioData.BakeryTalk),
            new KeyScenariosPair(BakeryActionTypes.Buy, ScenarioData.BakeryBuy)
        };
        kapsDict[ESmallPlaceName.Bakery] = bakeryScenarios;

        Debug.Log($"[GenerateRandomSchedule] ✅ BakeryKaps 생성 완료. 총 {bakeryScenarios.Count}개.");

        // ✅ Herbshop
        var herbshopScenarios = new List<KeyScenariosPair>
        {
            new KeyScenariosPair(HerbshopActionTypes.Buy, ScenarioData.HerbshopBuy),
            new KeyScenariosPair(HerbshopActionTypes.Heal, ScenarioData.HerbshopHeal)
        };
        kapsDict[ESmallPlaceName.Herbshop] = herbshopScenarios;

        Debug.Log($"[GenerateRandomSchedule] ✅ HerbshopKaps 생성 완료. 총 {herbshopScenarios.Count}개.");

        return kapsDict;
    }


}
