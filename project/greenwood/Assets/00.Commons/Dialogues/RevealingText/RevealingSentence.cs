using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

public class RevealingSentence : MonoBehaviour
{
    [Header("Word Prefab Reference")]
    [SerializeField] private RevealingWord _revealingWordPrefab;

    [Header("Container")]
    [SerializeField] private RectTransform _container;

    [Header("Layout Settings")]
    [SerializeField] private float _punctuationOffset = 10f; 
    [SerializeField] private float _extraLineSpacing = 0f;

    private float _playSpeed = 5000f;
    private bool _usePunctuationStop = false;

    /// <summary>
    /// 각 조각(RevealingWord)이 분리된 이유를 매핑
    /// </summary>
    private Dictionary<RevealingWord, SplitReason> _splitReasonDict = new Dictionary<RevealingWord, SplitReason>();

    private List<RevealingWord> _activeWords = new List<RevealingWord>();
    private RevealingWord _lastWord;
    public RevealingWord LastWord => _lastWord;

    /// <summary>
    /// 분리 이유 열거형
    /// </summary>
    public enum SplitReason
    {
        Normal,
        EndsWithPunctuation,
        EndsAtContainerMax
    }

    /// <summary>
    /// 미리 정의한 분리 기준 문장부호 목록 (쉼표 제외)
    /// </summary>
    private readonly List<string> _punctuationToPause = new List<string> { "!", ".", "?", "..." };

    // 캐시: 길이 내림차순으로 정렬된 문장부호 리스트
    private List<string> _sortedPunctuationToPause;
    
    private void Awake()
    {
        // _punctuationToPause 리스트를 길이 내림차순으로 정렬한 새로운 리스트 생성
        _sortedPunctuationToPause = _punctuationToPause.OrderByDescending(s => s.Length).ToList();
    }

    // -------------------------
    // Public API
    // -------------------------
    public void SetPunctuationStop(bool value)
    {
        _usePunctuationStop = value;
    }

    public void SetPlaySpeed(float speed)
    {
        _playSpeed = speed;
    }

