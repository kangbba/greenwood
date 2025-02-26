using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine.UI;
using UniRx;

public class DialoguePlayer : AnimationImage
{
    [Header("Arrow Prefab")]
    [SerializeField] private DialogueArrow _arrowPrefab;
    [SerializeField] private TextMeshProUGUI _ownerText;
    [SerializeField] private Image _ownerBackground;
    private DialogueArrow _activeArrow;

    [Header("Revealing Sentence")]
    [SerializeField] private RevealingSentence _revealingSentence;
    [SerializeField] private Transform _parent;
    
    private List<string> _sentences;
    private float _initialSpeed;
    private bool _isSkipping;

    private readonly ReactiveProperty<float> _currentSpeedNotifier = new ReactiveProperty<float>();

    public void Init(string ownerName, Color ownerTextColor, Color ownerBackgroundColor, List<string> sentences, float speed)
    {
        _sentences = sentences;
        _initialSpeed = speed;
        _currentSpeedNotifier.Value = _initialSpeed;

        _ownerText.SetText(ownerName);
        _ownerText.color = ownerTextColor;
        _ownerBackground.color = ownerBackgroundColor;

        SetupSpeedListener();
        FadeOut(0f);
    }

    private void SetupSpeedListener()
    {
        KeyboardInputManager.Instance.GetKeyNotifier(KeyboardInputManager.KeyboardActionType.SpeedUp)
            .Subscribe(isSpeedUp =>
            {
                _isSkipping = isSpeedUp;
                _currentSpeedNotifier.Value = isSpeedUp ? _initialSpeed * 5 : _initialSpeed;
            })
            .AddTo(this);
    }

    public async UniTask PlayDialogue(Action OnStart, Action OnPunctuationPause, Action OnPunctuationResume, Action OnComplete)
    {
        FadeIn(.2f);
        await UniTask.WaitForSeconds(.3f);

        for (int i = 0; i < _sentences.Count; i++)
        {
            string sentence = _sentences[i];
            _revealingSentence.ClearSentence();
            _revealingSentence.SetPlaySpeed(_currentSpeedNotifier.Value);
            _revealingSentence.SetPunctuationStop(true);
            _revealingSentence.SetText(sentence);

            await _revealingSentence.PlaySentence(
                OnStart: async () =>
                {
                    OnStart?.Invoke();
                    await UniTask.Yield();
                },
                OnPunctuationPause: async () =>
                {
                    OnPunctuationPause?.Invoke();

                    if (!_isSkipping)
                    {
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
        FadeOut(.2f);
        await UniTask.WaitForSeconds(.3f);
        _revealingSentence.ClearSentence();
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
