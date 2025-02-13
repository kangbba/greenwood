using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour
{
    public TextMeshProUGUI messageText;  // 메시지 텍스트
    public Button yesButton;  // '예' 버튼 (또는 '확인')
    public Button noButton;   // '아니오' 버튼 (필요 없을 경우 숨김)
    public RectTransform buttonContainer; // 버튼 정렬을 위한 부모 컨테이너

    private UniTaskCompletionSource<bool?> tcs;  // 버튼 선택 결과를 반환하는 비동기 Task

    /// <summary>
    /// 다이얼로그 초기화 및 결과 반환
    /// </summary>
    /// 
    public async UniTask<bool?> Initialize(string message, string yesText, string noText = null)
    {
        // 메시지 설정
        if (messageText != null)
        {
            messageText.text = message;
        }

        tcs = new UniTaskCompletionSource<bool?>();

        // '확인' 버튼(1개 버튼만 있는 경우)
        if (string.IsNullOrEmpty(noText))
        {
            yesButton.GetComponentInChildren<TextMeshProUGUI>().text = yesText;
            yesButton.onClick.RemoveAllListeners();
            yesButton.onClick.AddListener(() => tcs.TrySetResult(true));

            // '아니오' 버튼 숨김 및 '확인' 버튼 중앙 정렬
            noButton.gameObject.SetActive(false);
            yesButton.transform.SetParent(buttonContainer);
        }
        else
        {
            // '예 / 아니오' 버튼 (2개 버튼 지원)
            yesButton.GetComponentInChildren<TextMeshProUGUI>().text = yesText;
            yesButton.onClick.RemoveAllListeners();
            yesButton.onClick.AddListener(() => tcs.TrySetResult(true));

            noButton.GetComponentInChildren<TextMeshProUGUI>().text = noText;
            noButton.onClick.RemoveAllListeners();
            noButton.onClick.AddListener(() => tcs.TrySetResult(false));

            // '아니오' 버튼 활성화
            noButton.gameObject.SetActive(true);
        }

        // 버튼 클릭 결과 반환하며 대기
        bool? result = await tcs.Task;

        gameObject.SetAnimDestroy(.5f); // 다이얼로그 종료 후 제거
        return result;
    }
}