    /// <summary>
    /// 긴 문장을 (문장 조각)으로 분리하여 미리 생성합니다.
    /// { 와 }는 제거된 채로 분리한 후, 후처리를 통해 복원합니다.
    /// </summary>
    public void SetText(string sentence)
    {
        ClearSentence();

        // 1. 원본 대화에서 중괄호 정보를 추출하고 모두 제거
        Dictionary<int, string> bracePositions = ExtractBraces(sentence);
        string cleanDialogue = sentence.Replace("{", "").Replace("}", "");

        // 2. 깨끗한 텍스트를 기반으로 토큰 분리
        List<string> elements = SplitDialogue(cleanDialogue);

        float currentXOffset = 0f;
        float currentYOffset = 0f;
        float containerWidth = _container.rect.width;
        float baseLineHeight = _revealingWordPrefab.GetComponent<RectTransform>().sizeDelta.y + _extraLineSpacing;

        for (int i = 0; i < elements.Count; i++)
        {
            string token = elements[i];
            if (string.IsNullOrEmpty(token))
                continue;

            // 줄바꿈 토큰: 새 줄로 처리
            if (token == "\n")
            {
                currentXOffset = 0f;
                currentYOffset -= baseLineHeight;
                continue;
            }

            if (_punctuationToPause.Contains(token))
            {
                if (_activeWords.Count > 0)
                {
                    RevealingWord prevChunk = _activeWords.Last();
                    prevChunk.AppendText(token);
                    // 강제로 레이아웃 갱신하여 RectTransform 크기를 최신화합니다.
                    LayoutRebuilder.ForceRebuildLayoutImmediate(prevChunk.RectTransform);
                    _splitReasonDict[prevChunk] = SplitReason.EndsWithPunctuation;

                    // 업데이트된 크기를 기반으로 currentXOffset 갱신 + _punctuationOffset 추가
                    RectTransform prevTransform = prevChunk.RectTransform;
                    currentXOffset = prevTransform.anchoredPosition.x + prevTransform.sizeDelta.x + _punctuationOffset;
                }
                else
                {
                    // 이전 조각이 없으면 새로 생성
                    RevealingWord wordInstance = Instantiate(_revealingWordPrefab, _container);
                    wordInstance.Init(token);
                    _splitReasonDict[wordInstance] = SplitReason.Normal;
                    RectTransform wordTransform = wordInstance.RectTransform;
                    if (currentXOffset + wordTransform.sizeDelta.x > containerWidth)
                    {
                        currentXOffset = 0f;
                        currentYOffset -= baseLineHeight;
                    }
                    wordTransform.anchoredPosition = new Vector2(currentXOffset, currentYOffset);
                    _activeWords.Add(wordInstance);
                    currentXOffset += wordTransform.sizeDelta.x;
                }
                continue;
            }




            // 일반 텍스트 토큰 처리 (필요 시 분할)
            while (!string.IsNullOrEmpty(token))
            {
                // 미리 단어의 폭을 확인하기 위해 임시 인스턴스 생성
                float tokenWidth = GetTextWidth(token);
                if (currentXOffset + tokenWidth <= containerWidth)
                {
                    RevealingWord wordInstance = CreateWordChunk(token, ref currentXOffset, ref currentYOffset, containerWidth, baseLineHeight);
                    _splitReasonDict[wordInstance] = SplitReason.Normal;
                    token = "";
                }
                else
                {
                    // 토큰이 container를 넘어가는 경우, 일부만 표시하고 나머지는 다음 줄에
                    string fitPart, remainder;
                    SplitTokenToFit(token, containerWidth - currentXOffset, out fitPart, out remainder);
                    if (!string.IsNullOrEmpty(fitPart))
                    {
                        RevealingWord wordInstance = CreateWordChunk(fitPart, ref currentXOffset, ref currentYOffset, containerWidth, baseLineHeight);
                        _splitReasonDict[wordInstance] = SplitReason.EndsAtContainerMax;
                    }
                    else
                    {
                        // fitPart가 없으면 강제로 줄바꿈
                        currentXOffset = 0f;
                        currentYOffset -= baseLineHeight;
                    }
                    token = remainder;
                }
            }
        }

        // 3. 복원: 원본 중괄호 위치에 따라 {} 부활
        RestoreBraces(bracePositions);
        // 4. 후처리: EndsAtContainerMax 조각에 대해 색상 태그 경계 보정
        PostProcessColorTags();
        // 5. 최종: 모든 조각에서 {와 }를 색상 태그로 대체
        ReplaceColorTagsInWords();
    }

    /// <summary>
    /// 중복되는 단어 인스턴스 생성과 줄바꿈 오프셋 조정 로직을 분리
    /// </summary>
    private RevealingWord CreateWordChunk(string text, ref float currentXOffset, ref float currentYOffset, float containerWidth, float baseLineHeight)
    {
        RevealingWord wordInstance = Instantiate(_revealingWordPrefab, _container);
        wordInstance.Init(text);
        RectTransform wordTransform = wordInstance.RectTransform;
        // 만약 현재 오프셋에 단어가 들어가지 않는다면 줄바꿈 처리
        if (currentXOffset + wordTransform.sizeDelta.x > containerWidth)
        {
            currentXOffset = 0f;
            currentYOffset -= baseLineHeight;
        }
        wordTransform.anchoredPosition = new Vector2(currentXOffset, currentYOffset);
        _activeWords.Add(wordInstance);
        currentXOffset += wordTransform.sizeDelta.x;
        return wordInstance;
    }

    /// <summary>
    /// 텍스트의 폭을 확인하기 위해 임시 인스턴스를 생성 후 계산
    /// </summary>
    private float GetTextWidth(string text)
    {
        RevealingWord tempWord = Instantiate(_revealingWordPrefab);
        tempWord.Init(text);
        float width = tempWord.RectTransform.sizeDelta.x;
        Destroy(tempWord.gameObject);
        return width;
    }

