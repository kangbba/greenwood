using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(RectMask2D))]
public class RevealingWord : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMesh; // 미리 설정된 TextMeshProUGUI
    private RectMask2D _mask;
    private RectTransform _maskTransform;
    private float _remainingPadding;
    private bool _isPaused;
    private bool _isPlaying;

    public RectTransform RectTransform => GetComponent<RectTransform>();
    public TextMeshProUGUI TextMesh => _textMesh;

    private void Awake()
    {
        _mask = GetComponent<RectMask2D>();
        _maskTransform = GetComponent<RectTransform>();
    }

    /// <summary>
    /// 텍스트 초기화 및 마스크 설정
    /// </summary>
    public void Init(string text)
    {
        if (_textMesh == null)
        {
            Debug.LogError("TextMeshProUGUI가 설정되지 않았습니다!");
            return;
        }

        _textMesh.text = text;
        AdjustMaskAndTextSize();
    }

    /// <summary>
    /// 텍스트 길이에 맞춰 마스크 크기 조정
    /// </summary>
    private void AdjustMaskAndTextSize()
    {
        float textWidth = _textMesh.preferredWidth;
        _maskTransform.sizeDelta = new Vector2(textWidth, _maskTransform.sizeDelta.y);
        _textMesh.rectTransform.sizeDelta = new Vector2(textWidth, _maskTransform.sizeDelta.y);

        // 초기에는 오른쪽 패딩을 textWidth로 줘서 텍스트를 모두 숨김
        _mask.padding = new Vector4(0, 0, textWidth, 0);
        _remainingPadding = textWidth;
    }

    /// <summary>
    /// 기존 텍스트에 추가 텍스트를 붙이고, 마스크 크기를 갱신하는 함수
    /// </summary>
    public void AppendText(string extraText)
    {
        // 기존 텍스트에 extraText 이어붙임
        _textMesh.text += extraText;
        // 전체 너비 재계산 및 마스크, RectTransform 업데이트
        float newWidth = _textMesh.preferredWidth;
        _maskTransform.sizeDelta = new Vector2(newWidth, _maskTransform.sizeDelta.y);
        _textMesh.rectTransform.sizeDelta = new Vector2(newWidth, _maskTransform.sizeDelta.y);
        _mask.padding = new Vector4(0, 0, newWidth, 0);
        _remainingPadding = newWidth;
    }

    /// <summary>
    /// 마스크의 오른쪽 패딩을 0으로 줄이며 텍스트를 나타나게 함
    /// </summary>
    public async UniTask Play(float speed)
    {
        if (_isPlaying) return; // 이미 실행 중이면 중복 실행 방지

        _isPlaying = true;
        _isPaused = false;

        while (_remainingPadding > 0)
        {
            if (_isPaused)
            {
                await UniTask.WaitUntil(() => !_isPaused);
            }

            _remainingPadding = Mathf.Max(0, _remainingPadding - (speed * Time.deltaTime));
            _mask.padding = new Vector4(0, 0, _remainingPadding, 0);
            await UniTask.Yield();
        }

        _mask.padding = new Vector4(0, 0, 0, 0);
        _isPlaying = false;
    }

    /// <summary>
    /// 애니메이션 일시 정지
    /// </summary>
    public void Pause() => _isPaused = true;

    /// <summary>
    /// 애니메이션 다시 시작
    /// </summary>
    public void Resume() => _isPaused = false;

    /// <summary>
    /// 즉시 모든 텍스트 표시
    /// </summary>
    public void CompleteInstantly()
    {
        _isPaused = false;
        _isPlaying = false;
        _mask.padding = new Vector4(0, 0, 0, 0);
        _remainingPadding = 0;
    }
}
