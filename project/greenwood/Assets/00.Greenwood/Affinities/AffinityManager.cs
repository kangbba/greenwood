using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class AffinityManager : MonoBehaviour
{
    public static AffinityManager Instance { get; private set; }

    private Dictionary<string, ReactiveProperty<int>> _affinityData = new Dictionary<string, ReactiveProperty<int>>();

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
    /// 특정 캐릭터의 친밀도를 가져옴 (기본값 0)
    /// </summary>
    public int GetAffinity(string characterName)
    {
        return _affinityData.ContainsKey(characterName) ? _affinityData[characterName].Value : 0;
    }

    /// <summary>
    /// 특정 캐릭터의 친밀도를 증가
    /// </summary>
    public void IncreaseAffinity(string characterName, int amount)
    {
        if (!_affinityData.ContainsKey(characterName))
            _affinityData[characterName] = new ReactiveProperty<int>(0);

        _affinityData[characterName].Value += amount;
        Debug.Log($"[AffinityManager] {characterName}의 친밀도가 {amount} 증가 → 현재 친밀도: {_affinityData[characterName].Value}");
    }

    /// <summary>
    /// 특정 캐릭터의 친밀도를 감소 (최소 0)
    /// </summary>
    public void DecreaseAffinity(string characterName, int amount)
    {
        if (!_affinityData.ContainsKey(characterName))
            _affinityData[characterName] = new ReactiveProperty<int>(0);

        _affinityData[characterName].Value = Mathf.Max(0, _affinityData[characterName].Value - amount);
        Debug.Log($"[AffinityManager] {characterName}의 친밀도가 {amount} 감소 → 현재 친밀도: {_affinityData[characterName].Value}");
    }
}
