using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System;

public class DialoguePlayer : MonoBehaviour
{
    public static DialoguePlayer Instance { get; private set; }

    [Header("Arrow Prefab")]
    [SerializeField] private DialogueArrow _arrowPrefab;
    private DialogueArrow _activeArrow; // 현재 활성화된 화살표 인스턴스 저장

    [Header("Revealing Sentence")]
    [SerializeField] private RevealingSentence _revealingSentence;

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
        _revealingSentence.gameObject.SetAnim(true, .3f);
        await UniTask.WaitForSeconds(.3f);
        for (int i = 0; i < dialogue.Sentences.Count; i++)
        {
            string sentence = dialogue.Sentences[i];
            _revealingSentence.ClearSentence(); 
            _revealingSentence.SetPlaySpeed(dialogue.Speed);
            _revealingSentence.SetPunctuationStop(true);
            _revealingSentence.SetText(sentence);

            

            // PlaySentence() 실행 시 개별 상황에 대한 async 메서드 전달
            await _revealingSentence.PlaySentence(
                OnStart: async () =>
                {
                    Debug.Log("[대기] 시작에서 화살표 파괴");
                    await UniTask.Yield();
                },
                OnPunctuationMet: async () =>
                {
                    Debug.Log("[대기] 구두점에서 마우스 입력을 기다림...");
                    SpawnArrow(-90);
                    await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));
                    DestroyArrow();
                },
                OnComplete: async () =>
                {
                    Debug.Log("[대기] 마지막 문장에서 마우스 입력을 기다림...");
                    SpawnArrow(0);
                    await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0));
                    DestroyArrow();
                }
            );
        }
        _revealingSentence.gameObject.SetAnim(false, .3f);
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
        _activeArrow.transform.position = _revealingSentence.LastWord.transform.position + Vector3.right * 105f - Vector3.one * 30f + Vector3.down * 7;
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
