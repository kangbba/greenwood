using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class CharacterCardUI : AnimationImage
{
    [SerializeField] private RectTransform _characterContainer; // ✅ 캐릭터를 배치할 컨테이너
    [SerializeField] private RectMask2D _rectMask; // ✅ 얼굴 부분만 보이도록 마스크 적용

    [SerializeField] private TextMeshProUGUI _characterText; // ✅ 얼굴 부분만 보이도록 마스크 적용
    private Character _characterInstance; // ✅ 카드 내 캐릭터 인스턴스

    /// <summary>
    /// ✅ **카드에 캐릭터 설정 (프리팹을 인스턴스화)**
    /// </summary>
    public void SetCharacter(Character characterPrefab, string characterName)
    {
        if (characterPrefab == null)
        {
            Debug.LogWarning("[CharacterCardUI] Character Prefab is null!");
            return;
        }

        // ✅ 기존 캐릭터가 있다면 제거
        if (_characterInstance != null)
        {
            Destroy(_characterInstance.gameObject);
        }
        _characterText.text = characterName;

        // ✅ 새로운 캐릭터 인스턴스 생성
        _characterInstance = Instantiate(characterPrefab, _characterContainer);
        _characterInstance.transform.localPosition = Vector3.zero;
        _characterInstance.transform.localScale = Vector3.one * .2f;


        // ✅ 얼굴 부분만 보이도록 마스크 적용
    }

}