    /// <summary>
    /// 원본 문자열에서 {, }의 위치를 추출합니다.
    /// </summary>
    private Dictionary<int, string> ExtractBraces(string sentence)
    {
        Dictionary<int, string> bracePositions = new Dictionary<int, string>();
        int offset = 0;
        for (int i = 0; i < sentence.Length; i++)
        {
            if (sentence[i] == '{' || sentence[i] == '}')
            {
                bracePositions[i - offset] = sentence[i].ToString();
                offset++;
            }
        }
        return bracePositions;
    }

    /// <summary>
    /// 저장된 중괄호 정보를 사용하여, 분리된 조각들에 {}를 복원합니다.
    /// </summary>
    private void RestoreBraces(Dictionary<int, string> bracePositions)
    {
        int globalIndex = 0;
        foreach (var word in _activeWords)
        {
            string currentText = word.TextMesh.text;
            string restoredText = "";
            for (int i = 0; i < currentText.Length; i++)
            {
                if (bracePositions.ContainsKey(globalIndex))
                    restoredText += bracePositions[globalIndex];
                restoredText += currentText[i];
                globalIndex++;
            }
            word.TextMesh.text = restoredText;
        }
    }

    /// <summary>
    /// 주어진 너비에 맞게 토큰을 분할합니다.
    /// </summary>
    private void SplitTokenToFit(string token, float availableWidth, out string fitPart, out string remainder)
    {
        fitPart = "";
        remainder = "";
        RevealingWord tempWord = Instantiate(_revealingWordPrefab);
        for (int i = 1; i <= token.Length; i++)
        {
            string substring = token.Substring(0, i);
            tempWord.Init(substring);
            float width = tempWord.RectTransform.sizeDelta.x;
            if (width > availableWidth)
            {
                fitPart = token.Substring(0, i - 1);
                remainder = token.Substring(i - 1);
                Destroy(tempWord.gameObject);
                return;
            }
        }
        fitPart = token;
        remainder = "";
        Destroy(tempWord.gameObject);
    }

    /// <summary>
    /// 모든 조각을 즉시 완성합니다.
    /// </summary>
    public void CompleteAllInstantly()
    {
        foreach (var word in _activeWords)
            word.CompleteInstantly();
    }

    /// <summary>
    /// 현재 문장(조각들)을 클리어합니다.
    /// </summary>
    public void ClearSentence()
    {
        foreach (var w in _activeWords)
            Destroy(w.gameObject);
        _activeWords.Clear();
        _splitReasonDict.Clear();
    }

    /// <summary>
    /// 문자열을 분리하는 함수: 미리 정의한 문장부호 및 줄바꿈에서만 분리 (띄어쓰기는 유지)
    /// </summary>
    private List<string> SplitDialogue(string sentence)
    {
        List<string> elements = new List<string>();
        sentence = sentence.Replace("\r\n", "\n").Replace("\r", "\n");
        int i = 0;
        string buffer = "";
        while (i < sentence.Length)
        {
            if (sentence[i] == '\n')
            {
                if (!string.IsNullOrEmpty(buffer))
                {
                    elements.Add(buffer);
                    buffer = "";
                }
                elements.Add("\n");
                i++;
                continue;
            }
            bool matched = false;
            foreach (string token in _sortedPunctuationToPause)
            {
                if (i + token.Length <= sentence.Length && sentence.Substring(i, token.Length) == token)
                {
                    if (!string.IsNullOrEmpty(buffer))
                    {
                        elements.Add(buffer);
                        buffer = "";
                    }
                    elements.Add(token);
                    i += token.Length;
                    matched = true;
                    break;
                }
            }
            if (matched)
                continue;
            buffer += sentence[i];
            i++;
        }
        if (!string.IsNullOrEmpty(buffer))
            elements.Add(buffer);
        return elements;
    }

