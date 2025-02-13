using UnityEngine;
using System.Collections.Generic;
public enum CharacterName
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

    [SerializeField] private List<CharacterSetting> _characterSettings;

    private Dictionary<CharacterName, CharacterSetting> _characterSettingsDict;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeCharacterSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeCharacterSettings()
    {
        _characterSettingsDict = new Dictionary<CharacterName, CharacterSetting>();

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
    /// 캐릭터 이름을 기반으로 설정을 가져옴
    /// </summary>
    public CharacterSetting GetCharacterSetting(CharacterName characterName)
    {
        if (_characterSettingsDict.TryGetValue(characterName, out CharacterSetting setting))
        {
            return setting;
        }

        Debug.LogWarning($"CharacterManager: 해당 캐릭터({characterName}) 설정이 없습니다.");
        return null;
    }
}
