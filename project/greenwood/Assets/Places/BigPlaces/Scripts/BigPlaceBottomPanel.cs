// using UnityEngine;
// using UnityEngine.UI;

// public class BigPlaceBottomPanel : MonoBehaviour
// {
//     [SerializeField] private Button _returnButton; // ✅ 돌아가기 버튼
//     [SerializeField] private Button _leaveButton; // ✅ 외출 버튼

//     /// <summary>
//     /// 패널 활성화 및 버튼 설정 (애니메이션 실행 시간 포함)
//     /// </summary>
//     public void SetupPanel(bool showReturnButton, bool showLeaveButton, float duration)
//     {
//         // ✅ 버튼 활성화 여부 설정
//         _returnButton.gameObject.SetActive(showReturnButton);
//         _leaveButton.gameObject.SetActive(showLeaveButton);

//         // ✅ 애니메이션 적용 (false → true 상태로 duration 동안 페이드 인)
//         gameObject.SetAnim(false, true, duration);
//     }

//     /// <summary>
//     /// 돌아가기 버튼 클릭 이벤트 설정
//     /// </summary>
//     public void SetReturnAction(System.Action onReturn)
//     {
//         _returnButton.onClick.RemoveAllListeners();
//         _returnButton.onClick.AddListener(() => onReturn?.Invoke());
//     }

//     /// <summary>
//     /// 외출 버튼 클릭 이벤트 설정
//     /// </summary>
//     public void SetLeaveAction(System.Action onLeave)
//     {
//         _leaveButton.onClick.RemoveAllListeners();
//         _leaveButton.onClick.AddListener(() => onLeave?.Invoke());
//     }

//     /// <summary>
//     /// 패널 제거
//     /// </summary>
//     public void DestroyPanel()
//     {
//         Destroy(gameObject);
//     }
// }
