using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 감정 상태 Enum
/// </summary>
public enum KateEmotionType
{
    Happy,
    Exited,
    Angry,
    YeahRight,
    Concerned,
    Shy
}


/// <summary>
/// 포즈 상태 Enum
/// </summary>
public enum KatePoseType
{
    HandsFront,
    HandsBack,
    ArmCrossed,
}

public enum ECharacterName
{
    Mono,
    Ryan,
    Kate,
    Joseph,
    Lisa,
    Eldra,
    Amalian
}

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance { get; private set; }

    [SerializeField] private List<CharacterSetting> _characterSettings; // 캐릭터 설정 리스트
    [SerializeField] private List<Character> _characterPrefabs;        // 캐릭터 프리팹 리스트

    private Dictionary<ECharacterName, CharacterSetting> _characterSettingsDict;
    private Dictionary<ECharacterName, Character> _characterPrefabsDict;
    
    // "Spawned" → "Active"로 변수명 변경
    private Dictionary<ECharacterName, Character> _activeCharacters = new(); // 활성화된 캐릭터 관리

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeCharacterSettings();
            InitializeCharacterPrefabs();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 캐릭터 설정 초기화
    /// </summary>
    private void InitializeCharacterSettings()
    {
        _characterSettingsDict = new Dictionary<ECharacterName, CharacterSetting>();

        foreach (var setting in _characterSettings)
        {
            if (!_characterSettingsDict.ContainsKey(setting.CharacterName))
            {
                _characterSettingsDict.Add(setting.CharacterName, setting);
            }
            else
            {
                Debug.LogWarning($"CharacterManager: 중복된 캐릭터 설정 발견 - {setting.CharacterName}");
            }
        }
    }

    /// <summary>
    /// 캐릭터 프리팹 초기화
    /// </summary>
    private void InitializeCharacterPrefabs()
    {
        _characterPrefabsDict = new Dictionary<ECharacterName, Character>();

        foreach (Character prefab in _characterPrefabs)
        {
            if (!_characterPrefabsDict.ContainsKey(prefab.CharacterName))
            {
                _characterPrefabsDict.Add(prefab.CharacterName, prefab);
            }
            else
            {
                Debug.LogWarning($"CharacterManager: 중복된 캐릭터 프리팹 발견 - {prefab.CharacterName}");
            }
        }
    }

    /// <summary>
    /// 특정 캐릭터의 설정을 가져옴
    /// </summary>
    public CharacterSetting GetCharacterSetting(ECharacterName characterName)
    {
        if (_characterSettingsDict.TryGetValue(characterName, out CharacterSetting setting))
        {
            return setting;
        }

        Debug.LogWarning($"CharacterManager: 해당 캐릭터({characterName}) 설정이 없습니다.");
        return null;
    }

    /// <summary>
    /// 특정 캐릭터 프리팹을 가져옴
    /// </summary>
    public Character GetCharacterPrefab(ECharacterName characterName)
    {
        if (_characterPrefabsDict.TryGetValue(characterName, out Character character))
        {
            return character;
        }

        Debug.LogWarning($"CharacterManager: 해당 캐릭터({characterName}) 프리팹이 없습니다.");
        return null;
    }

    /// <summary>
    /// 특정 캐릭터가 활성화되어 있는지 확인하고 가져옴
    /// </summary>
    public Character GetActiveCharacter(ECharacterName characterName)
    {
        if (_activeCharacters.TryGetValue(characterName, out Character activeCharacter))
        {
            return activeCharacter;
        }

        Debug.LogWarning($"CharacterManager: `{characterName}` 캐릭터가 아직 활성화되지 않았습니다.");
        return null;
    }

    /// <summary>
    /// 특정 캐릭터를 생성(활성화)하여 UIManager.Instance.GameCanvas.CharacterLayer에 배치
    /// </summary>
    public Character CreateCharacter(ECharacterName characterName)
    {
        // 이미 활성화된 캐릭터라면 반환
        Character existingCharacter = GetActiveCharacter(characterName);
        if (existingCharacter != null)
        {
            Debug.Log($"CharacterManager: 캐릭터 `{characterName}` 이미 활성화됨.");
            return existingCharacter;
        }

        // 프리팹 가져오기
        Character prefab = GetCharacterPrefab(characterName);
        if (prefab == null)
        {
            Debug.LogError($"CharacterManager: `{characterName}` 캐릭터 프리팹을 찾을 수 없습니다.");
            return null;
        }

        // 부모 UI 레이어 설정
        Transform parent = UIManager.Instance.GameCanvas.CharacterLayer;
        if (parent == null)
        {
            Debug.LogError("CharacterManager: UIManager.Instance.GameCanvas.CharacterLayer가 존재하지 않습니다.");
            return null;
        }

        // 캐릭터 인스턴스 생성 및 부모 설정
        Character newCharacter = Instantiate(prefab, parent);
        _activeCharacters[characterName] = newCharacter;
        Debug.Log($"CharacterManager: `{characterName}` 캐릭터 활성화 완료.");
        return newCharacter;
    }

    /// <summary>
    /// 특정 캐릭터를 페이드 아웃 후 제거
    /// </summary>
    public void FadeOutAndDestroyCharacter(ECharacterName characterName, float duration)
    {
        if (!_activeCharacters.TryGetValue(characterName, out Character character))
        {
            Debug.LogWarning($"CharacterManager: `{characterName}` 캐릭터가 활성화되지 않아 제거할 수 없습니다.");
            return;
        }
        character.gameObject.SetAnimDestroy(duration); // 페이드 아웃 애니메이션 적용
        _activeCharacters.Remove(characterName);
    }
}
