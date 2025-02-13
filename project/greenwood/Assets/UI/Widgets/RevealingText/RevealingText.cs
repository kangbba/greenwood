using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(RectMask2D))]
public class RevealingText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMesh; // 미리 설정된 TextMeshProUGUI
    private RectMask2D _mask;
    
    private RectTransform _maskTransform;

    private float _remainingPadding;
    private float _speed;
    private bool _isPaused;
    private bool _isPlaying;

    public RectTransform RectTransform => GetComponent<RectTransform>();
    public TextMeshProUGUI TextMesh { get => _textMesh;  }

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

        // 텍스트 설정
        _textMesh.text = text;
        // 텍스트 크기 기반으로 마스크 크기 조정
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

        _mask.padding = new Vector4(0, 0, textWidth, 0); // 처음엔 모든 글자를 가림
        _remainingPadding = textWidth;
    }

    /// <summary>
    /// 마스크의 오른쪽 패딩을 0으로 줄이며 텍스트를 나타나게 함
    /// </summary>
    public async UniTask Play(float speed)
    {
        if (_isPlaying) return; // 이미 실행 중이면 중복 실행 방지

        _speed = speed;
        _isPlaying = true;
        _isPaused = false;

        while (_remainingPadding > 0)
        {
            if (_isPaused)
            {
                await UniTask.WaitUntil(() => !_isPaused);
            }

            _remainingPadding = Mathf.Max(0, _remainingPadding - (_speed * Time.deltaTime));
            _mask.padding = new Vector4(0, 0, _remainingPadding, 0);
            await UniTask.Yield();
        }

        _mask.padding = new Vector4(0, 0, 0, 0);
        _isPlaying = false;
    }

    /// <summary>
    /// 애니메이션 일시 정지
    /// </summary>
    public void Pause()
    {
        _isPaused = true;
    }

    /// <summary>
    /// 애니메이션 다시 시작
    /// </summary>
    public void Resume()
    {
        if (_isPaused)
        {
            _isPaused = false;
        }
    }

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
