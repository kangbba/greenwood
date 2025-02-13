using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using System.Text.RegularExpressions;

public class RevealingDialogue : MonoBehaviour
{
    [SerializeField] private RevealingText _revealingTextPrefab;
    [SerializeField] private RectTransform _container;
    [SerializeField] private float _wordSpacing = 10f;
    [SerializeField] private float _extraLineSpacing = 0f; // 추가적인 세로 간격
    [SerializeField] private float _playSpeed = 150f;

    private List<RevealingText> _activeTexts = new List<RevealingText>();

    private void Start()
    {
        // 예시 대사
        Init("안녕하세요. 길게 말을 해보겠습니다. 잘 되고 있나요? 하하하하하! 정말로? 네, 좋아요. 더욱더 길게 말을 해보자.");
        PlayDialogue().Forget();
    }

    /// <summary>
    /// 긴 대사를 원본 간격을 유지하며 미리 생성 (문장부호 포함)
    /// </summary>
    public void Init(string dialogue)
    {
        ClearDialogue();
        List<string> elements = SplitDialogue(dialogue);
        float currentXOffset = 0f;
        float currentYOffset = 0f;
        float containerWidth = _container.rect.width;

        // 기본 줄 높이 = 텍스트의 높이 + 추가 간격
        float baseLineHeight = _revealingTextPrefab.GetComponent<RectTransform>().sizeDelta.y + _extraLineSpacing;

        for (int i = 0; i < elements.Count; i++)
        {
            string element = elements[i];
            if (string.IsNullOrEmpty(element))
                continue;

            // 프리팹 인스턴스화 및 초기화
            RevealingText textInstance = Instantiate(_revealingTextPrefab, _container);
            textInstance.Init(element);
            RectTransform textTransform = textInstance.GetComponent<RectTransform>();

            // 컨테이너 폭을 초과하면 줄바꿈
            if (currentXOffset + textTransform.sizeDelta.x > containerWidth)
            {
                currentXOffset = 0f;
                currentYOffset -= baseLineHeight;
            }

            // 텍스트 위치 지정 (단어와 문장부호 모두 같은 방식으로 배치)
            textTransform.anchoredPosition = new Vector2(currentXOffset, currentYOffset);
            _activeTexts.Add(textInstance);

            // 현재 요소가 단어이고, 다음 요소가 문장부호라면 단어 사이의 간격을 0으로 설정하여 문장부호가 바로 붙도록 함
            float spacingToAdd = _wordSpacing;
            if (!IsPunctuation(element) && i + 1 < elements.Count && IsPunctuation(elements[i + 1]))
            {
                spacingToAdd = 0f;
            }

            currentXOffset += textTransform.sizeDelta.x + spacingToAdd;
        }
    }

    /// <summary>
    /// 미리 생성된 텍스트를 순차적으로 재생
    /// </summary>
    public async UniTask PlayDialogue()
    {
        foreach (var textInstance in _activeTexts)
        {
            await textInstance.Play(_playSpeed);
        }
    }

    /// <summary>
    /// 원본 문자열을 단어와 문장부호로 분리.
    /// 예) "안녕하세요." → "안녕하세요", "."
    /// </summary>
    private List<string> SplitDialogue(string dialogue)
    {
        List<string> elements = new List<string>();

        // 단어와 뒤따르는 문장부호(있을 경우)를 매치합니다.
        MatchCollection matches = Regex.Matches(dialogue, @"[\w가-힣]+[.,!?]*");
        foreach (Match match in matches)
        {
            string value = match.Value;
            if (string.IsNullOrEmpty(value))
                continue;

            // 단어 끝에 문장부호가 붙어있다면 단어와 문장부호를 분리합니다.
            Match punctuationMatch = Regex.Match(value, @"[.,!?]+$");
            if (punctuationMatch.Success)
            {
                int punctIndex = punctuationMatch.Index;
                string wordPart = value.Substring(0, punctIndex);
                string punctuationPart = value.Substring(punctIndex);

                if (!string.IsNullOrEmpty(wordPart))
                    elements.Add(wordPart);

                // 문장부호 전체(예: "?!")를 한 요소로 추가
                elements.Add(punctuationPart);
            }
            else
            {
                elements.Add(value);
            }
        }

        return elements;
    }

    /// <summary>
    /// 현재 요소가 문장부호만으로 이루어졌는지 체크
    /// </summary>
    private bool IsPunctuation(string word)
    {
        return Regex.IsMatch(word, @"^[.,!?]+$");
    }

    /// <summary>
    /// 현재 표시된 대사 제거
    /// </summary>
    public void ClearDialogue()
    {
        foreach (var text in _activeTexts)
        {
            Destroy(text.gameObject);
        }
        _activeTexts.Clear();
    }
}
