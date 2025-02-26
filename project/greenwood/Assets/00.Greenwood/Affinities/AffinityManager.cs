using System.Collections.Generic;
using UniRx;
using UnityEngine;
using static CharacterEnums;

public class AffinityManager : MonoBehaviour
{
    public static AffinityManager Instance { get; private set; }

    private Dictionary<ECharacterName, ReactiveProperty<int>> _affinityData = new();

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
    /// ✅ 특정 캐릭터의 친밀도를 가져옴 (기본값 0)
    /// </summary>
    public int GetAffinity(ECharacterName character)
    {
        return _affinityData.ContainsKey(character) ? _affinityData[character].Value : 0;
    }

    /// <summary>
    /// ✅ 특정 캐릭터의 친밀도를 증가
    /// </summary>
    public void IncreaseAffinity(ECharacterName character, int amount)
    {
        if (!_affinityData.ContainsKey(character))
            _affinityData[character] = new ReactiveProperty<int>(0);

        _affinityData[character].Value += amount;
        Debug.Log($"[AffinityManager] {character}의 친밀도가 {amount} 증가 → 현재 친밀도: {_affinityData[character].Value}");
    }

    /// <summary>
    /// ✅ 특정 캐릭터의 친밀도를 감소 (최소 0)
    /// </summary>
    public void DecreaseAffinity(ECharacterName character, int amount)
    {
        if (!_affinityData.ContainsKey(character))
            _affinityData[character] = new ReactiveProperty<int>(0);

        _affinityData[character].Value = Mathf.Max(0, _affinityData[character].Value - amount);
        Debug.Log($"[AffinityManager] {character}의 친밀도가 {amount} 감소 → 현재 친밀도: {_affinityData[character].Value}");
    }
}
