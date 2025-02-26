using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }

    [SerializeField] private ItemUI _itemUiPrefab; // ✅ ItemUI 프리팹 바인딩
    private Dictionary<string, ItemData> _allItemDataDictionary = new Dictionary<string, ItemData>(); // ✅ 모든 아이템 정보
    private ReactiveProperty<HashSet<string>> _ownedItemData = new ReactiveProperty<HashSet<string>>(new HashSet<string>()); // ✅ 보유 아이템

    public IReadOnlyReactiveProperty<HashSet<string>> OwnedItemData => _ownedItemData;

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
    /// ✅ Resources/Items 폴더에서 모든 ItemData 로드
    /// </summary>
    private void LoadAllItems()
    {
        _allItemDataDictionary.Clear();
        ItemData[] items = Resources.LoadAll<ItemData>("Items");

        foreach (var item in items)
        {
            if (!_allItemDataDictionary.ContainsKey(item.ItemId))
            {
                _allItemDataDictionary[item.ItemId] = item;
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
        return _allItemDataDictionary.TryGetValue(itemId, out var itemData) ? itemData : null;
    }

    /// <summary>
    /// ✅ 아이템 추가
    /// </summary>
    public void AddItem(string itemId)
    {
        if (!_allItemDataDictionary.ContainsKey(itemId))
        {
            Debug.LogError($"[ItemManager] ❌ 존재하지 않는 아이템 ID: {itemId}");
            return;
        }

        if (!_ownedItemData.Value.Contains(itemId))
        {
            // ✅ 변경 감지를 위해 새 HashSet 생성 후 할당
            var updatedOwnedItems = new HashSet<string>(_ownedItemData.Value) { itemId };
            _ownedItemData.SetValueAndForceNotify(updatedOwnedItems);
            
            Debug.Log($"🛒 [ItemManager] 아이템 획득: {itemId}");
        }
    }

    /// <summary>
    /// ✅ 아이템 보유 여부 확인
    /// </summary>
    public bool HasItem(string itemId)
    {
        return _ownedItemData.Value.Contains(itemId);
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
}
