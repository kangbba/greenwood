using UnityEngine;
using Cysharp.Threading.Tasks;

public class SmallPlace : MonoBehaviour
{
    [Header("SmallPlace Settings")]
    [SerializeField] private ESmallPlaceName smallPlaceName;

    private EBigPlaceName _parentBigPlaceName;

    private const float SHOW_DURATION = 0.5f;  // ✅ Show 애니메이션 시간 (상수)
    private const float HIDE_DURATION = 0.5f;  // ✅ Hide 애니메이션 시간 (상수)

    public ESmallPlaceName SmallPlaceName => smallPlaceName;

    public EBigPlaceName ParentBigPlaceName { get => _parentBigPlaceName; }

    public void Init(EBigPlaceName parentBigPlaceName){
        _parentBigPlaceName = parentBigPlaceName;
    }

    public async UniTask Show()
    {
        gameObject.SetAnimActive(true, SHOW_DURATION);  // ✅ 애니메이션 실행
        await UniTask.WaitForSeconds(SHOW_DURATION);  // ✅ 애니메이션이 끝날 때까지 대기
    }

    public async UniTask Hide()
    {
        gameObject.SetAnimActive(false, HIDE_DURATION);  // ✅ 애니메이션 실행
        await UniTask.WaitForSeconds(HIDE_DURATION);  // ✅ 애니메이션이 끝날 때까지 대기
    }
}
