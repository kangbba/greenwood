using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ✅ `SaveData` 기반 아이템 저장 방식
/// </summary>
public class GameSaveDataProvider : IOwnedItemProvider
{
    private Dictionary<string, int> _ownedItems = new Dictionary<string, int>();

    public void AddItem(string itemId, int amount)
    {
        if (_ownedItems.ContainsKey(itemId))
            _ownedItems[itemId] += amount;
        else
            _ownedItems[itemId] = amount;

        Debug.Log($"[GameSaveDataProvider] ✅ '{itemId}' 추가됨. 개수: {_ownedItems[itemId]}");
    }

    public bool RemoveItem(string itemId, int amount)
    {
        if (!_ownedItems.ContainsKey(itemId) || _ownedItems[itemId] < amount)
        {
            Debug.LogWarning($"[GameSaveDataProvider] ❌ '{itemId}' 아이템 부족!");
            return false;
        }

        _ownedItems[itemId] -= amount;
        if (_ownedItems[itemId] <= 0)
        {
            _ownedItems.Remove(itemId);
        }

        Debug.Log($"[GameSaveDataProvider] 🗑️ '{itemId}' 사용. 남은 개수: {_ownedItems.GetValueOrDefault(itemId, 0)}");
        return true;
    }

    public int GetItemCount(string itemId)
    {
        return _ownedItems.GetValueOrDefault(itemId, 0);
    }
}
