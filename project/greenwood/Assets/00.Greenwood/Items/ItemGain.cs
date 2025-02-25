using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ItemGain : Element
{
    private string _itemId;

    public ItemGain(string itemId)
    {
        _itemId = itemId;
    }

    public override async UniTask ExecuteAsync()
    {
        ItemData item = ItemManager.Instance.GetItemData(_itemId);

        if (item == null)
        {
            Debug.LogError($"[ItemGain] ❌ 아이템 '{_itemId}'을 찾을 수 없습니다.");
            return;
        }

        Debug.Log($"[ItemGain] ✅ '{item.DisplayName}' 획득!");

        // ✅ 아이템 즉시 추가
        ItemManager.Instance.AddItem(item.ItemId);

        // ✅ 아이템 UI 표시 및 확인 버튼 대기
        await ItemManager.Instance.ShowItemUI(item.ItemId);
    }

    public override void ExecuteInstantly()
    {
        ItemData item = ItemManager.Instance.GetItemData(_itemId);

        if (item == null)
        {
            Debug.LogError($"[ItemGain] ❌ 아이템 '{_itemId}'을 찾을 수 없습니다.");
            return;
        }

        // ✅ UI 없이 즉시 아이템 추가
        ItemManager.Instance.AddItem(item.ItemId);
        Debug.Log($"[ItemGain] ✅ '{item.DisplayName}' 획득! (즉시 실행)");
    }
}
