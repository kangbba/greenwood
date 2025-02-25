using System;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

public class KeyScenariosPair
{
    private string _key;  
    private List<Scenario> _scenarios = new List<Scenario>(); // 실행 가능한 시나리오 리스트

    public string Key => _key;
    public List<Scenario> Scenarios => _scenarios; // ✅ 캡슐화된 읽기 전용 리스트

    public KeyScenariosPair(string key, List<Scenario> scenarios)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentNullException(nameof(key), "Key cannot be null or empty.");
        }

        if (scenarios.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(scenarios), "Scenarios cannot be null or empty.");
        }

        _key = key;
        _scenarios = new List<Scenario>(scenarios); // ✅ 내부 리스트 복사하여 유지
    }

    /// <summary>
    /// ✅ KeyScenariosPair를 Dictionary<string, List<Scenario>> 형태로 변환
    /// </summary>
    public Dictionary<string, List<Scenario>> AsDictionary => new Dictionary<string, List<Scenario>>
    {
        { _key, new List<Scenario>(_scenarios) } // ✅ 리스트 복사하여 외부 수정 방지
    };
}
