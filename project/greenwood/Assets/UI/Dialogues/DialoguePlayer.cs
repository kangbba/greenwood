using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System;
using TMPro;

public class DialoguePlayer : MonoBehaviour
{
    [Header("Arrow Prefab")]
    [SerializeField] private DialogueArrow _arrowPrefab;
    [SerializeField] private TextMeshProUGUI _characterText;
    private DialogueArrow _activeArrow; // 현재 활성화된 화살표 인스턴스 저장

    [Header("Revealing Sentence")]
    [SerializeField] private RevealingSentence _revealingSentence;
    [SerializeField] private Transform _parent;
    private bool _isOn;

    private CharacterSetting _characterSetting;
    private List<string> _sentences;
    private float _speed;

    /// <summary>
    /// 다이얼로그 초기화
    /// </summary>
    public void Init(CharacterName characterName, List<string> sentences, float speed)
    {
        _characterSetting = CharacterManager.Instance.GetCharacterSetting(characterName);
        _sentences = sentences;
        _speed = speed;

        _characterText.SetText(_characterSetting.DisplayName);
        _characterText.color = _characterSetting.CharacterColor;

        SetAnim(false, 0f);  // 초기 상태 숨김
    }

    public async UniTask PlayDialogue()
    {
        SetAnim(true, .3f);
        await UniTask.WaitForSeconds(.3f);

        for (int i = 0; i < _sentences.Count; i++)
        {
            string sentence = _sentences[i];
            _revealingSentence.ClearSentence();
            _revealingSentence.SetPlaySpeed(_speed);
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
                    SpawnArrow(90);
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
        SetAnim(false, .3f);
        await UniTask.WaitForSeconds(.3f);
        _revealingSentence.ClearSentence();
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
