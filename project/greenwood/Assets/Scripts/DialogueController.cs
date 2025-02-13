using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System;

public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance { get; private set; }

    [Header("Revealing Sentence")]
    [SerializeField] private RevealingSentence _revealingSentence;

    [Header("Fade Settings")]
    [SerializeField] private float _fadeDuration = 0.5f;

    [Header("Dialogue Speed")]
    [SerializeField] private float _defaultSpeed = 500;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async UniTask PlayDialogue(Dialogue dialogue)
    {
        Debug.Log("DialogueController :: Fade In Panel");
        await UniTask.Delay(TimeSpan.FromSeconds(_fadeDuration));

        for (int i = 0; i < dialogue.Sentences.Count; i++)
        {
            string sentence = dialogue.Sentences[i];

            _revealingSentence.ClearSentence(); 
            _revealingSentence.SetPlaySpeed(_defaultSpeed);
            _revealingSentence.SetPunctuationStop(true);
            _revealingSentence.SetText(sentence);

            // PlaySentence() 실행 시 개별 상황에 대한 async 메서드 전달
            await _revealingSentence.PlaySentence(
                OnStart: null,
                OnPunctuationMet: async () =>
                {
                    Debug.Log("[대기] 마지막 문장에서 마우스 입력을 기다림...");
                    await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));
                },
                OnComplete: async () =>
                {
                    Debug.Log("[대기] 마지막 문장에서 마우스 입력을 기다림...");
                    await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));
                }
            );
        }
    }

    public void SkipCurrentDialogue()
    {
        _revealingSentence.CompleteAllInstantly();
    }
}