    /// <summary>
    /// 미리 정의한 문장부호인지 확인 ("..." 포함)
    /// </summary>
    private bool IsPunctuation(string word)
    {
        return _punctuationToPause.Contains(word);
    }

    /// <summary>
    /// 후처리: EndsAtContainerMax 분리 조각에 대해,
    /// 만약 해당 조각의 텍스트에서 '{'의 개수가 '}'의 개수보다 많으면,
    /// 바로 다음 조각에서 가장 가까운 '}'를 찾아 현재 조각의 끝에 '</color>'를 추가하고,
    /// 다음 조각의 시작에 '<color=#FFFF00>'를 추가하여 보완합니다.
    /// </summary>
    private void PostProcessColorTags()
    {
        for (int i = 0; i < _activeWords.Count; i++)
        {
            RevealingWord currentChunk = _activeWords[i];
            if (_splitReasonDict.TryGetValue(currentChunk, out SplitReason reason) &&
                reason == SplitReason.EndsAtContainerMax)
            {
                string currentText = currentChunk.TextMesh.text;
                int openCount = currentText.Count(c => c == '{');
                int closeCount = currentText.Count(c => c == '}');
                if (openCount > closeCount)
                {
                    // 현재 조각에 닫는 괄호가 없다면 보완
                    if (!currentText.EndsWith("}"))
                    {
                        currentText += "}";
                        currentChunk.TextMesh.text = currentText;
                    }
                    // 다음 조각에서 가장 가까운 '}'를 찾아, 만약 그 조각의 시작이 '{'가 아니라면,
                    // 그 조각 앞에 '{'를 추가하여 보완합니다.
                    if (i + 1 < _activeWords.Count)
                    {
                        RevealingWord nextChunk = _activeWords[i + 1];
                        string nextText = nextChunk.TextMesh.text;
                        if (!nextText.StartsWith("{"))
                        {
                            nextText = "{" + nextText;
                            nextChunk.TextMesh.text = nextText;
                        }
                        Debug.LogWarning($"후처리: 조각 {i}에서 열린 괄호 보완 - 현재: \"{currentText}\", 다음: \"{nextText}\"");
                    }
                }
            }
        }
    }

    /// <summary>
    /// 모든 활성 조각의 텍스트에서 '{'를 '<color=#FFFF00>'로, '}'를 '</color>'로 대체합니다.
    /// </summary>
    private void ReplaceColorTagsInWords()
    {
        foreach (var word in _activeWords)
        {
            string originalText = word.TextMesh.text;
            string newText = originalText.Replace("{", "<color=#FFFF00>").Replace("}", "</color>");
            word.TextMesh.text = newText;
            if (_splitReasonDict.TryGetValue(word, out SplitReason reason))
            {
                word.gameObject.name = $"[{reason}]";
            }
            else
            {
                word.gameObject.name = $"[Unknown]";
            }
        }
    }

    /// <summary>
    /// 문장을 재생하는 비동기 메서드 (외부 콜백은 그대로 유지)
    /// </summary>
    public async UniTask PlaySentence(Func<UniTask> OnStart = null, Func<UniTask> OnPunctuationPause = null, Func<UniTask> OnPunctuationResume = null, Func<UniTask> OnComplete = null)
    {
        if (OnStart != null) await OnStart();

        for (int i = 0; i < _activeWords.Count; i++)
        {
            var word = _activeWords[i];
            await word.Play(_playSpeed);
            _lastWord = word;

            if (i < _activeWords.Count - 1)
            {
                if (_usePunctuationStop && _splitReasonDict.TryGetValue(word, out SplitReason reason) && (reason == SplitReason.EndsWithPunctuation))
                {
                    if (OnPunctuationPause != null) await OnPunctuationPause();
                    if (OnPunctuationResume != null) await OnPunctuationResume();
                }
            }
        }

        if (OnComplete != null) await OnComplete();
    }
}
