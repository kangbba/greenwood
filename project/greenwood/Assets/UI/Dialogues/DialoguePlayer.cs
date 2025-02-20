using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine.UI;

public class DialoguePlayer : MonoBehaviour
{
    [Header("Arrow Prefab")]
    [SerializeField] private DialogueArrow _arrowPrefab;
    [SerializeField] private TextMeshProUGUI _ownerText;
    [SerializeField] private Image _ownerBackground;
    private DialogueArrow _activeArrow; // 현재 활성화된 화살표 인스턴스 저장

    [Header("Revealing Sentence")]
    [SerializeField] private RevealingSentence _revealingSentence;
    [SerializeField] private Transform _parent;
    private bool _isOn;

    private List<string> _sentences;
    private List<string> _importantWords;
    private float _currentSpeed;
    private float _initialSpeed;
    private bool _isSkipping;

    /// <summary>
    /// 다이얼로그 초기화
    /// </summary>
    public void Init(string ownerName, Color ownerTextColor, Color ownerBackgroundColor, List<string> sentences, float speed)
    {   
        _sentences = sentences;
        _initialSpeed = speed;

        _ownerText.SetText(ownerName);
        _ownerText.color = ownerTextColor;
        _ownerBackground.color = ownerBackgroundColor;
        
        ChangeSpeed(_initialSpeed);
        SetAnim(false, 0f);  // 초기 상태 숨김
    }

    private void ChangeSpeed(float speed){
        _currentSpeed = speed;
    }

    public async UniTask PlayDialogue(Action OnStart, Action OnPunctuationPause, Action OnPunctuationResume, Action OnComplete)
    {
        SetAnim(true, .3f);
        await UniTask.WaitForSeconds(.3f);

        for (int i = 0; i < _sentences.Count; i++)
        {
            string sentence = _sentences[i];
            _revealingSentence.ClearSentence();
            _revealingSentence.SetPlaySpeed(_currentSpeed);
            _revealingSentence.SetPunctuationStop(true);
            _revealingSentence.SetText(sentence);

            // PlaySentence() 실행 시 개별 상황에 대한 async 메서드 전달
            await _revealingSentence.PlaySentence(
                OnStart: async () =>
                {
                    OnStart?.Invoke();
                    await UniTask.Yield();
                },
                OnPunctuationPause: async () =>
                {
                    OnPunctuationPause?.Invoke();

                    if(!_isSkipping){
                        SpawnArrow(90);
                        await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));
                    }
                },
                OnPunctuationResume: async () =>
                {
                    OnPunctuationResume?.Invoke();
                    DestroyArrow();
                    await UniTask.Yield();
                },
                OnComplete: async () =>
                {
                    OnComplete?.Invoke();
                    SpawnArrow(0);
                    await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));
                    DestroyArrow();
                }
            );
        }
        SetAnim(false, .3f);
        await UniTask.WaitForSeconds(.3f);
        _revealingSentence.ClearSentence();
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.LeftControl | KeyCode.RightControl | KeyCode.LeftCommand | KeyCode.RightCommand)){
            _isSkipping = true;
            ChangeSpeed(_initialSpeed * 5);
        }
        else{
            _isSkipping = false;
            ChangeSpeed(_initialSpeed);
        }
    }

    private void SetAnim(bool b, float duration)
    {
        _isOn = b;
        _parent.gameObject.SetAnim(b, duration);
    }

    public void SkipCurrentDialogue()
    {
        _revealingSentence.CompleteAllInstantly();
    }

    private void SpawnArrow(float degreeZ)
    {
        DestroyArrow();
        _activeArrow = Instantiate(_arrowPrefab, transform);
        _activeArrow.transform.position = _revealingSentence.LastWord.transform.position + Vector3.right * (105f + _revealingSentence.LastWord.RectTransform.sizeDelta.x) - Vector3.one * 30f + Vector3.down * 7;
        _activeArrow.transform.localRotation = Quaternion.Euler(0, 0, degreeZ);
        _activeArrow.Initialize();
    }

    private void DestroyArrow()
    {
        if (_activeArrow != null)
        {
            _activeArrow.DestroyArrow();
        }
    }
}
