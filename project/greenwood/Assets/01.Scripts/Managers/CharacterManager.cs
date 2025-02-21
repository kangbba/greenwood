using UnityEngine;
using System.Collections.Generic;
using System.Linq; // ✅ LINQ 사용 추가

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance { get; private set; }

    [SerializeField] private List<CharacterSetting> _characterSettings; // 캐릭터 설정 리스트
    [SerializeField] private List<Character> _characterPrefabs;        // 캐릭터 프리팹 리스트

    private Dictionary<ECharacterName, CharacterSetting> _characterSettingsDict;
    private Dictionary<ECharacterName, Character> _characterPrefabsDict;

    private List<Character> _activeCharacters = new(); // ✅ 활성화된 캐릭터 리스트로 변경

    public List<Character> ActiveCharacters => _activeCharacters; // ✅ 활성 캐릭터 리스트 반환

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
        _characterSettingsDict = _characterSettings.ToDictionary(setting => setting.CharacterName);
    }

    /// <summary>
    /// 캐릭터 프리팹 초기화
    /// </summary>
    private void InitializeCharacterPrefabs()
    {
        _characterPrefabsDict = _characterPrefabs.ToDictionary(prefab => prefab.CharacterName);
    }

    /// <summary>
    /// 특정 캐릭터의 설정을 가져옴
    /// </summary>
    public CharacterSetting GetCharacterSetting(ECharacterName characterName)
    {
        return _characterSettingsDict.TryGetValue(characterName, out var setting) ? setting : null;
    }

    /// <summary>
    /// 특정 캐릭터 프리팹을 가져옴
    /// </summary>
    public Character GetCharacterPrefab(ECharacterName characterName)
    {
        return _characterPrefabsDict.TryGetValue(characterName, out var prefab) ? prefab : null;
    }

    /// <summary>
    /// 특정 캐릭터가 활성화되어 있는지 확인하고 가져옴
    /// </summary>
    public Character GetActiveCharacter(ECharacterName characterName)
    {
        return _activeCharacters.FirstOrDefault(character => character.CharacterName == characterName);
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
        _activeCharacters.Add(newCharacter);
        Debug.Log($"CharacterManager: `{characterName}` 캐릭터 활성화 완료.");
        return newCharacter;
    }

    /// <summary>
    /// 특정 캐릭터를 페이드 아웃 후 제거
    /// </summary>
    public void FadeOutAndDestroyCharacter(ECharacterName characterName, float duration)
    {
        Character character = GetActiveCharacter(characterName);
        if (character == null)
        {
            Debug.LogWarning($"CharacterManager: `{characterName}` 캐릭터가 활성화되지 않아 제거할 수 없습니다.");
            return;
        }

        character.gameObject.SetAnimDestroy(duration); // 페이드 아웃 애니메이션 적용
        _activeCharacters.Remove(character);
    }
}
