using UniRx;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ✅ **키보드 입력을 관리하는 글로벌 매니저**
/// </summary>
public class KeyboardInputManager : MonoBehaviour
{
    public static KeyboardInputManager Instance { get; private set; }

    /// <summary>
    /// ✅ **키 동작 타입 정의**
    /// </summary>
    public enum KeyboardActionType
    {
        SpeedUp,
        LogKey,
        DebugToggle
    }

    /// <summary>
    /// ✅ **각 키 동작의 입력 상태를 저장하는 ReactiveProperty**
    /// </summary>
    private readonly Dictionary<KeyboardActionType, ReactiveProperty<bool>> _keyStates =
        new Dictionary<KeyboardActionType, ReactiveProperty<bool>>();

    /// <summary>
    /// ✅ **키 동작별 매핑된 KeyCode 목록**
    /// </summary>
    private readonly Dictionary<KeyboardActionType, List<KeyCode>> _keyMappings =
        new Dictionary<KeyboardActionType, List<KeyCode>>()
        {
            { KeyboardActionType.SpeedUp, new List<KeyCode> { KeyCode.LeftControl, KeyCode.RightControl, KeyCode.LeftCommand, KeyCode.RightCommand } },
            { KeyboardActionType.LogKey, new List<KeyCode> { KeyCode.F12 } },
            { KeyboardActionType.DebugToggle, new List<KeyCode> { KeyCode.F1 } }
        };

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

        DontDestroyOnLoad(gameObject); // ✅ 씬 변경 시에도 유지

        // ✅ 키 상태를 ReactiveProperty로 초기화
        foreach (var action in System.Enum.GetValues(typeof(KeyboardActionType)))
        {
            _keyStates[(KeyboardActionType)action] = new ReactiveProperty<bool>(false);
        }

        // ✅ UniRx로 키 입력 감지
        Observable.EveryUpdate()
            .Subscribe(_ => CheckKeys())
            .AddTo(this);
    }

    /// <summary>
    /// ✅ **각 키 입력을 감지하여 상태 업데이트**
    /// </summary>
    private void CheckKeys()
    {
        foreach (var action in _keyMappings.Keys)
        {
            bool isPressed = false;
            foreach (var key in _keyMappings[action])
            {
                if (Input.GetKey(key))
                {
                    isPressed = true;
                    break;
                }
            }

            _keyStates[action].Value = isPressed;
        }
    }

    /// <summary>
    /// ✅ **키 상태 가져오기 (ReactiveProperty)**
    /// </summary>
    public IReadOnlyReactiveProperty<bool> GetKeyNotifier(KeyboardActionType actionType)
    {
        return _keyStates[actionType];
    }

    /// <summary>
    /// ✅ **키 매핑 변경 (설정 UI에서 활용 가능)**
    /// </summary>
    public void SetKeys(KeyboardActionType actionType, List<KeyCode> newKeys)
    {
        if (_keyMappings.ContainsKey(actionType))
        {
            _keyMappings[actionType] = newKeys;
        }
    }
}
