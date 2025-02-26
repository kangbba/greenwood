using UnityEngine;
using static CharacterEnums;

[CreateAssetMenu(fileName = "CharacterSetting", menuName = "Greenwood/CharacterSetting")]
public class CharacterSetting : ScriptableObject
{
    [SerializeField] private ECharacterName _characterName;
    [SerializeField] private string _displayName;
    [SerializeField] private Color _characterColor;
    [SerializeField] private float _height = 190;

    public ECharacterName CharacterName => _characterName;
    public string DisplayName => _displayName;
    public Color CharacterColor => _characterColor;

    public float Height { get => _height; }
}
