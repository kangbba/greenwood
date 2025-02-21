using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Sirenix.OdinInspector;

public class Eyes : MonoBehaviour
{
    [SerializeField] private bool _useAnimation = true; // 애니메이션 사용 여부

    private Vector2 closedDurationRange = new Vector2(0.1f, 0.2f);

    private Vector2 openedDurationRange = new Vector2(3f, 5f);

    [ShowIf("_useAnimation")]
    [SerializeField] private GameObject _closedObj;

    [ShowIf("_useAnimation")]
    [SerializeField] private GameObject _openedObj;

    private CancellationTokenSource _cts;
    private bool _isEyesOpened = true; // 눈 뜬 상태에서 시작

    /// <summary>
    /// 눈 감기/뜨기 상태 전환
    /// </summary>
    private void Toggle()
    {
        if (!_useAnimation) return; // 애니메이션을 사용하지 않으면 실행 안 함
        SetOpen(!_isEyesOpened);
    }

    /// <summary>
    /// 눈을 뜨거나 감는 상태 설정
    /// </summary>
    private void SetOpen(bool isOpen)
    {
        if (!_useAnimation) return; // 애니메이션을 사용하지 않으면 실행 안 함
        _isEyesOpened = isOpen;

        if (!_useAnimation) return; // 애니메이션을 사용하지 않으면 바로 종료

        if(_closedObj != null)
        {
            _closedObj.SetActive(!isOpen);
        }
        if(_openedObj != null)
        {
            _openedObj.SetActive(isOpen);
        }
    }

   public async UniTaskVoid Play()
    {
        if (!_useAnimation) return; // 애니메이션을 사용하지 않으면 실행 안 함


        _cts = new CancellationTokenSource();
        var token = _cts.Token;

        try
        {
            while (!token.IsCancellationRequested)
            {
                if (_isEyesOpened)
                {
                    SetOpen(false); // 눈 감기
                    await UniTask.Delay(TimeSpan.FromSeconds(UnityEngine.Random.Range(closedDurationRange.x, closedDurationRange.y)), cancellationToken: token);
                }
                else
                {
                    SetOpen(true); // 눈 뜨기
                    await UniTask.Delay(TimeSpan.FromSeconds(UnityEngine.Random.Range(openedDurationRange.x, openedDurationRange.y)), cancellationToken: token);
                }
            }
        }
        catch (OperationCanceledException)
        {
            // 예외 처리 (필요 시 추가 가능)
        }
    }

    /// <summary>
    /// 눈 깜빡임 중단 후 기본 상태 유지 (눈 뜬 상태 보장)
    /// </summary>
    public void Stop()
    {
        if (!_useAnimation) return; // 애니메이션을 사용하지 않으면 실행 안 함
        if (_cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }

        SetOpen(true); // 눈을 뜬 상태로 종료 보장
    }

    /// <summary>
    /// 눈 감기/뜨기 미리보기 버튼 (Sirenix Odin)
    /// </summary>
    [Button("👀 눈 깜빡이기 미리보기", ButtonSizes.Large)]
    private void PreviewBlink()
    {
        Toggle();
    }
}
