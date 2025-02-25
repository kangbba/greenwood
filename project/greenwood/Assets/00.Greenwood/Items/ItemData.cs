using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Game/Item")]
public class ItemData : ScriptableObject
{
    [SerializeField] private string _itemId;
    [SerializeField] private string _displayName;
    [SerializeField] private string _description;
    [SerializeField] private Sprite _thumbnail; // ✅ 추가됨 (아이템 썸네일)

    public string ItemId => _itemId;
    public string DisplayName => _displayName;
    public string Description => _description;
    public Sprite Thumbnail => _thumbnail;
}
