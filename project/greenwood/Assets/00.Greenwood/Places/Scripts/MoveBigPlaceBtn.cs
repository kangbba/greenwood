using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using Cysharp.Threading.Tasks;
using static BigPlaceNames;

public class MoveBigPlaceBtn : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] private EBigPlaceName _bigPlaceName; // ✅ SerializedField 추가

    public EBigPlaceName BigPlaceName => _bigPlaceName; // ✅ 프로퍼티 추가

    private ReactiveProperty<bool> _isEnabled = new ReactiveProperty<bool>(false);
    private System.Action<EBigPlaceName> _onSelected;

    private void Awake()
    {
        _button.onClick.AddListener(() =>
        {
            if (_isEnabled.Value)
            {
                Debug.Log($"[MoveBigPlaceBtn] Selected Place: {_bigPlaceName}");
                _onSelected?.Invoke(_bigPlaceName);
            }
        });

        _isEnabled.Subscribe(isActive =>
        {
            _button.interactable = isActive;
            _buttonText.color = isActive ? Color.white : Color.gray;
        })
        .AddTo(this);
    }

    /// <summary>
    /// 버튼을 초기화하고 클릭 이벤트 설정
    /// </summary>
    public void Init(System.Action<EBigPlaceName> onSelected)
    {
        _buttonText.text = _bigPlaceName.ToString();
        _onSelected = onSelected;
        SetEnable(false);
    }

    /// <summary>
    /// 버튼 활성화/비활성화 설정
    /// </summary>
    public void SetEnable(bool isActive)
    {
        _isEnabled.Value = isActive;
    }
}
