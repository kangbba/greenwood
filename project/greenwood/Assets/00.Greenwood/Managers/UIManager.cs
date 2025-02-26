using UnityEngine;
using Cysharp.Threading.Tasks;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Canvas References")]
    [SerializeField] private GameCanvas _gameCanvas;   
    [SerializeField] private UICanvas _uiCanvas;   
    [SerializeField] private PopupCanvas _popupCanvas;   
    [SerializeField] private HighestCanvas _highestCanvas;   

    [Header("UI Prefabs")]
    [SerializeField] private AskDialog _askDialogPrefab;
    [SerializeField] private ChoiceSetWindowDouble _choiceSetDoublePrefab;
    [SerializeField] private ChoiceSetWindowMultiple _choiceWindowMultiplePrefab;
    [SerializeField] private DialoguePlayer _dialoguePlayerPrefab;

    public GameCanvas GameCanvas => _gameCanvas;
    public UICanvas UICanvas => _uiCanvas;
    public PopupCanvas PopupCanvas => _popupCanvas;
    public HighestCanvas HighestCanvas => _highestCanvas;
    public DialoguePlayer DialoguePlayerPrefab => _dialoguePlayerPrefab;

    public ChoiceSetWindowDouble ChoiceSetWindowDoublePrefab { get => _choiceSetDoublePrefab; }
    public ChoiceSetWindowMultiple ChoiceSetWindowMultiplePrefab { get => _choiceWindowMultiplePrefab; }

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
        askDialog.FadeFrom(target : 1f, from : 0f, .2f);

        return await askDialog.Initialize(message, yesText, noText);
    }
}
