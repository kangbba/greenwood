using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using TMPro;

public class ItemUI : MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _itemDescText;
    [SerializeField] private Button _confirmButton;

    private UniTaskCompletionSource<bool> _confirmationTcs;

    /// <summary>
    /// ✅ 아이템 UI 초기화
    /// </summary>
    public void Init(ItemData itemData)
    {
        _itemImage.sprite = itemData.Thumbnail;
        _itemNameText.text = itemData.DisplayName;
        _itemDescText.text = itemData.Description;

        _confirmationTcs = new UniTaskCompletionSource<bool>();
        _confirmButton.onClick.AddListener(() => _confirmationTcs.TrySetResult(true));
    }

    /// <summary>
    /// ✅ 확인 버튼 클릭을 대기
    /// </summary>
    public async UniTask WaitForConfirmation()
    {
        await _confirmationTcs.Task;
    }
}
