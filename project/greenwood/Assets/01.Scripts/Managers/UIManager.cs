using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Canvas References")]
    [SerializeField] private Canvas _gameCanvas;   
    [SerializeField] private Canvas _uiCanvas;   
    [SerializeField] private Canvas _popupCanvas;   

    [Header("UI Prefabs")]
    [SerializeField] private AskDialog _askDialogPrefab;
    [SerializeField] private DialoguePlayer _dialoguePlayer;

    public Canvas GameCanvas => _gameCanvas;
    public Canvas UICanvas => _uiCanvas;
    public Canvas PopupCanvas => _popupCanvas;
    public DialoguePlayer DialoguePlayer => _dialoguePlayer;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 다이얼로그를 생성하고 사용자의 선택을 반환
    /// </summary>
    public async UniTask<bool?> ShowAskDialog(string message, string yesText, string noText)
    {
        AskDialog askDialog = Instantiate(_askDialogPrefab, _popupCanvas.transform);
        // 열리는 애니메이션 (예: scale, alpha 등)
        askDialog.gameObject.SetAnimActive(false, 0f);
        askDialog.gameObject.SetAnimActive(true, 0.2f);

        return await askDialog.Initialize(message, yesText, noText);
    }
}
