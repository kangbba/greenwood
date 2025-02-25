using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// ✅ 아이템 보유 데이터를 관리하는 인터페이스 (ItemManager가 직접 SaveData를 참조하지 않도록 분리)
/// </summary>
public interface IOwnedItemProvider
{
    void AddItem(string itemId, int amount);
    bool RemoveItem(string itemId, int amount);
    int GetItemCount(string itemId);
}

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }

    [SerializeField] private ItemUI _itemUiPrefab; // ✅ ItemUI 프리팹 바인딩
    private Dictionary<string, ItemData> _itemDictionary = new Dictionary<string, ItemData>();

    private IOwnedItemProvider _itemProvider; // ✅ 아이템 저장소

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadAllItems();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    /// <summary>
    /// ✅ `IOwnedItemProvider`를 설정 (외부에서 주입 가능)
    /// </summary>
    public void SetItemProvider(IOwnedItemProvider provider)
    {
        _itemProvider = provider;
    }

    /// <summary>
    /// ✅ Resources/Items 폴더에서 모든 ItemData 로드
    /// </summary>
    private void LoadAllItems()
    {
        _itemDictionary.Clear();
        ItemData[] items = Resources.LoadAll<ItemData>("Items");

        foreach (var item in items)
        {
            if (!_itemDictionary.ContainsKey(item.ItemId))
            {
                _itemDictionary[item.ItemId] = item;
            }
            else
            {
                Debug.LogWarning($"[ItemManager] 중복된 아이템 ID 발견: {item.ItemId}");
            }
        }

        Debug.Log($"[ItemManager] ✅ {items.Length}개의 아이템 로드 완료.");
    }

    /// <summary>
    /// ✅ 아이템 ID를 기반으로 ItemData 가져오기
    /// </summary>
    public ItemData GetItemData(string itemId)
    {
        if (_itemDictionary.TryGetValue(itemId, out var itemData))
        {
            return itemData;
        }
        else
        {
            Debug.LogWarning($"[ItemManager] ❌ 아이템 ID '{itemId}' 찾을 수 없음!");
            return null;
        }
    }

    /// <summary>
    /// ✅ 아이템 추가 (UI 표시 없이 저장만)
    /// </summary>
    public void AddItem(string itemId, int amount = 1)
    {
        if (_itemProvider == null)
        {
            Debug.LogError("[ItemManager] ❌ 아이템 저장소가 설정되지 않음!");
            return;
        }

        if (!_itemDictionary.ContainsKey(itemId))
        {
            Debug.LogError($"[ItemManager] ❌ 존재하지 않는 아이템 ID: {itemId}");
            return;
        }

        _itemProvider.AddItem(itemId, amount);
    }

    /// <summary>
    /// ✅ 아이템 UI 표시 (확인 버튼 클릭 시 닫힘)
    /// </summary>
    public async UniTask ShowItemUI(string itemId)
    {
        ItemData itemData = GetItemData(itemId);
        if (itemData == null)
        {
            Debug.LogError("[ItemManager] ❌ ShowItemUI() 호출 시 itemData가 null!");
            return;
        }

        // ✅ UI 생성 및 초기화
        ItemUI itemUI = Instantiate(_itemUiPrefab, UIManager.Instance.PopupCanvas.ItemGainLayer);
        itemUI.Init(itemData);

        // ✅ 확인 버튼 클릭 대기
        await itemUI.WaitForConfirmation();

        // ✅ UI 제거
        Destroy(itemUI.gameObject);
    }

    /// <summary>
    /// ✅ 아이템 사용 / 삭제
    /// </summary>
    public bool RemoveItem(string itemId, int amount = 1)
    {
        if (_itemProvider == null)
        {
            Debug.LogError("[ItemManager] ❌ 아이템 저장소가 설정되지 않음!");
            return false;
        }

        return _itemProvider.RemoveItem(itemId, amount);
    }

    /// <summary>
    /// ✅ 아이템 개수 조회
    /// </summary>
    public int GetItemCount(string itemId)
    {
        if (_itemProvider == null)
        {
            Debug.LogError("[ItemManager] ❌ 아이템 저장소가 설정되지 않음!");
            return 0;
        }

        return _itemProvider.GetItemCount(itemId);
    }
}
