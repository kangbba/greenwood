using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using System.Text.RegularExpressions;
using System;
using System.Linq;

public class RevealingSentence : MonoBehaviour
{
    [Header("Word Prefab Reference")]
    [SerializeField] private RevealingWord _revealingWordPrefab;

    [Header("Container")]
    [SerializeField] private RectTransform _container;

    [Header("Layout Settings")]
    [SerializeField] private float _wordSpacing = 10f;
    [SerializeField] private float _extraLineSpacing = 0f; 

    private float _playSpeed = 5000;

    /// <summary>
    /// 구두점(문장부호) 뒤에 대기할 것인지 여부
    /// </summary>
    private bool _usePunctuationStop = false;

    /// <summary>
    /// 각 단어(RevealingWord)가 구두점인지 여부를 매핑
    /// </summary>
    private Dictionary<RevealingWord, bool> _punctuationDict = new Dictionary<RevealingWord, bool>();

    private List<RevealingWord> _activeWords = new List<RevealingWord>();

    private RevealingWord _lastWord;

    public RevealingWord LastWord { get => _lastWord; }


    /// <summary>
    /// 문장부호 뒤 대기를 켜거나 끄는 함수
    /// </summary>
    public void SetPunctuationStop(bool value)
    {
        _usePunctuationStop = value;
    }

    /// <summary>
    /// 재생 속도 설정
    /// </summary>
    public void SetPlaySpeed(float speed)
    {
        _playSpeed = speed;
    }

    /// <summary>
    /// 긴 문장을 (단어+문장부호)로 분리하여 미리 생성
    /// highlightWords에 포함된 단어는 노란색
    /// </summary>
    public void SetText(string dialogue, List<string> highlightWords = null)
    {
        ClearSentence();

        List<string> elements = SplitDialogue(dialogue);
        float currentXOffset = 0f;
        float currentYOffset = 0f;
        float containerWidth = _container.rect.width;

        // 기본 줄 높이 = (프리팹 높이 + 추가 라인 스페이싱)
        float baseLineHeight = _revealingWordPrefab.GetComponent<RectTransform>().sizeDelta.y + _extraLineSpacing;

        for (int i = 0; i < elements.Count; i++)
        {
            string element = elements[i];
            if (string.IsNullOrEmpty(element)) 
                continue;

            // 프리팹 인스턴스화
            RevealingWord wordInstance = Instantiate(_revealingWordPrefab, _container);

            // highlightWords에 포함되면 노란색 표시
            bool shouldHighlight = highlightWords != null && highlightWords.Contains(element);
            if (shouldHighlight)
            {
                wordInstance.Init($"<color=#FFFF00>{element}</color>");
            }
            else
            {
                wordInstance.Init(element);
            }

            bool isPunctuation = IsPunctuation(element);
            _punctuationDict[wordInstance] = isPunctuation;

            RectTransform wordTransform = wordInstance.RectTransform;

            // 컨테이너 폭 초과 시 줄바꿈
            if (currentXOffset + wordTransform.sizeDelta.x > containerWidth)
            {
                currentXOffset = 0f;
                currentYOffset -= baseLineHeight;
            }

            // 위치 지정
            wordTransform.anchoredPosition = new Vector2(currentXOffset, currentYOffset);
            _activeWords.Add(wordInstance);

            // 단어 + 문장부호가 붙어 있을 경우, 간격 0
            float spacingToAdd = _wordSpacing;
            if (!isPunctuation && i + 1 < elements.Count && IsPunctuation(elements[i + 1]))
            {
                spacingToAdd = 0f;
            }

            currentXOffset += wordTransform.sizeDelta.x + spacingToAdd;
        }
    }

    public async UniTask PlaySentence(Func<UniTask> OnStart = null, Func<UniTask> OnPunctuationPause = null, Func<UniTask> OnPunctuationResume = null, Func<UniTask> OnComplete = null)
    {
        // 시작 시 OnStart 실행 (필수 아님)
        if (OnStart != null) await OnStart();

        // 인덱스를 사용하여 마지막 단어인지 확인
        for (int i = 0; i < _activeWords.Count; i++)
        {
            var word = _activeWords[i];

            await word.Play(_playSpeed);
            
            _lastWord = word;
                
            // 마지막 단어가 아니라면 구두점 멈춤 로직 적용
            if (i < _activeWords.Count - 1)
            {
                if (_usePunctuationStop && _punctuationDict.TryGetValue(word, out bool isPunc) && isPunc)
                {
                    // 외부에서 전달된 대기 로직 실행 (필수 아님)
                    if (OnPunctuationPause != null) await OnPunctuationPause();
                    if (OnPunctuationResume != null) await OnPunctuationResume();
                }
            }
        }

        // 모든 단어가 완료된 후 OnComplete 실행 (필수 아님)
        if (OnComplete != null) await OnComplete();
    }
        




    /// <summary>
    /// 모든 단어를 즉시 완성 (전체 문장을 바로 표시)
    /// </summary>
    public void CompleteAllInstantly()
    {
        foreach (var word in _activeWords)
        {
            word.CompleteInstantly();
        }
    }

    /// <summary>
    /// 현재 문장(단어들) 클리어
    /// </summary>
    public void ClearSentence()
    {
        foreach (var w in _activeWords)
        {
            Destroy(w.gameObject);
        }
        _activeWords.Clear();
        _punctuationDict.Clear();
    }
    /// <summary>
    /// 문자열을 단어와 문장부호로 분리 (쉼표는 단어에 포함)
    /// </summary>
    private List<string> SplitDialogue(string dialogue)
    {
        List<string> elements = new List<string>();

        // "단어+문장부호" 매치 (쉼표 포함, ...은 별도로 인식)
        MatchCollection matches = Regex.Matches(dialogue, @"(\.{3}|[\w가-힣,]+[.!?]*)");
        foreach (Match match in matches)
        {
            string value = match.Value;
            if (string.IsNullOrEmpty(value)) 
                continue;

            // 끝에 문장부호가 붙어있으면 분리 (쉼표는 제외)
            Match punctuationMatch = Regex.Match(value, @"(\.{3}|[.!?]+)$");
            if (punctuationMatch.Success)
            {
                int punctIndex = punctuationMatch.Index;
                string wordPart = value.Substring(0, punctIndex);
                string punctuationPart = value.Substring(punctIndex);

                if (!string.IsNullOrEmpty(wordPart))
                    elements.Add(wordPart); // 쉼표 포함된 단어 유지

                elements.Add(punctuationPart); // "..."도 문장부호로 추가
            }
            else
            {
                elements.Add(value); // 쉼표가 있어도 단어로 취급
            }
        }

        return elements;
    }

    /// <summary>
    /// 해당 단어가 문장부호인지 확인 ("..." 포함)
    /// </summary>
    private bool IsPunctuation(string word)
    {
        return Regex.IsMatch(word, @"^(\.{3}|[.!?]+)$");
    }

}
