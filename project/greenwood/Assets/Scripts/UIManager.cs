using UnityEngine;
using Cysharp.Threading.Tasks;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Canvas References")]
    public Canvas popupCanvas;   // 다이얼로그 UI를 띄울 Canvas

    [Header("UI Prefabs")]
    public GameObject dialogPrefab;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 다이얼로그를 생성하고 사용자의 선택을 반환
    /// </summary>
    public async UniTask<bool?> ShowDialog(string message, string yesText = "확인", string noText = null)
    {
        GameObject dialog = Instantiate(dialogPrefab, popupCanvas.transform);
        DialogBox dialogBox = dialog.GetComponent<DialogBox>();

        return await dialogBox.Initialize(message, yesText, noText);
    }
}
