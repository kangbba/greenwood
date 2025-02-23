using System;
using UnityEngine;

public class PlaceUIManager : MonoBehaviour
{
    public static PlaceUIManager Instance { get; private set; }

    [SerializeField] private ButtonGroup bottomButtonGroupPrefab;
    private ButtonGroup _currentBottomButtonGroup; // ✅ 변수명 변경
    public ButtonGroup BottomButtonGroup => _currentBottomButtonGroup; // ✅ 외부에서 접근 가능

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        
        // ✅ 초기 생성 (기본적으로 존재해야 하는 경우)
        _currentBottomButtonGroup = Instantiate(bottomButtonGroupPrefab, UIManager.Instance.UICanvas.PlaceUiLayer);   
    }

}
