using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSetting", menuName = "Greenwood/CharacterSetting")]
public class CharacterSetting : ScriptableObject
{
    [SerializeField] private CharacterName _characterName;
    [SerializeField] private string _displayName;
    [SerializeField] private Color _characterColor;

    public CharacterName CharacterName => _characterName;
    public string DisplayName => _displayName;
    public Color CharacterColor => _characterColor;
}
