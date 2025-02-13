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

    private void Start()
    {
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){

            ShowDialog("안녕하세요", "네", "아니오").Forget();
        }
    }

    /// <summary>
    /// 다이얼로그를 생성하고 사용자의 선택을 반환
    /// </summary>
    public async UniTask<bool?> ShowDialog(string message, string yesText, string noText)
    {
        DialogBox dialogBox = Instantiate(dialogPrefab, popupCanvas.transform).GetComponent<DialogBox>();
        dialogBox.gameObject.SetAnimActive(false,0f);
        dialogBox.gameObject.SetAnimActive(true, .2f);
        return await dialogBox.Initialize(message, yesText, noText);
    }
}
